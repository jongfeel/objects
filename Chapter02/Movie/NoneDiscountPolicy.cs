public class NoneDiscountPolicy : DiscountPolicy
{
    protected override Money GetDiscountAmount(Screening screening) => Money.ZERO;
}