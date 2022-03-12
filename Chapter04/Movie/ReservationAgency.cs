
public class ReservationAgency
{
    public Reservation Reserve(Screening screening, Customer customer, int audienceCount) => new Reservation(customer, screening, screening.CalculateFee(audienceCount), audienceCount);
}