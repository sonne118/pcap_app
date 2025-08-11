FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

EXPOSE 5010-5014

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["srv_pub/srv_pub.csproj", "srv_pub/"]
COPY ["kafka/kafka.csproj", "kafka/"]
COPY ["outbox/outbox.csproj", "outbox/"]
COPY srv_pub/ srv_pub/
COPY kafka/ kafka/
COPY outbox/ outbox/
RUN dotnet restore "srv_pub/srv_pub.csproj"
RUN dotnet build "srv_pub/srv_pub.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "srv_pub/srv_pub.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "srv_pub.dll"]



