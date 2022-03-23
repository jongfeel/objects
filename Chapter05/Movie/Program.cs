// See https://aka.ms/new-console-template for more information
Console.WriteLine("Chapter 05, Movie program");

// Reservation movie by DiscountCondition
PeriodCondition periodCondition = new PeriodCondition(
    DayOfWeek.Wednesday,
    new DateTime(2022, 3, 23, 12, 0, 0),
    new DateTime(2022, 3, 23, 22, 0, 0)
);

SequenceCondition sequenceCondition = new SequenceCondition(1);

Movie theBatman = new AmountDiscountMovie("The Batman", TimeSpan.FromMinutes(176), Money.Wons(14000), Money.Wons(2000),
    new DiscountCondition[] { periodCondition, sequenceCondition });

Screening screening = new Screening();
screening.Movie = theBatman;
screening.Sequence = 1;
screening.WhenScreened = new DateTime(2022, 3, 19, 18, 30, 0);

Reservation reservation = new Reservation(new Customer("jongfeel", "feel"), screening, Money.Wons(14000), 1);

Console.WriteLine("Movie.Fee=" + theBatman.CalculateMovieFee(screening));
Console.WriteLine("Reservation.Fee=" + reservation.Fee);
