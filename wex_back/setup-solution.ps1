dotnet new sln -n Wex.PurchaseCurrency --force
dotnet sln add src/SharedKernel/SharedKernel.csproj
dotnet sln add src/Purchase.Api/Purchase.Api.csproj
dotnet sln add src/ExchangeRate.Api/ExchangeRate.Api.csproj
dotnet sln add tests/Wex.Tests/Wex.Tests.csproj
