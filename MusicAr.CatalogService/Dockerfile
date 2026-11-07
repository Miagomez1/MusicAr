# --- Etapa 1: Compilación ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copiar todos los proyectos y sus dependencias
COPY . .

# Restaurar dependencias
RUN dotnet restore "MusicAr.CatalogService/MusicAr.ApiCatalog.csproj"

# Compilar y publicar
RUN dotnet publish "MusicAr.CatalogService/MusicAr.ApiCatalog.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# --- Etapa 2: Ejecución ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MusicAr.ApiCatalog.dll"]