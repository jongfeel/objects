using System;
using System.Collections.ObjectModel;

public class Movie {
    public string Title { private set; get; }
    public TimeSpan RunningTime { private set; get; }
    public Money Fee { set; get; }
    private List<PeriodCondition> PeriodConditions;
    private List<SequenceCondition> SequenceConditions;

    public MovieType MovieType { set; get; }
    public Money DiscountAmount { set; get; }
    public double DiscountPercent { set; get; }

    public Movie(string title, TimeSpan runningTime, Money fee, List<PeriodCondition> periodConditions, List<SequenceCondition> sequenceConditions)
    {
        Title = title;
        RunningTime = runningTime;
        Fee = fee;
        PeriodConditions = periodConditions;
        SequenceConditions = sequenceConditions;
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

    private bool IsDiscountable(Screening screening) => CheckPeriodConditions(screening) || CheckSequenceConditions(screening);

    private bool CheckPeriodConditions(Screening screening) => PeriodConditions.Any(condition => condition.IsSatisfiedBy(screening));

    private bool CheckSequenceConditions(Screening screening) => SequenceConditions.Any(condition => condition.IsSatisfiedBy(screening));
}