using ProductionScheduler.Infrastructure;
using ProductionScheduler.Infrastructure.Logging;
using Serilog;
using WebAPI.Installers;

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
                          policy
                             .AllowAnyMethod()
                             .AllowAnyHeader();
                      });
});

builder.Services.InstallServicesInAssembly(builder.Configuration);

builder.UseSerilog();

var app = builder.Build();

app.UseInfrastructure();
app.UseCors(MyAllowSpecificOrigins);

app.Run();