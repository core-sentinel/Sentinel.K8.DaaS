#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ENV ASPNETCORE_URLS=http://+:8080
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# COPY ["src/DaaS.UI.Blazor/DaaS.UI.Blazor.csproj", "src/DaaS.UI.Blazor/"]
# RUN dotnet restore "./src/DaaS.UI.Blazor/DaaS.UI.Blazor.csproj"
COPY . .
WORKDIR "/src/src/DaaS.UI.Blazor"
RUN dotnet build "./DaaS.UI.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DaaS.UI.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaaS.UI.Blazor.dll"]