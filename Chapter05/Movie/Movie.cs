using System;
using System.Collections.ObjectModel;

public class Movie {
    public string Title { private set; get; }
    public TimeSpan RunningTime { private set; get; }
    public Money Fee { set; get; }
    private List<DiscountCondition> discountConditions;

    public MovieType MovieType { set; get; }
    public Money DiscountAmount { set; get; }
    public double DiscountPercent { set; get; }

    public Movie(string title, TimeSpan runningTime, Money fee, List<DiscountCondition> discountConditions)
    {
        Title = title;
        RunningTime = runningTime;
        Fee = fee;
        this.discountConditions = discountConditions;
    }
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

    private bool IsDiscountable(Screening screening) => discountConditions.Any(condition => condition.IsSatisfiedBy(screening));
}