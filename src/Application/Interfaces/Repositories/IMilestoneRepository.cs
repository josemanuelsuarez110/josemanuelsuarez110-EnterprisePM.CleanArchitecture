using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProjectManagementERP.Domain.Entities;

namespace ProjectManagementERP.Application.Interfaces.Repositories
{
    public interface IMilestoneRepository
    {
        Task<Milestone?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Milestone>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task AddAsync(Milestone milestone, CancellationToken cancellationToken = default);
        void Update(Milestone milestone);
    }
}
