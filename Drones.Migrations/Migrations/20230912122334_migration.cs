using FluentMigrator;
namespace Drones.Migrations.Migrations;

[Migration(20230912122334)]
public class DronesTable : Migration
{
    public override void Up()
        {
            Create.Schema("DronesServices");

            Create.Table("Drones").InSchema("DronesServices")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("SerialNumber").AsString(100).NotNullable()
                .WithColumn("Model").AsString(50)
                .WithColumn("WeightLimit").AsDouble().NotNullable()
                .WithColumn("BatteryCapacity").AsInt32().NotNullable()
                .WithColumn("State").AsInt32().NotNullable();

            Create.Table("Medications").InSchema("DronesServices")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("Weight").AsDouble().NotNullable()
                .WithColumn("DroneId").AsInt32().NotNullable()
                .WithColumn("ImagePath").AsString().Nullable()

                .WithColumn("Code").AsString(50).NotNullable();

            Create.Table("BatteryLogs").InSchema("DronesServices")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("DroneId").AsInt32().NotNullable()
                .WithColumn("Timestamp").AsDateTime().NotNullable()
                .WithColumn("BatteryLevel").AsInt32().NotNullable();

            Create.Table("LoadedMedications").InSchema("DronesServices")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()  // Primary key for LoadedMedications
                .WithColumn("DroneId").AsInt32().NotNullable()
                .WithColumn("MedicationId").AsInt32().NotNullable();

            Create.ForeignKey("FK_BatteryLogs_DroneId_Drones_Id")
                .FromTable("BatteryLogs").InSchema("DronesServices").ForeignColumn("DroneId")
                .ToTable("Drones").InSchema("DronesServices").PrimaryColumn("Id");

            Create.ForeignKey("FK_LoadedMedications_DroneId_Drones_Id")
                .FromTable("LoadedMedications").InSchema("DronesServices").ForeignColumn("DroneId")
                .ToTable("Drones").InSchema("DronesServices").PrimaryColumn("Id");

            Create.ForeignKey("FK_LoadedMedications_MedicationId_Medications_Id")
                .FromTable("LoadedMedications").InSchema("DronesServices").ForeignColumn("MedicationId")
                .ToTable("Medications").InSchema("DronesServices").PrimaryColumn("Id");

            Create.ForeignKey("FK_Medications_Drones")
                .FromTable("Medications").InSchema("DronesServices").ForeignColumn("DroneId")
                .ToTable("Drones").InSchema("DronesServices").PrimaryColumn("Id");
        }

    public override void Down()
    {
        Delete.ForeignKey("FK_Medications_Drones").OnTable("Medications").InSchema("DronesServices");
        Delete.ForeignKey("FK_LoadedMedications_MedicationId_Medications_Id").OnTable("LoadedMedications")
            .InSchema("DronesServices");
        Delete.ForeignKey("FK_LoadedMedications_DroneId_Drones_Id").OnTable("LoadedMedications")
            .InSchema("DronesServices");
        Delete.ForeignKey("FK_BatteryLogs_DroneId_Drones_Id").OnTable("BatteryLogs").InSchema("DronesServices");

        Delete.Table("LoadedMedications").InSchema("DronesServices");
        Delete.Table("BatteryLogs").InSchema("DronesServices");
        Delete.Table("Medications").InSchema("DronesServices");
        Delete.Table("Drones").InSchema("DronesServices");

        Delete.Schema("DronesServices");
    }
}