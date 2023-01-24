using ProductionScheduler.Application;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core;
using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Infrastructure;
using ProductionScheduler.Infrastructure.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services

    .AddSingleton<IClock, Clock>()
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration) 
    .AddControllers();

builder.UseSerilog();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseInfrastructure(); 

app.Run();