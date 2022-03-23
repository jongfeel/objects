public class NoneDiscountMovie : Movie
{
    public NoneDiscountMovie(string title, TimeSpan runningTime, Money fee) : base(title, runningTime, fee) { }

    protected override Money CalculateDiscountAmount() => Money.ZERO;
}