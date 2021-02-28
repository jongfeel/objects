using System;

public class Bag {
    
    public long Amount { private set; get; }
    public Invitation Invitation { private set; get; }
    private Ticket ticket;

    public Bag(long amount) : this(null, amount) {}

    public Bag(Invitation invitation, long amount)
    {
        Invitation = invitation;
        this.Amount = amount;
    }

    public long Hold(Ticket ticket)
    {
        this.ticket = ticket;
        if (Invitation != null) // has invitations
        {
            ticket.InvitationExchanged();
            Invitation = null;
        }

        Amount -= ticket.Fee;
        return ticket.Fee;
    }
}