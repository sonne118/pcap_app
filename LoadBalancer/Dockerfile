FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

#EXPOSE 8080
#EXPOSE 8081
EXPOSE 5001
EXPOSE 5000
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LoadBalancer/LoadBalancer.csproj", "LoadBalancer/"]
RUN dotnet restore "./LoadBalancer/LoadBalancer.csproj"
COPY . .
WORKDIR "/src/LoadBalancer"
RUN dotnet build "./LoadBalancer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LoadBalancer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/https
COPY LoadBalancer/ssl/server.pfx /app/https
ENTRYPOINT ["dotnet", "LoadBalancer.dll"]

