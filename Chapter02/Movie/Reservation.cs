public class Reservation
{
    private Customer customer;
    private Screening screening;
    private Money fee;
    private int audienceCount;

    public Reservation(Customer customer, Screening screening, Money fee, int audienceCount)
    {
        this.customer = customer;
        this.screening = screening;
        this.fee = fee;
        this.audienceCount = audienceCount;
    }

    public override string ToString() => $"Name: {customer}, Fee: {fee}, audienceCount: {audienceCount}";
}