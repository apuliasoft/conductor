﻿using System.Threading.Tasks;

namespace Conductor.Domain.Interfaces
{
    public interface ISubscriptionsRepository
    {
        Task TerminateOrphans();
    }
}
