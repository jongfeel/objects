using System;

public class Audience {
    
    private Bag bag;

    public long Amount => bag.Amount;

    public Audience(Bag bag) => this.bag = bag;

    public long Buy(Ticket ticket) => bag.Hold(ticket);

    public Invitation Invitation => bag.Invitation;
}