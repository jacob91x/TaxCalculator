using System;
using System.Collections.Generic;

namespace ZadanieCSharp
{
    /// <summary>
    /// Contain date of the month and list of incomes with currency information.
    /// </summary>
    public class MonthSalary
    {
        /// <summary>
        /// Date with year, month and day needed.
        /// </summary>
        public DateTime Date { get; set; }
        public List<MoneyAndCurrency> Salary { get; set; }
    }
}
