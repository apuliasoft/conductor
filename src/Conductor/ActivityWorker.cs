using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace Conductor
{
    // TODO: questo worker affossa le performance HTTP del server, si può fare meglio?
    // considerare che con AMQP abilitato, HTTP potrebbe servire solo per definire i workflow (perchè piu stabile)
    public class ActivityWorker : BackgroundService
    {
        readonly IBus _bus;
        IActivityController _activityService;

        public ActivityWorker(IBus bus, IActivityController activityService)
        {
            _bus = bus;
            _activityService = activityService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var activities = await _activityService.GetPendingActivities();
                if(activities != null)
                {
                    var activitySendTasks = activities.Select(async a =>
                        await _bus.Publish<AMQP.Contracts.PendingActivity>(new
                        {
                            Token = a.Token,
                            ActivityName = a.ActivityName,
                            Parameters = a.Parameters,
                            TokenExpiry = a.TokenExpiry
                        })
                    )
                    .ToList();

                    await Task.WhenAll(activitySendTasks);
                }

                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
