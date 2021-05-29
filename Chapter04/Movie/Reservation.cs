using System;

public class Reservation
{
    public Customer Customer { set; get; }
    public Screening Screening { set; get; }
    public Money Fee { set; get; }
    public int AudienceCount { set; get; }

    public Reservation(Customer customer, Screening screening, Money fee, int audienceCount)
    {
        Customer = customer;
        Screening = screening;
        Fee = fee;
        AudienceCount = audienceCount;
    }
}