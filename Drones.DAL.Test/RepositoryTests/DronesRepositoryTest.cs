using AutoMapper;
using Drones.DAL.DbContext;
using Drones.DAL.DronesDTO;
using Drones.DAL.Repository.Implementation;
using Drones.DAL.Repository.Interfaces;
using Drones.Migrations.Migrations;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;

namespace Drones.DAL.Test.RepositoryTests;

public class DronesRepositoryTest : IAsyncLifetime
{
        private DroneDbContext _dbContext;
        private IDroneRepository _droneRepository;
        private IMapper _mapper;
        private readonly ILogger<DroneRepository> _logger;
        
        public DronesRepositoryTest()
        {
           
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<DroneRepository>();
        }
        


        private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

        public async Task InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();

            _dbContext = CreateDbContext();

            _droneRepository = new DroneRepository(_dbContext, _mapper, _logger);
        }

        private void SeedTestData()
        {
            var testData = new List<Drone>
            {
                new Drone
                {
                    Id = 101,
                    SerialNumber = "X101",
                    Model = "Lightweight",
                    BatteryCapacity = 80,
                    State = DroneState.IDLE,
                    WeightLimit = 500.0
                }
            };

            _dbContext.Drones.AddRange(testData);
            _dbContext.SaveChanges();

        }


        private DroneDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<DroneDbContext>()
                .UseNpgsql(_postgreSqlContainer.GetConnectionString())
                .Options;

            var dbContext = new DroneDbContext(options);
            
            var serviceProvider = CreateServices();
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }

            return dbContext;
        }

        
        private IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(_postgreSqlContainer.GetConnectionString())
                    .ScanIn(typeof(DronesTable).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
        
        [Fact]
        public async Task CreateAndGetDroneAsync_ShouldCreateAndRetrieveDrone()
        {
            // Arrange
            var newDrone = new Drone
            {
                Id =  101,
                SerialNumber = "X101",
                Model = "Lightweight",
                State = DroneState.IDLE
            };

            // Act
            var createdDrone = await _droneRepository.CreateDroneAsync(newDrone);

            // Assert
            Assert.NotNull(createdDrone);
            Assert.NotEqual(0, createdDrone.Id); // Ensure the ID is populated

            // Retrieve the created drone from the database
            var retrievedDrone = await _droneRepository.GetDroneByIdAsync(createdDrone.Id);

            Assert.NotNull(retrievedDrone);
            Assert.Equal("X101", retrievedDrone.SerialNumber);
        }
        
        [Fact]
        public async Task IsDroneAvailableForLoadingAsync_ReturnsTrue_WhenDroneIsAvailable()
        {
            SeedTestData();
            // Arrange
            int droneId = 101;
            double medicationWeight = 10.0;
            int batteryCapacity = 80;

            // Act
            var isAvailable = await _droneRepository.IsDroneAvailableForLoadingAsync(droneId, medicationWeight);

            // Assert
            Assert.True(isAvailable, "The drone should be available for loading.");
        } 
        
        [Fact]
        public async Task IsDroneAvailableForLoadingAsync_ReturnsFalse_WhenDroneIsNotAvailable()
        {
            // Arrange
            int droneId = 101;
            double medicationWeight = 10.0;
            int batteryCapacity = 20; // Set the battery capacity

            // Act
            bool isNotAvailable = await _droneRepository.IsDroneAvailableForLoadingAsync(droneId, medicationWeight);

            // Assert
            Assert.False(isNotAvailable, "The drone is not available for loading.");
        }


        [Fact]
        public async Task UpdateDroneBatteryCapacityAsync_UpdatesBatteryCapacity()
        {
            // Arrange
            SeedTestData();
            var droneId = 101;
            var newBatteryCapacity = 80; // Adjust the capacity as needed

            // Act
            await _droneRepository.UpdateDroneBatteryCapacityAsync(droneId, newBatteryCapacity);

            // Assert
            var drone = await _dbContext.Drones.FindAsync(droneId);

            // Add an additional assertion to ensure the 'drone' object is not null
            Assert.NotNull(drone);


            if (drone != null)
            {
                Assert.Equal(newBatteryCapacity, drone.BatteryCapacity);
            }
        }


        [Fact]
        public async Task UpdateDroneStateAsync_UpdatesDroneState()
        {
            // Arrange
            SeedTestData(); 

            var droneId = 101;
            var updatedState = DroneState.LOADING;

            // Act
            await _droneRepository.UpdateDroneStateAsync(droneId, updatedState);

            // Assert
            var drone = await _dbContext.Drones.FindAsync(droneId);
            
            Assert.NotNull(drone);
    
            // Ensure that the drone's state is updated correctly
            Assert.Equal(updatedState, drone.State);
        }
        
        
        public async Task DisposeAsync()
        {
            await _postgreSqlContainer.DisposeAsync();
        }
}