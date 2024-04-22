#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081



FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ShippingProAPICollection.RestApi/ShippingProAPICollection.RestApi.csproj", "ShippingProAPICollection.RestApi/"]

RUN dotnet nuget add source "https://www.myget.org/F/domaindrivendev/api/v3/index.json" --name "myget.org"


RUN dotnet restore "./ShippingProAPICollection.RestApi/./ShippingProAPICollection.RestApi.csproj"
COPY . .
WORKDIR "/src/ShippingProAPICollection.RestApi"
RUN dotnet build "./ShippingProAPICollection.RestApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ShippingProAPICollection.RestApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShippingProAPICollection.RestApi.dll"]