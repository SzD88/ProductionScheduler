using ProductionScheduler.Infrastructure;
using ProductionScheduler.Infrastructure.Logging;
using Serilog;
using ProductionScheduler.Api.Installers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServicesInAssembly(builder.Configuration);

builder.UseSerilog();

var app = builder.Build();

app.UseInfrastructure();

app.Run();