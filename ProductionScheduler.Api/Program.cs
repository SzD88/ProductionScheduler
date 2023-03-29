using ProductionScheduler.Application;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core;
using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Infrastructure;
using ProductionScheduler.Infrastructure.Logging;
using Serilog;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{

    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy
                           .WithOrigins("http://localhost:4200/")
                            .SetIsOriginAllowed((host) => true);
                          policy // .AllowAnyOrigin()
                                .AllowAnyMethod()
                               .AllowAnyHeader();
                          //  .AllowCredentials();
                      });
});
builder.Services.AddMemoryCache(); // to powinnjo isc do jakiegos exdtensiona z API jak sa extensiony innych warstw #refactor

builder.Services
    .AddSingleton<IClock, Clock>() // jw
     
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddControllers();

builder.UseSerilog();

var app = builder.Build();


app.UseInfrastructure();
app.UseCors(MyAllowSpecificOrigins);

app.Run();