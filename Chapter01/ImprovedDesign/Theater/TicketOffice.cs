using System.Collections.Generic;

public class TicketOffice {
    
    private long amount;
    private Stack<Ticket> tickets;

    public TicketOffice(long amount, params Ticket[] tickets)
    {
        this.amount = amount;
        this.tickets = new Stack<Ticket>(tickets);
    }

    public void SellTicketTo(Audience audience)
    {
        if (audience.Invitation != null)
        {
            Ticket.InvitationExchanged();
        }
        
        amount += audience.Buy(Ticket);
    }

    private Ticket Ticket => tickets.Pop();

    public int TicketCount => tickets.Count;
}