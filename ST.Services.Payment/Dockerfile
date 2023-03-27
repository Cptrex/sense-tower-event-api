#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ST.Services.Payment.csproj", "ST.Services.Payment/"]
RUN dotnet restore "ST.Services.Payment/ST.Services.Payment.csproj"

WORKDIR "/src/ST.Services.Payment"
COPY . .
RUN dotnet build "ST.Services.Payment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ST.Services.Payment.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ST.Services.Payment.dll"]