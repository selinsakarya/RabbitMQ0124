using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Banking.Data.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly BankingDbContext _context;

    public AccountRepository(BankingDbContext context)
    {
        _context = context;
        
        // For seeding in-memory db.
        _context.Database.EnsureCreated();
    }
    
    public async Task<List<Account>> GetAccounts()
    {
        return await _context.Accounts.ToListAsync();
    }
}