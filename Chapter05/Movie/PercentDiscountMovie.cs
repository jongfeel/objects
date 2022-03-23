public class PercentDiscountMovie : Movie
{
    private double percent;

    public PercentDiscountMovie(String title, TimeSpan runningTime, Money fee, double percent, params DiscountCondition[] discountConditions)
        : base(title, runningTime, fee, discountConditions) => this.percent = percent;

    protected override Money CalculateDiscountAmount() => Fee * percent;
}