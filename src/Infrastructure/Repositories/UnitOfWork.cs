using System.Threading;
using System.Threading.Tasks;
using ProjectManagementERP.Application.Interfaces.Repositories;
using ProjectManagementERP.Infrastructure.Data;

namespace ProjectManagementERP.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
