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
        this.ticket = ticket;
        long fee = HasInvitation ? 0 : ticket.Fee;
        amount -= fee;
        return fee;
    }
}