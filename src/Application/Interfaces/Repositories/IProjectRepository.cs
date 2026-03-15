using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectManagementERP.Domain.Entities;
using ProjectManagementERP.Domain.Enums;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PagedResult<Project>> GetPagedAsync(int page, int pageSize, string? search, ProjectStatus? status, CancellationToken cancellationToken = default);
        Task AddAsync(Project project, CancellationToken cancellationToken = default);
        void Update(Project project);
        void Delete(Project project);
    }
}
