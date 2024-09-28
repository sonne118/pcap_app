#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

#WORKDIR /app
#COPY --from=publish /app/publish .
#COPY --from=build src/LoadBalancer/ssl /ssl


#ENV ASPNETCORE_Kestrel__Certificates__Default__Password=11111
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/ssl/server.pfx

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
#COPY $env:USERPROFILE/.aspnet/https/sever.pfx /https/server.pfx
#ENV ASPNETCORE_URLS="https://+;http://+"
#ENV ASPNETCORE_HTTPS_PORT=443
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password="11111"
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/server.pfx
#COPY --from=build /LoadBalancer/ssl /ssl
RUN mkdir -p /app/https
COPY LoadBalancer/ssl/server.pfx /app/https
ENTRYPOINT ["dotnet", "LoadBalancer.dll"]


##See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
#WORKDIR /app
#
##WORKDIR /app
##COPY --from=publish /app/publish .
##COPY --from=build src/LoadBalancer/ssl /ssl
## Copy the SSL certificate
##COPY your_certificate.pfx /https/your_certificate.pfx
#
#
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password=11111
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/server.pfx
#
#EXPOSE 8080
#EXPOSE 8081
#EXPOSE 5001
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["LoadBalancer.csproj", "./"]
#RUN dotnet restore "LoadBalancer.csproj"
#COPY . .
#WORKDIR "/src/."
#RUN dotnet build "LoadBalancer.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "LoadBalancer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
##COPY $env:USERPROFILE/.aspnet/https/sever.pfx /https/server.pfx
##ENV ASPNETCORE_URLS="https://+;http://+"
##ENV ASPNETCORE_HTTPS_PORT=443
##ENV ASPNETCORE_Kestrel__Certificates__Default__Password="11111"
##ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/server.pfx
#RUN mkdir -p /app/https
#COPY LoadBalancer/ssl/server.pfx /app/https
#ENTRYPOINT ["dotnet", "LoadBalancer.dll"]