using System;
using System.Collections.Generic;
using System.Linq;

public class Movie {
    private string title;
    private TimeSpan runningTime;
    public Money Fee { private set; get; }
    public IEnumerable<DiscountCondition> DiscountConditions { private set; get;}

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
        this.DiscountConditions = discountConditions.AsEnumerable();
    }
}