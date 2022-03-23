using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest;

[TestClass]
public class MovieTest
{
    [TestMethod]
    public void ReserveTest()
    {
        // Arrange & Action
        Customer jongfeel = new Customer("jongfeel", "1");
        PeriodCondition periodCondition = new PeriodCondition(
            DayOfWeek.Wednesday,
            new DateTime(2022, 3, 23, 12, 0, 0),
            new DateTime(2022, 3, 23, 22, 0, 0)
        );

        SequenceCondition sequenceCondition = new SequenceCondition(1);

        Movie theBatman = new Movie("The Batman", TimeSpan.FromMinutes(176), Money.Wons(14000),
            new List<PeriodCondition>() { periodCondition },
            new List<SequenceCondition>() { sequenceCondition }
        );
        theBatman.DiscountAmount = Money.Wons(2000);
        theBatman.DiscountPercent = 0.1;
        
        Reservation reservation = new Screening() { Movie = theBatman }.Reserve(jongfeel, 1);

        // Assert
        Assert.AreEqual(reservation.AudienceCount, 1);
        Assert.AreEqual(reservation.Customer, jongfeel);
    }
}