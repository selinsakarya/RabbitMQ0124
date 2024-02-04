using MicroRabbit.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Banking.Data.Context;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().HasData(
            new Account
            {
                Id = 1,
                Type = "USD",
                Balance = 1000
            });
        
        modelBuilder.Entity<Account>().HasData(
            new Account
            {
                Id = 2,
                Type = "Saving",
                Balance = 2000
            });
    }
}