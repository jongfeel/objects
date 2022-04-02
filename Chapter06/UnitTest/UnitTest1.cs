using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void Event()
    {
        Event meeting = new Event("회의", new DateTime(2022, 4, 2, 10, 30, 0), TimeSpan.FromMinutes(30));
    }

    [TestMethod]
    public void RecurringSchedule()
    {
        RecurringSchedule schedule = new RecurringSchedule("회의", DayOfWeek.Saturday, DateTime.Today.AddHours(10).AddMinutes(30), TimeSpan.FromMinutes(30));

        Assert.AreEqual(schedule.DayOfWeek, DayOfWeek.Saturday);
        Assert.AreEqual(schedule.From.Hour, 10);
        Assert.AreEqual(schedule.From.Minute, 30);
        Assert.AreEqual(schedule.Duration, TimeSpan.FromMinutes(30));
    }

    [TestMethod]
    public void Event_Match_RecurringSchedule()
    {
        RecurringSchedule schedule = new RecurringSchedule("회의", DayOfWeek.Saturday, DateTime.Today.AddHours(10).AddMinutes(30), TimeSpan.FromMinutes(30));
        Event meeting = new Event("회의", new DateTime(2022, 4, 2, 10, 30, 0), TimeSpan.FromMinutes(30));

        Assert.IsTrue(meeting.IsSatisfied(schedule));
    }

    [TestMethod]
    public void EventReschedule_Match_RecurringSchedule()
    {
        RecurringSchedule schedule = new RecurringSchedule("회의", DayOfWeek.Saturday, DateTime.Today.AddHours(10).AddMinutes(30), TimeSpan.FromMinutes(30));
        Event meeting = new Event("회의", new DateTime(2022, 4, 3, 10, 30, 0), TimeSpan.FromMinutes(30));

        Assert.IsFalse(meeting.IsSatisfied(schedule));

        meeting.Reschedule(schedule);

        Assert.IsTrue(meeting.IsSatisfied(schedule));
    }
}