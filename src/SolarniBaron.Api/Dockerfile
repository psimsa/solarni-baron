#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/SolarniBaron.Api/SolarniBaron.Api.csproj", "src/SolarniBaron.Api/"]
COPY ["src/SolarniBaron.Domain/SolarniBaron.Domain.csproj", "src/SolarniBaron.Domain/"]
RUN dotnet restore "src/SolarniBaron.Api/SolarniBaron.Api.csproj"
COPY . .
WORKDIR "/src/src/SolarniBaron.Api"
RUN dotnet build "SolarniBaron.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SolarniBaron.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SolarniBaron.Api.dll"]