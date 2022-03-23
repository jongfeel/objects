public class SequenceCondition : DiscountCondition
{
    public int Sequence { private set; get; }

    public SequenceCondition(int sequence) => Sequence = sequence;

    public bool IsSatisfiedBy(Screening screening) => Sequence == screening.Sequence;
}