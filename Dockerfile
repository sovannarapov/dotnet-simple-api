# Base image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

WORKDIR /app

EXPOSE 80
EXPOSE 443

COPY *.csproj ./

RUN dotnet restore

COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "FormulaOneK8S.dll"]
