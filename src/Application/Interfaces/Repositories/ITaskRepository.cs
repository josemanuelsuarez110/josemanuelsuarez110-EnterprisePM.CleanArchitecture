using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProjectManagementERP.Domain.Entities;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PagedResult<TaskItem>> GetByProjectAsync(Guid projectId, int page, int pageSize, CancellationToken cancellationToken = default);
        Task AddAsync(TaskItem task, CancellationToken cancellationToken = default);
        void Update(TaskItem task);
    }
}
