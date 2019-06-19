FROM endeveit/docker-jq AS config
WORKDIR /src
COPY . .
WORKDIR /src/tests/Restaurant.Business.Tests

RUN jq '.ConnectionStrings.DefaultConnection = "Server=relationaldb;Port=5432;Database=restaurant-relational;User Id=postgres;Password=postgres;"' appsettings.json > tmp.$$.json && mv tmp.$$.json appsettings.json
RUN jq '.EventStore.ConnectionString = "Server=eventstore;Port=5432;Database=restaurant-event-store;User Id=postgres;Password=postgres;"' appsettings.json > tmp.$$.json && mv tmp.$$.json appsettings.json
RUN cat appsettings.json

FROM microsoft/dotnet:2.1-sdk AS build

WORKDIR /src

COPY --from=config /src .

RUN ls

WORKDIR /src/tests/Restaurant.Business.Tests

RUN ls

RUN chmod +x run-tests-with-coverage-report.sh
CMD bash run-tests-with-coverage-report.sh \
    && cp coverage.* /coverage