#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated7.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/runtime:8.0 as runtime6.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Copy .NET Core 6.0 runtime from the 6.0 image
COPY --from=runtime6.0 /usr/share/dotnet/host /usr/share/dotnet/host
COPY --from=runtime6.0 /usr/share/dotnet/shared /usr/share/dotnet/shared
WORKDIR /src
COPY ["src/SolarniBaron.Func/SolarniBaron.Func.csproj", "src/SolarniBaron.Func/"]
COPY ["src/SolarniBaron.Caching/SolarniBaron.Caching.csproj", "src/SolarniBaron.Caching/"]
COPY ["src/SolarniBaron.Domain/SolarniBaron.Domain.csproj", "src/SolarniBaron.Domain/"]
COPY ["src/SolarniBaron.Persistence/SolarniBaron.Persistence.csproj", "src/SolarniBaron.Persistence/"]
RUN dotnet restore "src/SolarniBaron.Func/SolarniBaron.Func.csproj"
COPY . .
WORKDIR "/src/src/SolarniBaron.Func"
RUN dotnet build "SolarniBaron.Func.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SolarniBaron.Func.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true