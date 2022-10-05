FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SecretaryProblem/SecretaryProblem.csproj", "SecretaryProblem/"]
RUN dotnet restore "SecretaryProblem/SecretaryProblem.csproj"
COPY . .
WORKDIR "/src/SecretaryProblem"
RUN dotnet build "SecretaryProblem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SecretaryProblem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SecretaryProblem.dll"]
