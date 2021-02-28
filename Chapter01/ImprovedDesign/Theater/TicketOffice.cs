using System.Collections.Generic;

public class TicketOffice {
    
    public long Amount { private set; get; }
    private Stack<Ticket> tickets;

    public TicketOffice(long amount, params Ticket[] tickets)
    {
        this.Amount = amount;
        this.tickets = new Stack<Ticket>(tickets);
    }

    public void SellTicketTo(Audience audience) => Amount += audience.Buy(Ticket);

    private Ticket Ticket => tickets.Pop();

    public int TicketCount => tickets.Count;
}