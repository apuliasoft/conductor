using System.Threading.Tasks;
using MassTransit;
using Conductor.AMQP.Contracts;
using WorkflowCore.Interface;

namespace Conductor.AMQP.Consumers
{
    class PublishEventConsumer : IConsumer<PublishEvent>
    {
        private readonly IWorkflowController _workflowController;

        public PublishEventConsumer(IWorkflowController workflowController)
        {
            _workflowController = workflowController;
        }

        public async Task Consume(ConsumeContext<PublishEvent> context)
        {
            await _workflowController.PublishEvent(context.Message.Name, context.Message.Key, context.Message.Data);
        }
    }
}