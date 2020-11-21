using System;

public class Audience {
    
    private Bag bag;

    public Audience(Bag bag) => this.bag = bag;

    public long Buy(Ticket ticket) => bag.Hold(ticket);
}