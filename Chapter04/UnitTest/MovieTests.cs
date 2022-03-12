using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest;

[TestClass]
public class MovieTests
{
    [TestMethod]
    public void CalculateAmountDiscountedFee()
    {
        // Arrange
        Movie movie = new Movie("THE BATMAN", TimeSpan.FromMinutes(300), Money.Wons(14000), Money.Wons(2000), new DiscountCondition());

        // Action and Assert
        Assert.AreEqual(movie.CalculateAmountDiscountedFee, Money.Wons(12000));
    }

    [TestMethod]
    public void CalculatePercentDiscountedFee()
    {
        // Arrange
        Movie movie = new Movie("THE BATMAN", TimeSpan.FromMinutes(300), Money.Wons(14000), 0.1, new DiscountCondition());

        // Action and Assert
        Assert.AreEqual(movie.CalculatePercentDiscountedFee, Money.Wons(12600));
    }

    [TestMethod]
    public void CalculateNoneDiscountedFee()
    {
        // Arrange
        Movie movie = new Movie("THE BATMAN", TimeSpan.FromMinutes(300), Money.Wons(14000));

        // Action and Assert
        Assert.AreEqual(movie.CalculateNoneDiscountedFee, Money.Wons(14000));
    }

    [TestMethod]
    public void IsDiscountable_HasDiscountCondition()
    {
        // Arrange
        DiscountCondition dc = new DiscountCondition()
        {
            Type = DiscountConditionType.PERIOD,
            DayOfWeek = DayOfWeek.Saturday,
            StartTime = new DateTime(2022, 3, 12, 12, 00, 00),
            EndTime = new DateTime(2022, 3, 12, 22, 00, 00)
        };
        Movie movie = new Movie("THE BATMAN", TimeSpan.FromMinutes(300), Money.Wons(14000), 0.1, dc);

        // Action and Assert
        Assert.IsTrue(movie.IsDiscountable(new DateTime(2022, 3, 12, 13, 00, 00), 2));
    }

    [TestMethod]
    public void IsDiscountable_NoDiscountCondition()
    {
        // Arrange
        DiscountCondition dc = new DiscountCondition()
        {
            Type = DiscountConditionType.SEQUENCE,
            Sequence = 2
        };
        Movie movie = new Movie("THE BATMAN", TimeSpan.FromMinutes(300), Money.Wons(14000), 0.1, dc);

        // Action and Assert
        Assert.IsFalse(movie.IsDiscountable(new DateTime(2022, 3, 12, 13, 00, 00), 3));
    }
}