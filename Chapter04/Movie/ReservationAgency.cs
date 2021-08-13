
public class ReservationAgency
{
    public Reservation Reserve(Screening screening, Customer customer, int audienceCount)
    {
        Movie movie = screening.Movie;

        bool discountable = false;
        foreach (DiscountCondition condition in movie.DiscountConditions)
        {
            if (condition.Type == DiscountConditionType.PERIOD)
            {
                discountable = screening.WhenScreened.DayOfWeek == condition.DayOfWeek &&
                        condition.StartTime <= screening.WhenScreened &&
                        condition.EndTime >= screening.WhenScreened;
            }
            else
            {
                discountable = condition.Sequence == screening.Sequence;
            }

            if (discountable)
            {
                break;
            }
        }

        Money fee;
        if (discountable)
        {
            Money discountAmount = Money.ZERO;
            switch (movie.MovieType)
            {
                case MovieType.AMOUNT_DISCOUNT:
                    discountAmount = movie.DiscountAmount;
                    break;
                case MovieType.PERCENT_DISCOUNT:
                    discountAmount = movie.Fee * movie.DiscountPercent;
                    break;
                case MovieType.NONE_DISCOUNT:
                    discountAmount = Money.ZERO;
                    break;
            }

            fee = (movie.Fee - discountAmount) * audienceCount;
        }
        else
        {
            fee = movie.Fee * audienceCount;
        }

        return new Reservation(customer, screening, fee, audienceCount);
    }
}