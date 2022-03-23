public class Money {
    
    public static Money ZERO = Money.Wons(0);

    private decimal amount;

    public static Money Wons(long amount) => new Money((decimal)amount);

    public static Money Wons(double amount) => new Money((decimal)amount);

    private Money(decimal amount) => this.amount = amount;

    public static Money operator +(Money a, Money b) => new Money(a.amount + b.amount);

    public static Money operator -(Money a, Money b) => new Money(a.amount - b.amount);

    public static Money operator *(Money a, double b) => new Money(a.amount * (decimal)b);

    public static bool operator <(Money a, Money b) => a.amount < b.amount;

    public static bool operator >(Money a, Money b) => a.amount > b.amount;

    public static bool operator >=(Money a, Money b) => a.amount >= b.amount;

    public static bool operator <=(Money a, Money b) => a.amount >= b.amount;

    public override bool Equals(object? obj)
    {
        if (this == obj)
        {
            return true;
        }

        if (obj is not Money)
        {
            return false;
        }

        Money? other = obj as Money;
        return object.Equals(amount, other?.amount);
    }

    public override int GetHashCode()
    {
        return amount.GetHashCode();
    }

    public override string ToString() => $"{amount:C0}";
}