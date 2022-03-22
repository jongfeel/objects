using System;

public class DiscountCondition
{
    public DiscountConditionType Type { set; get; }
    public int Sequence { set; get; }
    public DayOfWeek DayOfWeek { set; get; }
    public DateTime StartTime { set; get; }
    public DateTime EndTime { set; get; }

    public bool IsSatisfiedBy(Screening screening) => Type == DiscountConditionType.PERIOD ? IsSatisfiedByPeriod(screening) : IsSatisfiedBySequence(screening);

    private bool IsSatisfiedByPeriod(Screening screening)
        => DayOfWeek == screening.WhenScreened.DayOfWeek &&
        StartTime < screening.WhenScreened &&
        EndTime > screening.WhenScreened;
    
    private bool IsSatisfiedBySequence(Screening screening) => Sequence == screening.Sequence;
}