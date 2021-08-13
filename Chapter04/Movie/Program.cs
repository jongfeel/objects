using System;

namespace MovieProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            // Reservation movie by DiscountCondition
            DiscountCondition dc = new DiscountCondition();
            dc.Type = DiscountConditionType.PERIOD;
            dc.DayOfWeek = DayOfWeek.Friday;
            dc.StartTime = new DateTime(2021, 8, 13, 17, 0, 0);
            dc.EndTime = new DateTime(2021, 8, 13, 21, 0, 0);

            Movie freeguy = new Movie("Free guy", TimeSpan.FromMinutes(115), Money.Wons(14000), Money.Wons(5000), dc);

            Screening screening = new Screening();
            screening.Movie = freeguy;
            screening.Sequence = 1;
            screening.WhenScreened = new DateTime(2021, 8, 13, 18, 30, 0);

            Reservation reservation = new ReservationAgency().Reserve(screening, new Customer("jongfeel", "feel"), 1);

            Console.WriteLine("Movie.Fee=" + freeguy.Fee);
            Console.WriteLine("Reservation.Fee=" + reservation.Fee);
            
        }
    }
}
