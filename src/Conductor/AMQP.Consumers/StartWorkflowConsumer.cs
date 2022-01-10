using System.Threading.Tasks;
using MassTransit;
using Conductor.AMQP.Contracts;
using WorkflowCore.Interface;

namespace Conductor.AMQP.Consumers
{
    class StartWorkflowConsumer : IConsumer<StartWorkflow>
    {
        private readonly IWorkflowController _workflowController;

        public StartWorkflowConsumer(IWorkflowController workflowController)
        {
            _workflowController = workflowController;
        }

        public async Task Consume(ConsumeContext<StartWorkflow> context)
        {
            await _workflowController.StartWorkflow(context.Message.Id, context.Message.Data);
        }
    }
}