FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PrincessHttpClient/PrincessHttpClient.csproj", "PrincessHttpClient/"]
RUN dotnet restore "PrincessHttpClient/PrincessHttpClient.csproj"
COPY . .
WORKDIR "/src/PrincessHttpClient"
RUN dotnet build "PrincessHttpClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PrincessHttpClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrincessHttpClient.dll"]
