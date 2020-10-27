using System;

namespace TheaterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Theater example!");

            // Create tickets and ready to set up theater with ticket office, ticket seller
            Ticket[] tickets = new Ticket[50];
            for (int i = 0; i < 50; i++)
            {
                tickets[i] = new Ticket();
            }
            TicketSeller ticketSeller = new TicketSeller(new TicketOffice(1000000, tickets));
            
            // Create five audiences with bags
            // Three bags has no invitation
            // Two bags has invitation
            Audience jisoo = new Audience(new Bag(20000));
            Audience rose = new Audience(new Bag(30000));
            Audience lisa = new Audience(new Bag(60000));
            Audience jennie = new Audience(new Bag(new Invitation(), 30));
            Audience feel = new Audience(new Bag(new Invitation(), 40));

            // Audience enter to theater
            Theater1 theater = new Theater1(ticketSeller);
            theater.Enter(jisoo);
            theater.Enter(rose);
            theater.Enter(lisa);
            theater.Enter(jennie);
            theater.Enter(feel);
        }
    }
}
