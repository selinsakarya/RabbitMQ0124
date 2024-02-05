using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.v1.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAccounts()
    {
        List<Account> accounts = await _accountService.GetAccounts();

        return Ok(accounts);
    }
    
    [HttpPost("transfer-accounts")]
    public Task<IActionResult> TransferAccounts()
    {
        return Task.FromResult<IActionResult>(Ok("selin"));
    }
}