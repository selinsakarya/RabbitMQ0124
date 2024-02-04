using System.Reflection;
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

builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

DependencyContainer.RegisterServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/accounts", async (IAccountService accountService) =>
    {
        List<Account> accounts = await accountService.GetAccounts();

        return accounts;
    })
    .WithOpenApi();

app.Run();

