# --- Etapa 1: Compilación ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copiar todos los proyectos
COPY . .

# Restaurar dependencias
RUN dotnet restore "MusicAr.ApiGateway/MusicAr.ApiGateway.csproj"

# Compilar y publicar
RUN dotnet publish "MusicAr.ApiGateway/MusicAr.ApiGateway.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# --- Etapa 2: Ejecución ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MusicAr.ApiGateway.dll"]