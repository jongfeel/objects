using System;
using System.Collections.ObjectModel;

public class Movie {
    private string title;
    private TimeSpan runningTime;
    public Money Fee { private set; get; }
    public ReadOnlyCollection<DiscountCondition> DiscountConditions { private set; get;}

    public MovieType MovieType { private set; get; }
    public Money DiscountAmount { private set; get; }
    public double DiscountPercent { private set; get; }

    public Movie(string title, TimeSpan runningTime, Money fee, double discountPercent, params DiscountCondition[] discountConditions)
        : this(MovieType.PERCENT_DISCOUNT, title, runningTime, fee, Money.ZERO, discountPercent, discountConditions)
    {
    }

    public Movie(string title, TimeSpan runningTime, Money fee, Money discountAmount, params DiscountCondition[] discountConditions)
        : this(MovieType.AMOUNT_DISCOUNT, title, runningTime, fee, discountAmount, 0, discountConditions)
    {    
    }

    public Movie(string title, TimeSpan runningTime, Money fee)
        : this(MovieType.NONE_DISCOUNT, title, runningTime, fee, Money.ZERO, 0)
    {    
    }

    private Movie(MovieType movieType, string title, TimeSpan runningTime, Money fee, Money discountAmount, double discountPercent, params DiscountCondition[] discountConditions)
    {
        this.MovieType = movieType;
        this.title = title;
        this.runningTime = runningTime;
        this.Fee = fee;
        this.DiscountAmount = discountAmount;
        this.DiscountPercent = discountPercent;
        this.DiscountConditions = Array.AsReadOnly(discountConditions);
    }

    public Money CalculateAmountDiscountedFee => MovieType == MovieType.AMOUNT_DISCOUNT ? Fee - DiscountAmount : Money.ZERO;

    public Money CalculatePercentDiscountedFee => MovieType == MovieType.PERCENT_DISCOUNT ? Fee - (Fee * DiscountPercent) : Money.ZERO;

    public Money CalculateNoneDiscountedFee => MovieType == MovieType.NONE_DISCOUNT ? Fee : Money.ZERO;

    public bool IsDiscountable(DateTime whenScreened, int sequence)
    {
        foreach (DiscountCondition condition in DiscountConditions)
        {
            if (condition.Type == DiscountConditionType.PERIOD && condition.IsDiscountable(whenScreened.DayOfWeek, whenScreened))
            {
                return true;
            }
            else
            {
                if (condition.IsDiscountable(sequence))
                {
                    return true;
                }
            }
        }

        return false;
    }
}