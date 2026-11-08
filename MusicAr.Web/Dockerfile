FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "MusicAr.Web/MusicAr.Web.csproj"

RUN dotnet publish "MusicAr.Web/MusicAr.Web.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MusicAr.Web.dll"]