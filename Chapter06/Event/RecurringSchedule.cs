public class RecurringSchedule
{
    private string subject;
    public DayOfWeek DayOfWeek { private set; get; }
    public DateTime From { private set; get; }
    public TimeSpan Duration { private set; get; }

    public RecurringSchedule(string subject, DayOfWeek dayOfWeek, DateTime from, TimeSpan duration)
    {
        this.subject = subject;
        this.DayOfWeek = dayOfWeek;
        this.From = from;
        this.Duration = duration;
    }
}