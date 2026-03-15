using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagementERP.Application.Interfaces.Repositories;
using ProjectManagementERP.Domain.Entities;
using ProjectManagementERP.Domain.Enums;
using ProjectManagementERP.Infrastructure.Data;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Project project, CancellationToken cancellationToken = default)
        {
            await _context.Projects.AddAsync(project, cancellationToken);
        }

        public void Delete(Project project)
        {
            _context.Projects.Remove(project);
        }

        public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .Include(p => p.Milestones)
                .Include(p => p.ResourceAllocations)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<PagedResult<Project>> GetPagedAsync(int page, int pageSize, string? search, ProjectStatus? status, CancellationToken cancellationToken = default)
        {
            var query = _context.Projects.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status);
            }

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new PagedResult<Project>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }

        public void Update(Project project)
        {
            _context.Projects.Update(project);
        }
    }
}
