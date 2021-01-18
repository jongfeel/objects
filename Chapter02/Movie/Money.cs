public class Money {
    
    public static Money ZERO = Money.Wons(0);

    private decimal amount;

    public static Money Wons(long amount) => new Money((decimal)amount);

    public static Money Wons(double amount) => new Money((decimal)amount);

    Money(decimal amount) => this.amount = amount;

    public Money Plus(Money amount) => new Money(this.amount + amount.amount);

    public Money Minus(Money amount) => new Money(this.amount - amount.amount);

    public Money Times(double percent) => new Money(this.amount * (decimal)percent);

    public bool IsLessThan(Money other) => amount < other.amount;

    public bool IsGreaterThanOrEqual(Money other) => amount >= other.amount;

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

    // public String toString() {
    //     return amount.toString() + "원";
    // }
}