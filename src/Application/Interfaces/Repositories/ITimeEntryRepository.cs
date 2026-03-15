using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProjectManagementERP.Domain.Entities;

namespace ProjectManagementERP.Application.Interfaces.Repositories
{
    public interface ITimeEntryRepository
    {
        Task<IEnumerable<TimeEntry>> GetByTaskAsync(Guid taskId, CancellationToken cancellationToken = default);
        Task<IEnumerable<TimeEntry>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<IEnumerable<TimeEntry>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddAsync(TimeEntry timeEntry, CancellationToken cancellationToken = default);
    }
}
