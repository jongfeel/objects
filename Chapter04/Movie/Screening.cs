using System;

public class Screening
{
    private Movie movie;
    private int sequence;
    private DateTime whenScreened;

    public Screening(Movie movie, int sequence, DateTime whenScreened)
    {
        this.movie = movie;
        this.sequence = sequence;
        this.whenScreened = whenScreened;
    }

    public DateTime StartTime => whenScreened;

    public bool IsSequence(int sequence) => this.sequence == sequence;

    public Money MovieFee => movie.Fee;

    // public Reservation Reserve(Customer customer, int audienceCount) =>
    //     new Reservation(customer, this, CalculateFee(audienceCount), audienceCount);

    //private Money CalculateFee(int audienceCount) => movie.CalculateMovieFee(this) * audienceCount;
}