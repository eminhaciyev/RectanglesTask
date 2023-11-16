# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./RectanglesTask/RectanglesTask.csproj" --disable-parallel
RUN dotnet publish "./RectanglesTask/RectanglesTask.csproj" -c release -o /app --no-restore

# Server Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "RectanglesTask.dll"]