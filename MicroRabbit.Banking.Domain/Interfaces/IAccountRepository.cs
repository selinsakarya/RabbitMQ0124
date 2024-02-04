using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Domain.Interfaces;

public interface IAccountRepository
{
    Task<List<Account>> GetAccounts();
}