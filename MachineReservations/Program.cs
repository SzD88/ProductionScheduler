using ProductionScheduler.Application;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core;
using ProductionScheduler.Core.Repositories;
using ProductionScheduler.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services

    .AddSingleton<IClock, Clock>()
    .AddCore()
    .AddApplication()
    .AddInfrastructure()
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