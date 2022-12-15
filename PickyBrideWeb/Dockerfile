FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PickyBrideWeb/PickyBrideWeb.csproj", "PickyBrideWeb/"]
RUN dotnet restore "PickyBrideWeb/PickyBrideWeb.csproj"
COPY . .
WORKDIR "/src/PickyBrideWeb"
RUN dotnet build "PickyBrideWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PickyBrideWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PickyBrideWeb.dll"]
