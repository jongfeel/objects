// See https://aka.ms/new-console-template for more information
Console.WriteLine("Chapter 05, Movie program");

// Reservation movie by DiscountCondition
DiscountCondition dc = new DiscountCondition();
dc.Type = DiscountConditionType.PERIOD;
dc.DayOfWeek = DayOfWeek.Saturday;
dc.StartTime = new DateTime(2022, 3, 19, 12, 0, 0);
dc.EndTime = new DateTime(2022, 3, 19, 22, 0, 0);

Movie theBatman = new Movie(new DiscountCondition[] { dc });
theBatman.Fee = Money.Wons(14000);
theBatman.MovieType = MovieType.AMOUNT_DISCOUNT;
theBatman.DiscountAmount = Money.Wons(2000);

Screening screening = new Screening();
screening.Movie = theBatman;
screening.Sequence = 1;
screening.WhenScreened = new DateTime(2022, 3, 19, 18, 30, 0);

Reservation reservation = new Reservation(new Customer("jongfeel", "feel"), screening, Money.Wons(14000), 1);

Console.WriteLine("Movie.Fee=" + theBatman.CalculateMovieFee(screening));
Console.WriteLine("Reservation.Fee=" + reservation.Fee);
