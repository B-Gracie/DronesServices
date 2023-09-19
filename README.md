

# Drone Service
[Bukola Ayangunna, gracieayan@gmail.com]

## Table of Contents
- [Introduction]
- [Task Description]
- [Requirements]
  - [Functional Requirements]
  - [Non-functional Requirements]
- [Getting Started]
  - [Prerequisites]
  - [Installation]
  - [Running the Application]
- [API Endpoints]
- [Testing]
- [Technologies Used]
- [Contributing]


## Introduction
Welcome to the Drone Service project! This service provides a REST API for managing a fleet of drones that can be used for various transportation tasks, such as delivering medications to remote locations.

## Task Description
The primary goal of this project is to develop a service that allows clients to communicate with drones. Specifically, it should support the following functionalities:
- Registering a drone.
- Loading a drone with medication items.
- Checking loaded medication items for a given drone.
- Checking available drones for loading.
- Checking drone battery level for a given drone.

## Requirements

### Functional Requirements
1. **Load Limit Control**: Prevent the drone from being loaded with more weight than it can carry.
2. **Battery Level Check**: Prevent the drone from being in the LOADING state if the battery level is below 25%.
3. **Battery Level Logging**: Introduce a periodic task to check drones' battery levels and create a history/audit event log for this.

### Non-functional Requirements
- **Data Format**: Input/output data must be in JSON format.
- **Build and Run**: The project must be buildable and runnable.
- **Database**: Use a local database (e.g., in-memory or via container) for data storage.
- **Data Preloading**: Preload any required data, such as reference tables and dummy data, into the database.
- **Unit Tests**: Implement unit tests to ensure the correctness of the application.
- **Framework**: Use a modern and popular framework for development.

## Getting Started

### Prerequisites
- Before you begin, make sure you have the following prerequisites installed on your system:

.NET SDK - Make sure you have .NET SDK installed. You can verify this by running dotnet --version in your terminal or command prompt.
Docker - Install Docker to run the PostgreSQL database container.

### Installation
-Restore Dependencies: In the project directory, run the following command to restore the project's dependencies:
-Ensure that your application's configuration (usually found in appsettings.json or a similar file) is correctly set up to connect to your PostgreSQL database. 
the connection string is properly configured in the project. [See Drones.API project for reference].

### Running the Application
[Provide instructions for running the application locally]

## API Endpoints

###Drone Endpoints

POST /api/drones/create: Register a new drone.
Example: POST https://localhost:7063/api/drones/create

GET /api/drones/getall: Retrieve a list of all drones.
Example: GET https://localhost:7063/api/drones/getall

P.S. Drone IDs are from 
 101 - 105.

GET /api/drones/{droneId}: Retrieve details of a specific drone by its ID.
Example: GET https://localhost:7063/api/drones/101

POST /api/drones/{droneId}/updateState: Update the state of a specific drone.
Example: POST https://localhost:7063/api/drones/101/updateState
Request Body: Specify the new state (e.g., "2 for LOADING") in the request body.
               
    IDLE = 1,
    LOADING = 2,
    LOADED = 3,
    DELIVERING = 4,
    DELIVERED = 5,
    RETURNING = 6


PUT /api/drones/{droneId}/update-battery: Update the battery capacity of a specific drone.
Example: PUT https://localhost:7063/api/drones/101/update-battery
Request Body: Specify the new battery capacity as an integer in the request body.

GET /api/drones/{droneId}/isAvailableForLoading: Check if a specific drone is available for loading medications based on its weight limit and battery level.
Example: GET https://localhost:7063/api/drones/101/isAvailableForLoading?medicationWeight=10.0
Query Parameter: medicationWeight (double) - The weight of the medication to be loaded onto the drone


### Medication Endpoints
GET /api/medications/getloaded-medication: Retrieve a list of medications that are currently loaded onto drones.
Example: GET https://localhost:7063/api/medications/getloaded-medication

GET /api/medications/{medicationId}: Retrieve details of a specific medication by its ID.
Example: GET https://localhost:7063/api/medications/101

P.S. Medication IDs rnages from 
1 - 4.

POST /api/medications/{droneId}/medications: Create a new medication and load it onto a specific drone.
Example: POST https://localhost:7063/api/medications/101/medications
Request Body: Include the details of the medication to be created and loaded.


### Battery Log Endpoints
POST /api/batterylog/log: Log the battery level for a specific drone.
Example: POST https://localhost:7063/api/batterylog/log
Request Body: Include the DroneId and BatteryLevel in the request body.
Returns a success message if the battery level is logged successfully.

GET /api/batterylog/{droneId}/logs: Retrieve battery level logs for a specific drone.
Example: GET https://localhost:7063/api/batterylog/101/logs?count=10
Parameters:
droneId: The ID of the drone for which you want to retrieve battery logs.
count: The number of battery logs to retrieve (optional).
Returns a list of battery logs for the specified drone.

P.S. The database has been populated with neccessary data.


### Testing

To run the unit tests for this project, follow these steps:

#### 1. Test Setup Instructions
- **Prerequisites**: Ensure that you have .NET Core SDK installed on your machine.

- **Installing Testing Libraries**: Make sure you have the necessary NuGet packages installed for testing, including:
   - `xunit` and `xunit.runner.visualstudio` for the xUnit testing framework.
   - 'Moq' to mock dependencies

- **Installing TestContainers (if applicable)**: to manage Docker containers for testing purposes (e.g., PostgreSQL for database tests).
you can install it using the following NuGet command:
   
   ```bash
   dotnet add package TestContainers
   ```

#### 2. Command to Run Tests
To run the unit tests, follow these steps:

- Open a terminal or command prompt.

- Navigate to the root directory of the test project.

- Use the following command to run the tests:

   ```bash
   dotnet test
   ```

   This command will discover and execute the unit tests in the project.


## Technologies Used
- Docker: Used for containerization and managing dependencies.

- C#: The primary programming language for developing the application.

- ASP.NET Core: Used for building the RESTful API.

- PostgreSQL: The chosen database management system.

- xUnit: A testing framework used for unit testing.

- FluentMigrator: Used for database schema migrations.

- TestContainers: for running containerized services in the integration tests

- Postman and Swagger: tool for API testing.

## Contributing
Contributions are welcome! Please feel free.

##
To clone this repository to your local machine, open your terminal or command prompt and run the following command:

```bash
git clone https://gitlab.com/musala_soft/DEV_DRONES-642f9847-d01d-4e09-8817-383ee60d67b2.git


##Project Developed By:

Bukola Ayangunna
gracieayan@gmail
=======
# DronesServices
>>>>>>> 679720ee66b8fbdef968404022b84699ebbcda92
