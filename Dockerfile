FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./ST.Events.API/ST.Events.API.csproj", "ST.Events.API/"]

RUN dotnet restore "ST.Events.API/ST.Events.API.csproj"
COPY . .
WORKDIR "/src/ST.Events.API"
RUN dotnet build "ST.Events.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ST.Events.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ST.Events.API.dll"]