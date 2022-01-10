using Conductor.Domain.Models;

namespace Conductor.AMQP.Contracts
{
    public interface CreateDefinition
    {
        Definition Definition { get; }
    }
}
