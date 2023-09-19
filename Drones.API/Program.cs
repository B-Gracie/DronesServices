using AutoMapper;
using Drones.DAL;
using Drones.DAL.DbContext;
using Drones.DAL.DronesDTO;
using Drones.DAL.MappingProfiles;
using Drones.DAL.Repository.Implementation;
using Drones.DAL.Repository.Interfaces;
using Drones.Services.Implementation;
using Drones.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
    .AddJsonFile("app" + "settings.json")
    .Build();


builder.Services.AddDbContext<DroneDbContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("DronesServices"));
});

MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<DronesProfile>();
    cfg.AddProfile<MedicationProfile>();
    cfg.AddProfile<LoadedMedicationProfile>();
});
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddLogging();

builder.Services.AddScoped<IDroneRepository, DroneRepository>();
builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();
builder.Services.AddScoped<IBatteryLogRepository, BatteryLogRepository>();

builder.Services.AddHostedService<BatteryLevelCheckService>();

builder.Services.AddScoped<IDroneService, DronesService>();
builder.Services.AddScoped< IMedicationService, MedicationService >();
builder.Services.AddScoped<IBatteryLogService, BatteryLogService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }