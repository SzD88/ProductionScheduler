using ProductionScheduler.Application;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core;
using ProductionScheduler.Core.Abstractions;
using ProductionScheduler.Infrastructure;
using ProductionScheduler.Infrastructure.Logging;
using Serilog;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; //#refactor wywalic 

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

builder.Services

    .AddSingleton<IClock, Clock>()
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddControllers();

builder.UseSerilog();

// builder.Services.AddSwaggerGen();


var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseInfrastructure();
app.UseCors(MyAllowSpecificOrigins);

app.Run();