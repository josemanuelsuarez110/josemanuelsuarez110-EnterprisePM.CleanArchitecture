using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagementERP.Application.Interfaces.Repositories;
using ProjectManagementERP.Domain.Entities;
using ProjectManagementERP.Infrastructure.Data;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskItem task, CancellationToken cancellationToken = default)
        {
            await _context.Tasks.AddAsync(task, cancellationToken);
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks.Include(t => t.TimeEntries).FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<PagedResult<TaskItem>> GetByProjectAsync(Guid projectId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.Tasks.Where(t => t.ProjectId == projectId);
            var total = await query.CountAsync(cancellationToken);
            var items = await query.OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(t => t.TimeEntries)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new PagedResult<TaskItem>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }

        public void Update(TaskItem task)
        {
            _context.Tasks.Update(task);
        }
    }
}
