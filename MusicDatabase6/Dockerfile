FROM mcr.microsoft.com/dotnet/aspnet:8.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-bullseye AS build
WORKDIR /src
COPY ["MusicDatabase6.csproj", "./"]
RUN dotnet restore "./MusicDatabase6.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet publish "MusicDatabase6.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MusicDatabase6.dll"]
