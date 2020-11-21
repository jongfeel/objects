public class Theater1 {
    
    private TicketSeller ticketSeller;

    public Theater1(TicketSeller ticketSeller) => this.ticketSeller = ticketSeller;

    public void Enter(Audience audience) => ticketSeller.SellTo(audience);
}