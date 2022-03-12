using System;

public class DiscountCondition
{
    public DiscountConditionType Type { set; get; }
    public int Sequence { set; get; }
    public DayOfWeek DayOfWeek { set; get; }
    public DateTime StartTime { set; get; }
    public DateTime EndTime { set; get; }

    public bool IsDiscountable(DayOfWeek dayOfWeek, DateTime time) => Type == DiscountConditionType.PERIOD ? DayOfWeek == dayOfWeek && StartTime < time && EndTime > time : false;
    public bool IsDiscountable(int sequence) => Type == DiscountConditionType.SEQUENCE ? Sequence == sequence : false;
}