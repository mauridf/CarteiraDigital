# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CarteiraDigital.Api/CarteiraDigital.Api.csproj", "CarteiraDigital.Api/"]
COPY ["CarteiraDigital.Application/CarteiraDigital.Application.csproj", "CarteiraDigital.Application/"]
COPY ["CarteiraDigital.Domain/CarteiraDigital.Domain.csproj", "CarteiraDigital.Domain/"]
COPY ["CarteiraDigital.Infrastructure/CarteiraDigital.Infrastructure.csproj", "CarteiraDigital.Infrastructure/"]
RUN dotnet restore "CarteiraDigital.Api/CarteiraDigital.Api.csproj"
COPY . .
WORKDIR "/src/CarteiraDigital.Api"
RUN dotnet build "CarteiraDigital.Api.csproj" -c Release -o /app/build

# Estágio de publicação
FROM build AS publish
RUN dotnet publish "CarteiraDigital.Api.csproj" -c Release -o /app/publish

# Estágio final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarteiraDigital.Api.dll"]