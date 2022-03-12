using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest;

[TestClass]
public class ReservationAgencyTests
{
    [TestMethod]
    public void CreateReservation()
    {
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
        Customer customer = new Customer("jongfeel", "0");

        // Action
        Reservation reservation = new ReservationAgency().Reserve(screening, customer, 1);

        // Assert
        Assert.IsNotNull(new ReservationAgency().Reserve(screening, customer, 1));
        Assert.AreEqual(reservation.Customer, customer);
        Assert.AreEqual(reservation.Screening, screening);
        Assert.AreEqual(reservation.Fee, Money.Wons(12000));
        Assert.AreEqual(reservation.AudienceCount, 1);
    }
}