namespace Wex.SharedKernel;

public readonly record struct Money(decimal Amount, string Currency)
{
    public static Money Usd(decimal amount) => new(Round(amount), "USD");

    public static decimal Round(decimal value) => decimal.Round(value, 2, MidpointRounding.AwayFromZero);

    public Money Convert(decimal rate, string currency) => new(Round(Amount * rate), currency.ToUpperInvariant());
}
