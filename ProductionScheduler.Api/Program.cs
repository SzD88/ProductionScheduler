using ProductionScheduler.Application;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core;
using ProductionScheduler.Infrastructure;

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

app.Use(async (ctx, next) =>
{
    Console.WriteLine("step 1 otworz ");
    await next(ctx);
    if (ctx.Request.Headers)
    {

    }
    Console.WriteLine("step 1  zamknij"); 
});
app.Use(async (ctx, next) =>
{
    Console.WriteLine("step 2 otworz ");
    await next(ctx);
    Console.WriteLine("step 2  zamknij");
});
app.MapControllers();

app.Run();