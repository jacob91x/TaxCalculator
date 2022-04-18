using System;
using System.Collections.Generic;
using ZadanieCSharp;

namespace ZadanieCSarp_Test
{
    class ApplicationTest
    {
        public static TaxCalculator taxCalculator = new TaxCalculator();

        static void Main(string[] args)
        {
            RunTest();
        }

        private static void RunTest()
        {
            List<MonthSalary> monthSalaries = new List<MonthSalary>();
            monthSalaries.Add(new MonthSalary
            {
                Date = DateTime.Parse("2022-01-03"),
                Salary = new List<MoneyAndCurrency>
            {
                new MoneyAndCurrency { Money = 180200.55F, Currency = "pln" },
                new MoneyAndCurrency { Money = 1000F, Currency = "eur" },
                new MoneyAndCurrency { Money = 1000F, Currency = "usd" }
            }
            });

            float tax = taxCalculator.CalculateTax(monthSalaries, TaxKind.Progressive);
            Console.WriteLine("Wyliczony podatek: " + tax + " zł");
            Console.ReadKey();
        }
    }
}
