using System;

public class Ticket {
    
    private long fee;

    public long Fee => fee;

    public Ticket(long fee) => this.fee = fee;
}