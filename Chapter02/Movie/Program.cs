using System;

namespace MovieProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            // SequenceCondition + AmountDiscountPolicy
            SequenceCondition sc1 = new SequenceCondition(1);
            SequenceCondition sc2 = new SequenceCondition(2);
            AmountDiscountPolicy adp = new AmountDiscountPolicy(Money.Wons(10000), sc1, sc2);

            Movie GodzillaVSKong = new Movie("고질라VS.콩", TimeSpan.FromMinutes(113), Money.Wons(18000), adp);

            Screening screening = new Screening(GodzillaVSKong, 1, new DateTime(2021, 4, 3));
            Reservation reservation = screening.Reserve(new Customer(), 1);

            Console.WriteLine(reservation);

            // ---

            // PeriodCondition + PercentDiscountPolicy
            PeriodCondition pc1 = new PeriodCondition(DayOfWeek.Saturday, TimeSpan.FromHours(9), TimeSpan.FromHours(13));
            PeriodCondition pc2 = new PeriodCondition(DayOfWeek.Sunday, TimeSpan.FromHours(9), TimeSpan.FromHours(13));
            PeriodCondition pc3 = new PeriodCondition(DayOfWeek.Monday, TimeSpan.FromHours(18), TimeSpan.FromHours(24));
            PercentDiscountPolicy pdp = new PercentDiscountPolicy(0.1, pc1, pc2, pc3);

            Movie theBookOfFish = new Movie("자산어보", TimeSpan.FromMinutes(126), Money.Wons(12000), pdp);

            Screening screening2 = new Screening(theBookOfFish, 3, new DateTime(2021, 4, 4, 11, 50, 00));
            Reservation reservation2 = screening2.Reserve(new Customer() { Name = "jongfeel"}, 1);

            Console.WriteLine(reservation2);
        }
    }
}
