public abstract class DefaultDiscountPolicy : DiscountPolicy
{
    private DiscountCondition[] conditions;

    public DefaultDiscountPolicy(params DiscountCondition[] conditions) {
        this.conditions = conditions;
    }

    public override Money CalculateDiscountAmount(Screening screening)
    {
        foreach (var each in conditions)
        {
            if (each.IsSatisfiedBy(screening))
            {
                return getDiscountAmount(screening);
            }
        }

        return Money.ZERO;
    }

    abstract protected Money getDiscountAmount(Screening screening);
}