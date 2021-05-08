public interface DiscountCondition
{
    bool IsSatisfiedBy(Screening screening);
}