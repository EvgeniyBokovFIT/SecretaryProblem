FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HostedServiceAndDI/HostedServiceAndDI.csproj", "HostedServiceAndDI/"]
RUN dotnet restore "HostedServiceAndDI/HostedServiceAndDI.csproj"
COPY . .
WORKDIR "/src/HostedServiceAndDI"
RUN dotnet build "HostedServiceAndDI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HostedServiceAndDI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HostedServiceAndDI.dll"]
