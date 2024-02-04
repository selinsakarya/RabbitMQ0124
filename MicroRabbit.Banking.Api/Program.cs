using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Infra.IoC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BankingDbContext>(o =>
{
    o.UseInMemoryDatabase(builder.Configuration.GetConnectionString("BankingDb") ?? throw new InvalidOperationException());
});

DependencyContainer.RegisterServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/accounts", async (IAccountService accountService, BankingDbContext bankingDbContext) =>
    {
        List<Account> accountsViaService = await accountService.GetAccounts();

        List<Account> accountsViaDbContextDirectly = await bankingDbContext.Accounts.ToListAsync();
        
        return accountsViaDbContextDirectly;
    })
    .WithOpenApi();

app.Run();

