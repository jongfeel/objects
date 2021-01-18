using System;

public class PeriodCondition : DiscountCondition
{
    private DayOfWeek dayOfWeek;
    private TimeSpan startTime;
    private TimeSpan endTime;

    public PeriodCondition(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        this.dayOfWeek = dayOfWeek;
        this.startTime = startTime;
        this.endTime = endTime;
    }

    public bool IsSatisfiedBy(Screening screening) => screening.StartTime.DayOfWeek == dayOfWeek &&
            startTime <= screening.StartTime.TimeOfDay &&
            endTime >= screening.StartTime.TimeOfDay;
}