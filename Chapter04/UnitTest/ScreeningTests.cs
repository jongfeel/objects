using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest;

[TestClass]
public class ScreeningTests
{
    [TestMethod]
    public void CalculateFee()
    {
        // Arrange
        // Arrange
        DiscountCondition dc = new DiscountCondition()
        {
            Type = DiscountConditionType.SEQUENCE,
            Sequence = 5
        };
        Movie movie = new Movie("THE BATMAN", TimeSpan.FromMinutes(300), Money.Wons(14000), Money.Wons(2000), dc);
        Screening screening = new Screening()
        {
            Movie = movie,
            Sequence = 5,
            WhenScreened = new DateTime(2022, 3, 12, 18, 00, 00)
        };

        // Action and Assert
        Assert.AreEqual(screening.CalculateFee(1), Money.Wons(12000));
        Assert.AreEqual(screening.CalculateFee(2), Money.Wons(24000));
    }
}