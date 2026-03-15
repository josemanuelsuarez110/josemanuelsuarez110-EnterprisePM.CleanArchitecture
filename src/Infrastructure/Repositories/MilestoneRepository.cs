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
    public class MilestoneRepository : IMilestoneRepository
    {
        private readonly AppDbContext _context;

        public MilestoneRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Milestone milestone, CancellationToken cancellationToken = default)
        {
            await _context.Milestones.AddAsync(milestone, cancellationToken);
        }

        public async Task<Milestone?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Milestones.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Milestone>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken = default)
        {
            return await _context.Milestones.Where(m => m.ProjectId == projectId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public void Update(Milestone milestone)
        {
            _context.Milestones.Update(milestone);
        }
    }
}
