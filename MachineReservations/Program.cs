using ProductionScheduler.Application;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core;
using ProductionScheduler.Core.Entities;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Core.ValueObjects;
using ProductionScheduler.Infrastructure;
using ProductionScheduler.Infrastructure.DAL;

var builder = WebApplication.CreateBuilder(args);
builder.Services

    .AddSingleton<IClock, Clock>()
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)//sql wolasz razem z cala infrastruktura
    .AddControllers();


builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();



app.Run();