FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["api.Presentation/api.Presentation.csproj", "api.Presentation/"]
COPY ["api.Application/api.Application.csproj", "api.Application/"]
COPY ["api.Common/api.Common.csproj", "api.Common/"]
COPY ["api.Core/api.Core.csproj", "api.Core/"]
COPY ["api.Infrastructure/api.Infrastructure.csproj", "api.Infrastructure/"]
RUN dotnet restore "./api.Presentation/api.Presentation.csproj"
COPY . .
WORKDIR "/src/api.Presentation"
RUN dotnet build "./api.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./api.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.Presentation.dll"]