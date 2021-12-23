using System.Threading.Tasks;
using Conductor.Domain.Interfaces;
using MassTransit;
using Conductor.Contracts;

namespace Conductor.Consumers
{
    class CreateDefinitionConsumer : IConsumer<CreateDefinition>
    {
        private readonly IDefinitionService _service;

        public CreateDefinitionConsumer(IDefinitionService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<CreateDefinition> context)
        {
            _service.RegisterNewDefinition(context.Message.Definition);
        }
    }
}