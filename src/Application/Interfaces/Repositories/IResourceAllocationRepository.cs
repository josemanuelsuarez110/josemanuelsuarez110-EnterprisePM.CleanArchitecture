using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProjectManagementERP.Domain.Entities;

namespace ProjectManagementERP.Application.Interfaces.Repositories
{
    public interface IResourceAllocationRepository
    {
        Task<IEnumerable<ResourceAllocation>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<ResourceAllocation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(ResourceAllocation allocation, CancellationToken cancellationToken = default);
        void Update(ResourceAllocation allocation);
    }
}
