using System.Collections.Generic;
using System.Linq;

public class TicketOffice {
    
    private long amount;
    private List<Ticket> tickets = new List<Ticket>();

    public TicketOffice(long amount, params Ticket[] tickets)
    {
        this.amount = amount;
        this.tickets.AddRange(tickets);
    }

    public void SellTicketTo(Audience audience) => amount += audience.Buy(Ticket);

    public Ticket Ticket
    {
        get
        {
            Ticket t = tickets[0];
            tickets.RemoveAt(0);
            return t;
        }
    }
}