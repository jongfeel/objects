using System;
using System.Collections.ObjectModel;

public class Movie {
    private string title;
    private TimeSpan runningTime;
    public Money Fee { set; get; }
    private List<DiscountCondition> DiscountConditions;

    public MovieType MovieType { set; get; }
    public Money DiscountAmount { set; get; }
    private double DiscountPercent;

    public Movie(DiscountCondition[] discountConditions) => DiscountConditions = new (discountConditions);

    private Money CalcualteDiscountAmount()
    {
        switch (MovieType)
        {
            case MovieType.AMOUNT_DISCOUNT:
                return CalculateAmountDiscountAmount;
            case MovieType.PERCENT_DISCOUNT:
                return CalculatePercentDiscountAmount;
        }

        return CalculateNoneDiscountAmount;
    }

    private Money CalculateAmountDiscountAmount => DiscountAmount;

    private Money CalculatePercentDiscountAmount => DiscountAmount * DiscountPercent;

    private Money CalculateNoneDiscountAmount => Money.ZERO;

    public Money CalculateMovieFee(Screening screening)
    {
        if (IsDiscountable(screening))
        {
            return Fee - CalcualteDiscountAmount();
        }
        return Fee;
    }

    private bool IsDiscountable(Screening screening) => DiscountConditions.Any(condition => condition.IsSatisfiedBy(screening));
}