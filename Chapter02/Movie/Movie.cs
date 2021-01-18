using System;

public class Movie
{
    private string title;
    private TimeSpan runningTime;
    private Money fee;
    private DiscountPolicy discountPolicy;

    public Movie(string title, TimeSpan runningTime, Money fee, DiscountPolicy discountPolicy) {
        this.title = title;
        this.runningTime = runningTime;
        this.fee = fee;
        this.discountPolicy = discountPolicy;
    }

    public Money Fee => fee;

    public Money CalculateMovieFee(Screening screening) =>
        discountPolicy == null ? fee : fee.Minus(discountPolicy.CalculateDiscountAmount(screening));
}