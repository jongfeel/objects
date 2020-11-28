using System;

public class Ticket {
    
    public long Fee { get; set; } = 10000;

    public void InvitationExchanged() => Fee = 0;
}