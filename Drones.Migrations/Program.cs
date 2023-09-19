using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Drones.Migrations;

public static class Program
{
    static async Task Main(string[] args)
    {
        await RunCommand(args, "Host=127.0.0.1; Port=5432; Database=postgres; Username=user; Password=admin");
    }

    private static IServiceProvider CreateServices(string connectionString)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(Program).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }
    static async Task RunCommand(string[] args, string conString)
        {
            var serviceProvider = CreateServices(conString);
            using var scope = serviceProvider.CreateScope();
            var input = args.Length == 0 ? "" : args[0];
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            long? version = (args.Length > 1 && long.TryParse(args[1], out var v)) ? v : null;
            switch (input)
            {
                case "up":
                {
                    runner.MigrateUp();
                    break;
                }
                case "down":
                {
                    if (version > 0)
                    {
                        runner.MigrateDown(version.Value);
                    }
                    else
                    {
                        runner.MigrateDown(0);
                    }
                    break;
                }
                case "rollback":
                {
                    if (version > 0)
                    {
                        runner.RollbackToVersion(version.Value);
                    }
                    else runner.Rollback(1);
                    break;
                }
                case "new":
                {
                    if (args.Length <= 1)
                    {
                        Console.WriteLine("File name is required.");
                        return;
                    }

                    var filename = args[1];
                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Migrations", $"{timestamp}_{filename}.cs");

                    var template = $@"using FluentMigrator;
namespace Drones.Migrations.Migrations;

[Migration(" + timestamp +
@")]
public class "+filename+@" : Migration
{
    public override void Up()
    {
        
    }

    public override void Down()
    {
        
    }
}";
                    await File.WriteAllTextAsync(path, template);
                    break;
                }
                default:
                {
                    Console.WriteLine("Unsupported command");
                    break;
                }
            }
        }

        public static void RunUpCommand(string conString)
        {
            var serviceProvider = CreateServices(conString);
            using var scope = serviceProvider.CreateScope();
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }