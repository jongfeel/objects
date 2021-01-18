using System.Collections.Generic;
using System.Linq;

public abstract class DiscountPolicy
{
    private List<DiscountCondition> conditions = new List<DiscountCondition>();

    public DiscountPolicy(params DiscountCondition[] conditions) => this.conditions.AddRange(conditions);

    public virtual Money CalculateDiscountAmount(Screening screening) =>
        conditions.Count(condition => condition.IsSatisfiedBy(screening)) > 0 ?
        GetDiscountAmount(screening) : Money.ZERO;

    abstract protected Money GetDiscountAmount(Screening screening);
}