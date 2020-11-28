using System;

public class Bag {
    
    private long amount;
    public Invitation Invitation { private set; get; }
    private Ticket ticket;

    public Bag(long amount) : this(null, amount) {}

    public Bag(Invitation invitation, long amount)
    {
        Invitation = invitation;
        this.amount = amount;
    }

    public long Hold(Ticket ticket)
    {
        this.ticket = ticket;
        amount -= ticket.Fee;
        return ticket.Fee;
    }
}