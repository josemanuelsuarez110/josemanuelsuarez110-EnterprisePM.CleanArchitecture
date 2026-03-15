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
    public class ResourceAllocationRepository : IResourceAllocationRepository
    {
        private readonly AppDbContext _context;

        public ResourceAllocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ResourceAllocation allocation, CancellationToken cancellationToken = default)
        {
            await _context.ResourceAllocations.AddAsync(allocation, cancellationToken);
        }

        public async Task<ResourceAllocation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.ResourceAllocations.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<ResourceAllocation>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken = default)
        {
            return await _context.ResourceAllocations.Where(r => r.ProjectId == projectId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public void Update(ResourceAllocation allocation)
        {
            _context.ResourceAllocations.Update(allocation);
        }
    }
}
