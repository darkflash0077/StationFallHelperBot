#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["StationFallHelperBot/StationFallHelperBot.csproj", "StationFallHelperBot/"]
RUN dotnet restore "StationFallHelperBot/StationFallHelperBot.csproj"
COPY . .
WORKDIR "/src/StationFallHelperBot"
RUN dotnet build "StationFallHelperBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StationFallHelperBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StationFallHelperBot.dll"]