namespace Conductor.AMQP.Contracts
{
    public interface PublishEvent
    {
        string Name { get; }
        string Key { get; }
        object Data { get; }
    }
}
