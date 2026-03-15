using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagementERP.Application.Interfaces.Repositories;
using ProjectManagementERP.Domain.Entities;
using ProjectManagementERP.Infrastructure.Data;

namespace ProjectManagementERP.Infrastructure.Repositories
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly AppDbContext _context;

        public TimeEntryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TimeEntry timeEntry, CancellationToken cancellationToken = default)
        {
            await _context.TimeEntries.AddAsync(timeEntry, cancellationToken);
        }

        public async Task<IEnumerable<TimeEntry>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken = default)
        {
            return await _context.TimeEntries
                .Include(t => t.Task)
                .Where(t => t.Task.ProjectId == projectId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TimeEntry>> GetByTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
        {
            return await _context.TimeEntries
                .Include(t => t.Task)
                .Where(t => t.TaskId == taskId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TimeEntry>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.TimeEntries
                .Include(t => t.Task)
                .Where(t => t.UserId == userId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
