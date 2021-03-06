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

    // public boolean equals(Object object) {
    //     if (this == object) {
    //         return true;
    //     }

    //     if (!(object instanceof Money)) {
    //         return false;
    //     }

    //     Money other = (Money)object;
    //     return Objects.equals(amount.doubleValue(), other.amount.doubleValue());
    // }

    // public int hashCode() {
    //     return Objects.hashCode(amount);
    // }

    public override string ToString() => $"{amount}원";
}