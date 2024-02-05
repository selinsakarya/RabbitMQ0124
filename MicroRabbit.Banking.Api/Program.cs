using System.Reflection;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Infra.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BankingDbContext>(o =>
{
    o.UseInMemoryDatabase(builder.Configuration.GetConnectionString("BankingDb") ??
                          throw new InvalidOperationException());
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Banking Microservice", Version = "v1" });
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers();

DependencyContainer.RegisterServices(builder.Services);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking Microservice V1"); });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();