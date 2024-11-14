FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RouteWise.Bot/RouteWise.Bot.csproj", "RouteWise.Bot/"]
COPY ["RouteWise.Data/RouteWise.Data.csproj", "RouteWise.Data/"]
COPY ["RouteWise.Domain/RouteWise.Domain.csproj", "RouteWise.Domain/"]
COPY ["RouteWise.Service/RouteWise.Service.csproj", "RouteWise.Service/"]
RUN dotnet restore "RouteWise.Bot/RouteWise.Bot.csproj"
COPY . .
WORKDIR "/src/RouteWise.Bot"
RUN dotnet build "RouteWise.Bot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RouteWise.Bot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RouteWise.Bot.dll"]
