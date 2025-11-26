using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Persistence.Repositories
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
            // İşte veritabanına gidiş anı burasıdır!
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
