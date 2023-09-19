using AutoMapper;
using Drones.DAL.DbContext;
using Drones.DAL.Repository.Implementation;
using Drones.DAL.Repository.Interfaces;
using Drones.Migrations.Migrations;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;

namespace Drones.DAL.Test.RepositoryTests;

public class MedicationRepositoryTest : IAsyncLifetime
{

        private DroneDbContext _dbContext;
        private IMedicationRepository _medicationRepository;
        private IMapper _mapper;
        private readonly ILogger<MedicationRepository> _logger;
        
        public MedicationRepositoryTest()
        {
           
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<MedicationRepository>();
        }
        
        
        private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

        public async Task InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();

            _dbContext = CreateDbContext();

            _medicationRepository = new MedicationRepository(_dbContext);
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
        public async Task CreateMedicationAsync_ShouldCreateMedication()
        {
            // Arrange
            SeedTestData();
            var medication = new Medication
            {
                Name = "Test Medication",
                Weight = 10.5,
                Code = "TEST123",
                ImagePath = "https://example.com/image.jpg",
                DroneId = 101 // Assign a valid DroneId that exists in the database
            };

            // Act
            var createdMedication = await _medicationRepository.CreateMedicationAsync(medication);

            // Assert
            Assert.NotNull(createdMedication);
            Assert.NotEqual(0, createdMedication.Id); // Ensure the ID is populated
        }

        [Fact]
        public async void GetLoadedMedications_ShouldReturnLoadedMedications()
        {
            // Arrange
            SeedTestData(); 

            // Act
            var loadedMedications = await _medicationRepository.GetLoadedMedications();

            // Assert
            Assert.NotNull(loadedMedications);
            Assert.NotEmpty(loadedMedications);
        }

        private void SeedTestData()
        {
            if (!_dbContext.Drones.Any())
            {
                var drones = new List<Drone>
                {
                    new Drone 
                    {  Id = 101,
                        SerialNumber = "X101",
                        Model = "Lightweight",
                        BatteryCapacity = 80,
                        State = DroneState.IDLE,
                        WeightLimit = 500.0
                        
                    },
                };
        
                _dbContext.Drones.AddRange(drones);
                _dbContext.SaveChanges();
            }

            var testData = new List<Medication>
            {
                new Medication
                {
                    Name = "Test Medication 1",
                    Weight = 5.0,
                    Code = "TEST001",
                    ImagePath = "https://example.com/image2.jpg",
                    DroneId = 101
                },
                new Medication
                {
                    Name = "Test Medication 2",
                    Weight = 7.5,
                    Code = "TEST002",
                    ImagePath = "https://example.com/image3.jpg",
                    DroneId = 101
                }
            };

            _dbContext.Medications.AddRange(testData);
            _dbContext.SaveChanges();
        }

        public async Task DisposeAsync()
        {
            await _postgreSqlContainer.DisposeAsync();
        }
        
}