using System;

public class Bag {
    
    private long amount;
    private Invitation invitation;
    private Ticket ticket;

    public Bag(long amount) : this(null, amount) {}

    public Bag(Invitation invitation, long amount)
    {
        this.invitation = invitation;
        this.amount = amount;
    }

    public long Hold(Ticket ticket)
    {
        if (HasInvitation)
        {
            SetTicket(ticket);
            return 0;
        }
        else
        {
            SetTicket(ticket);
            MinusAmount(ticket.Fee);
            return ticket.Fee;
        }
    }

    public bool HasInvitation => invitation != null;

    public void SetTicket(Ticket ticket) => this.ticket = ticket;

    public void MinusAmount(long amount) => this.amount -= amount;
}