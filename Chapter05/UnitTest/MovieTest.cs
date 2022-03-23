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

        Movie theBatman = new AmountDiscountMovie("The Batman", TimeSpan.FromMinutes(176), Money.Wons(14000), Money.Wons(2000),
            new DiscountCondition[] { periodCondition, sequenceCondition });
        
        Reservation reservation = new Screening() { Movie = theBatman }.Reserve(jongfeel, 1);

        // Assert
        Assert.AreEqual(reservation.AudienceCount, 1);
        Assert.AreEqual(reservation.Customer, jongfeel);
    }
}