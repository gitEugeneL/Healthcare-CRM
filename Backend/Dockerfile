FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Directory.Build.props", "."]

COPY ["Core/Domain/Domain.csproj", "Core/Domain/"]
COPY ["Core/Application/Application.csproj", "Core/Application/"]
COPY ["Infrastructure/Persistence/Persistence.csproj", "Infrastructure/Persistence/"]
COPY ["Infrastructure/Security/Security.csproj", "Infrastructure/Security/"]
COPY ["Infrastructure/Cache/Cache.csproj", "Infrastructure/Cache/"]
COPY ["Infrastructure/Mail/Mail.csproj", "Infrastructure/Mail/"]
COPY ["Presentation/Api/Api.csproj", "Presentation/Api/"]

RUN dotnet restore "Presentation/Api/Api.csproj"

COPY . .

WORKDIR "Presentation/Api"
RUN dotnet build -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]