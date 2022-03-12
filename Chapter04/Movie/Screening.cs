using System;

public class Screening
{
    public Movie Movie { set; get; }
    public int Sequence { set; get; }
    public DateTime WhenScreened { set; get; }

    public Money CalculateFee(int audienceCount)
    {
        Money money = Movie.CalculateNoneDiscountedFee;
        if (Movie.IsDiscountable(WhenScreened, Sequence))
        {
            if (Movie.MovieType == MovieType.AMOUNT_DISCOUNT)
            {
                money = Movie.CalculateAmountDiscountedFee;
            }
            else if (Movie.MovieType == MovieType.PERCENT_DISCOUNT)
            {
                money = Movie.CalculatePercentDiscountedFee;
            }
        }

        return money * audienceCount;
    }
}