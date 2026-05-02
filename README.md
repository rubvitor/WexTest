# WEX Purchase Currency Platform

Production-style .NET 10 solution for the WEX product brief.

## Architecture
- **Purchase.Api**: stores purchase transactions through EF Core using an in-memory SQL-style database model, publishes domain events, and exposes purchase endpoints.
- **ExchangeRate.Api**: converts a stored purchase using Treasury Reporting Rates of Exchange rules.
- **SharedKernel**: domain primitives, money/date value objects, event abstractions.
- **Tests**: xUnit + FluentAssertions covering domain rules, exchange-rate rules, and EF Core repository behavior.

No external database or server is required. The purchase microservice uses EF Core InMemory with explicit DDD persistence entities, table configuration, and LINQ-to-Entities queries. Seeded exchange-rate JSON remains only as an offline Treasury API fallback.

# VERY IMPORTANT
The Project could be done using more than one layer. Could be for example 2 folders, one for the service Purchase with .csprojs (layers) Application, Domain, Infrastructure separated. The same with the Project ExachangeRate. The Unit Test folder I decided that it belongs to the whole microservices instead of creating one for each Project. It can help with I have integrations between the projects too.

## Run
```bash
dotnet restore
dotnet test
dotnet run --project src/Purchase.Api
dotnet run --project src/ExchangeRate.Api
```

Purchase API: `https://localhost:7001/swagger`  
Exchange API: `https://localhost:7002/swagger`

## Main flows
1. `POST /api/purchases` stores a USD transaction.
2. `GET /api/purchases/{id}` retrieves it from the EF Core in-memory database.
3. `GET /api/conversions/{purchaseId}?country=Brazil` returns converted values using the exchange rate active on or before the purchase date and not older than six months.

## Precision
All money calculations use `decimal`. Input USD amounts and converted amounts are rounded with `MidpointRounding.AwayFromZero`.





# WEX Purchase Currency UI

Angular 20 standalone app. It contains two screens:

1. **Create Purchase**: description, transaction date and USD amount.
2. **Convert Purchase**: purchase id and target country, showing exchange rate and converted amount.

## Run
```bash
npm install
npm start
```
The API URLs are configured in `src/environments/environment.ts`.
