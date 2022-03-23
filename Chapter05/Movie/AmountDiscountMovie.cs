public class AmountDiscountMovie : Movie
{
    private Money discountAmount;

    public AmountDiscountMovie(String title, TimeSpan runningTime, Money fee, Money discountAmount, params DiscountCondition[] discountConditions)
        : base(title, runningTime, fee, discountConditions) => this.discountAmount = discountAmount;

    protected override Money CalculateDiscountAmount() => discountAmount;
}