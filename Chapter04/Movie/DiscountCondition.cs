using System;

public class DiscountCondition
{
    public DiscountConditionType Type { set; get; }
    public int Sequence { set; get; }
    public DayOfWeek DayOfWeek { set; get; }
    public TimeSpan StartTime { set; get; }
    public TimeSpan EndTime { set; get; }
}