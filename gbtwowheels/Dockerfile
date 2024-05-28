#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["gbtwowheels/gbtwowheels.csproj", "gbtwowheels/"]
COPY ["https/certificate.pfx", "https/"]
COPY ["https/mycert.pfx", "https/"]
RUN dotnet restore "gbtwowheels/gbtwowheels.csproj"
COPY . .
WORKDIR "/src/gbtwowheels"
RUN dotnet build "gbtwowheels.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gbtwowheels.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gbtwowheels.dll"]
