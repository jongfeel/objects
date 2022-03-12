using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest;

[TestClass]
public class DiscountConditionTests
{
    [TestMethod]
    public void IsDiscountable_PERIOD()
    {
        // Arrange
        DiscountCondition dc = new DiscountCondition()
        {
            Type = DiscountConditionType.PERIOD,
            DayOfWeek = DayOfWeek.Saturday,
            StartTime = new DateTime(2022, 3, 12, 12, 00, 00),
            EndTime = new DateTime(2022, 3, 12, 22, 00, 00)
        };

        // Action and Assert
        Assert.IsTrue(dc.IsDiscountable(DayOfWeek.Saturday, new DateTime(2022, 3, 12, 18, 00, 00)));
        Assert.IsFalse(dc.IsDiscountable(DayOfWeek.Saturday, new DateTime(2022, 3, 12, 8, 00, 00)));
        Assert.IsFalse(dc.IsDiscountable(DayOfWeek.Sunday, new DateTime(2022, 3, 12, 8, 00, 00)));
        Assert.IsFalse(dc.IsDiscountable(DayOfWeek.Sunday, new DateTime(2022, 3, 12, 8, 00, 00)));
    }

    [TestMethod]
    public void IsDiscountable_SEQUENCE()
    {
        // Arrange
        DiscountCondition dc = new DiscountCondition()
        {
            Type = DiscountConditionType.SEQUENCE,
            Sequence = 2
        };

        // Action and Assert
        Assert.IsTrue(dc.IsDiscountable(2));
        Assert.IsFalse(dc.IsDiscountable(1));
    }
}