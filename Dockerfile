FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base

ARG VAULT_TOKEN
ENV HTDC_VAULT_TOKEN=$VAULT_TOKEN

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2.204 AS build
WORKDIR /src
COPY *.sln ./
COPY . .
RUN dotnet restore
WORKDIR /src/Api
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app

COPY --from=publish /app .
COPY ./Api/HappyTravel.Edo.Api.xml .
HEALTHCHECK --interval=6s --timeout=10s --retries=3 CMD curl -sS 127.0.0.1/health || exit 1

ENTRYPOINT ["dotnet", "HappyTravel.Edo.Api.dll"]