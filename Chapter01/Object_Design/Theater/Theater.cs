public class Theater1 {
    
    private TicketSeller ticketSeller;

    public Theater1(TicketSeller ticketSeller) => this.ticketSeller = ticketSeller;

    public void Enter(Audience audience)
    {
        if (audience.Bag.HasInvitation)
        {
            Ticket ticket = ticketSeller.TicketOffice.Ticket;
            audience.Bag.SetTicket(ticket);
        }
        else
        {
            Ticket ticket = ticketSeller.TicketOffice.Ticket;
            audience.Bag.MinusAmount(ticket.Fee);
            ticketSeller.TicketOffice.PlusAmount(ticket.Fee);
            audience.Bag.SetTicket(ticket);
        }
    }
}