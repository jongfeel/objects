public class Screening
{
    public Movie Movie { set; get; }
    public int Sequence { set; get;}
    public DateTime WhenScreened { set; get;}
    public Reservation reserve(Customer customer, int audienceCount) => new Reservation(customer, this, CalculateFee(audienceCount), audienceCount);

    private Money CalculateFee(int audienceCount) => Movie.CalculateMovieFee(this) * audienceCount;
}