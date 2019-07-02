FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/server/Restaurant.Api/Restaurant.Api.csproj src/server/Restaurant.Api/
COPY src/server/Restaurant.Business/Restaurant.Business.csproj src/server/Restaurant.Business/
COPY src/server/Restaurant.Core/Restaurant.Core.csproj src/server/Restaurant.Core/
COPY src/server/Restaurant.Domain/Restaurant.Domain.csproj src/server/Restaurant.Domain/
COPY src/server/Restaurant.Persistence/Restaurant.Persistence.csproj src/server/Restaurant.Persistence/
RUN dotnet restore src/server/Restaurant.Api/Restaurant.Api.csproj
COPY . .
WORKDIR /src/src/server/Restaurant.Api
RUN dotnet build Restaurant.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Restaurant.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Restaurant.Api.dll"]
