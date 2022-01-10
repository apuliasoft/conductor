using System;

namespace Conductor.AMQP.Contracts
{
    public interface PendingActivity
    {
        string Token { get; }
        string ActivityName { get; }
        object Parameters { get; }
        DateTime TokenExpiry { get; }
    }
}
