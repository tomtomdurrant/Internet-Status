FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Internet-Status.csproj", "./"]
RUN dotnet restore "./Internet-Status.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Internet-Status.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Internet-Status.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Internet-Status.dll"]