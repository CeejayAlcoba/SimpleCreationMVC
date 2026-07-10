using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Models;

namespace ApplicationContexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<__EFMigrationsHistory> __EFMigrationsHistory { get; set; }
        public DbSet<Authority> Authority { get; set; }
        public DbSet<ControlNumber> ControlNumber { get; set; }
        public DbSet<DemeritRecord> DemeritRecord { get; set; }
        public DbSet<TestTbl> TestTbl { get; set; }
        public DbSet<TouringDeduction> TouringDeduction { get; set; }
        public DbSet<Trainee> Trainee { get; set; }
    }
}