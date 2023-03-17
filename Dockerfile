#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# escape=`

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-comm
WORKDIR /src
COPY ["./SenseTowerEventAPI/SenseTowerEventAPI.csproj", "SenseTowerEventAPI/"]
RUN dotnet restore "SenseTowerEventAPI/SenseTowerEventAPI.csproj"
COPY . .
WORKDIR "/src/SenseTowerEventAPI"
RUN dotnet build "SenseTowerEventAPI.csproj" -c Release -o /app/build
FROM build-comm AS publish-comm
RUN dotnet publish "SenseTowerEventAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-main
WORKDIR /src
COPY ["./SenseTowerEventAPI/SenseTowerEventAPI.csproj", "SenseTowerEventAPI/"]
RUN dotnet restore "SenseTowerEventAPI/SenseTowerEventAPI.csproj"
COPY . .
WORKDIR "/src/SenseTowerEventAPI"
RUN dotnet build "SenseTowerEventAPI.csproj" -c Release -o /app/build
FROM build-main AS publish-main
RUN dotnet publish "SenseTowerEventAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish-main /app/publish .
COPY --from=publish-comm /app/publish .

ENTRYPOINT ["dotnet", "SenseTowerEventAPI.dll"]