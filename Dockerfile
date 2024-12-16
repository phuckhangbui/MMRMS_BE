#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./API/API.csproj", "API/"]
COPY ["./BusinessObject/BusinessObject.csproj", "BusinessObject/"]
COPY ["./DAO/DAO.csproj", "DAO/"]
COPY ["./DTOs/DTOs.csproj", "DTOs/"]
COPY ["./Common/Common.csproj", "Common/"]
COPY ["./Repository/Repository.csproj", "Repository/"]
COPY ["./Service/Service.csproj", "Service/"]
RUN dotnet restore "./API/API.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "./API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER root
RUN chown app:app /app/systemsetting.json && chmod u+w /app/systemsetting.json

USER app

ENTRYPOINT ["dotnet", "API.dll"]
