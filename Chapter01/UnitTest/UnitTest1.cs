using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            Ticket[] tickets = new Ticket[50];
            for (int i = 0; i < 50; i++)
            {
                tickets[i] = new Ticket();
            }

            int officeProperty = 1000000;
            TicketOffice ticketOffice = new TicketOffice(1000000, tickets);
            TicketSeller ticketSeller = new TicketSeller(ticketOffice);
            Theater1 theater = new Theater1(ticketSeller);

            int audienceCash = 20000;
            Bag bag = new Bag(audienceCash);
            Audience jisoo = new Audience(bag);

            // Act
            theater.Enter(jisoo);

            // Assert
            Assert.AreEqual(officeProperty + audienceCash, ticketOffice.Amount + jisoo.Amount);
        }
    }
}
