public class PercentDiscountPolicy : DiscountPolicy
{
    private double percent;

    public PercentDiscountPolicy(double percent, params DiscountCondition[] conditions) : base(conditions) => this.percent = percent;

    protected override Money GetDiscountAmount(Screening screening) => screening.MovieFee * percent;
}