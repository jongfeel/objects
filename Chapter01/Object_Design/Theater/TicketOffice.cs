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

    public Ticket Ticket
    {
        get
        {
            Ticket t = tickets[0];
            tickets.RemoveAt(0);
            return t;
        }
    }

    public void MinusAmount(long amount) => this.amount -= amount;

    public void PlusAmount(long amount) => this.amount += amount;
}