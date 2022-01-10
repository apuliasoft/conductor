using System.Dynamic;

namespace Conductor.AMQP.Contracts
{
    public interface StartWorkflow
    {
        string Id { get; }
        ExpandoObject Data { get; }
    }
}
