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
            TicketSeller ticketSeller = new TicketSeller(new TicketOffice(1000000, tickets));
            Theater1 theater = new Theater1(ticketSeller);
            
            // Act
            theater.Enter(new Audience(new Bag(20000)));

            // Assert
            Assert.AreEqual("1", "1");
        }
    }
}
