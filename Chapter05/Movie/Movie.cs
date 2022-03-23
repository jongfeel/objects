using System;
using System.Collections.ObjectModel;

public abstract class Movie {
    public string Title { private set; get; }
    public TimeSpan RunningTime { private set; get; }
    protected Money Fee { private set; get; }
    private List<DiscountCondition> discountConditions;

    public MovieType MovieType { set; get; }

    public Movie(string title, TimeSpan runningTime, Money fee, params DiscountCondition[] discountConditions)
    {
        Title = title;
        RunningTime = runningTime;
        Fee = fee;
        this.discountConditions = new List<DiscountCondition>(discountConditions);
    }
    abstract protected Money CalculateDiscountAmount();

    public Money CalculateMovieFee(Screening screening) => IsDiscountable(screening) ? Fee - CalculateDiscountAmount() : Fee;

    private bool IsDiscountable(Screening screening) => discountConditions.Any(condition => condition.IsSatisfiedBy(screening));
}