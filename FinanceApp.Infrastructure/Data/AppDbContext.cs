using Microsoft.EntityFrameworkCore;
using FinanceApp.Domain.Entities;

namespace FinanceApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<IncomeRecords> IncomeRecords => Set<IncomeRecords>();
    }
}
