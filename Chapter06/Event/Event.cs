public class Event
{
    private string subject;
    private DateTime from;
    private TimeSpan duration;

    public Event(string subject, DateTime from, TimeSpan duration)
    {
        this.subject = subject;
        this.from = from;
        this.duration = duration;
    }

    public bool IsSatisfied(RecurringSchedule schedule)
     => from.DayOfWeek != schedule.DayOfWeek ||
        from.Hour != schedule.From.Hour || from.Minute != schedule.From.Minute ||
        duration.TotalMinutes != schedule.Duration.TotalMinutes
        ? false : true;

    public void Reschedule(RecurringSchedule schedule)
    {
        from = from.AddDays(DaysDistance(schedule));
        duration = schedule.Duration;
    }

    private int DaysDistance(RecurringSchedule schedule) => schedule.DayOfWeek - from.DayOfWeek;
}