public class PeriodCondition : DiscountCondition
{
    public DayOfWeek DayOfWeek { private set; get; }
    public DateTime StartTime { private set; get; }
    public DateTime EndTime { private set; get; }

    public PeriodCondition(DayOfWeek dayOfWeek, DateTime startTime, DateTime endTime)
    {
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
    }

    public bool IsSatisfiedBy(Screening screening) =>
        DayOfWeek == screening.WhenScreened.DayOfWeek &&
        StartTime < screening.WhenScreened &&
        EndTime > screening.WhenScreened;
}