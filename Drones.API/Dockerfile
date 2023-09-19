FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Drones.API/Drones.API.csproj", "Drones.API/"]
RUN dotnet restore "Drones.API/Drones.API.csproj"
COPY . .
WORKDIR "/src/Drones.API"
RUN dotnet build "Drones.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Drones.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Drones.API.dll"]
