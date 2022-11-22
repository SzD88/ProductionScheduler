

using MachineReservations.Api.Entities;
using MachineReservations.Api.Services;
using MachineReservations.Api.ValueObjects;

var builder = WebApplication.CreateBuilder(args);
builder.Services

    .AddSingleton<IClock, Clock>()
    .AddSingleton<IEnumerable<WeeklyMachineReservation>>(
        serviceProvider =>
        {
            var clock = serviceProvider.GetService<IClock>();
            return new List<WeeklyMachineReservation>()
         {
        new  (Guid.Parse("00000000-0000-0000-0000-000000000001"), new ReservationTimeForward(clock.Current()), "P1"),
        new  (Guid.Parse("00000000-0000-0000-0000-000000000002"), new ReservationTimeForward(clock.Current()), "P2"),
        new  (Guid.Parse("00000000-0000-0000-0000-000000000003"), new ReservationTimeForward(clock.Current()), "P3"),
        new  (Guid.Parse("00000000-0000-0000-0000-000000000004"), new ReservationTimeForward(clock.Current()), "P4"),
        new  (Guid.Parse("00000000-0000-0000-0000-000000000005"), new ReservationTimeForward(clock.Current()), "P5")
         };
        })
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