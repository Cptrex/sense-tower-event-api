FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./SenseTowerEventAPI/SenseTowerEventAPI.csproj", "SenseTowerEventAPI/"]

RUN dotnet restore "SenseTowerEventAPI/SenseTowerEventAPI.csproj"
COPY . .
WORKDIR "/src/SenseTowerEventAPI"
RUN dotnet build "SenseTowerEventAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SenseTowerEventAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "SenseTowerEventAPI.dll"]