using MachineReservations.Api.Entities;
using MachineReservations.Api.Services;
using MachineReservations.Api.ValueObjects;
using MachineReservations.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services

    .AddSingleton<IClock, Clock>()
    .AddSingleton<IPeriodMachineReservationRepository, InMemoryPeriodMachineReservationRepository>()
     
    .AddSingleton<IReservationService, ReservationService>()

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