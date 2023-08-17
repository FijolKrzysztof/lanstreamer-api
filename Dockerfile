FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["lanstreamer-api.csproj", "./"]
RUN dotnet restore "lanstreamer-api.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "lanstreamer-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lanstreamer-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY ["ssl/certificate.pfx", "certificate.pfx"]
COPY ["appsettings.json", "appsettings.json"]

ENTRYPOINT ["dotnet", "lanstreamer-api.dll"]
