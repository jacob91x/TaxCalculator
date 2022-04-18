using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;

namespace ZadanieCSharp
{
    public class TaxCalculator
    {
        private DateTime _nbpMinDate = new DateTime(2002, 01, 02);

        /// <summary>
        /// Calculates tax from given incomes.
        /// </summary>
        /// <param name="monthSalaries">List of MonthSalary.</param>
        /// <param name="taxKind">Contain type of used tax.</param>
        /// <returns>Total value of tax.</returns>
        public float CalculateTax(List<MonthSalary> monthSalaries, TaxKind taxKind)
        {
            float totalSalary = ConvertToPln(monthSalaries);
            return CalculateTaxOnTotalIncome(totalSalary, taxKind);
        }

        private float ConvertToPln(List<MonthSalary> monthSalaries)
        {
            float totalSalary = 0;

            foreach (var monthSalary in monthSalaries)
            {
                foreach (var salary in monthSalary.Salary)
                {
                    if (monthSalary.Date < _nbpMinDate)
                    {
                        throw new Exception("Wrong date. Date must be younger than: " + _nbpMinDate);
                    }

                    float exchangeRate = GetExchangeRate(monthSalary.Date, salary.Currency);
                    totalSalary += salary.Money * exchangeRate;
                }
            }
            return totalSalary;
        }

        private float GetExchangeRate(DateTime date, string currency)
        {
            float exchangeRate = 0;

            if (currency.ToString() == "pln")
            {
                exchangeRate = 1;
            }
            else
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create("http://api.nbp.pl/api/exchangerates/rates/A/" + currency + "/" + date.ToString("yyyy-MM-dd") + "/?format=xml");
                    var response = (HttpWebResponse)request.GetResponse();
                    
                    var stream = response.GetResponseStream();
                    var reader = new StreamReader(stream);
                    var xmlDocument = new XmlDocument();
                    var xml = reader.ReadToEnd();
                    xmlDocument.LoadXml(xml);
                    var rate = xmlDocument.SelectNodes("/ExchangeRatesSeries/Rates/Rate")[0];
                    exchangeRate = float.Parse(rate["Mid"].InnerText, CultureInfo.InvariantCulture.NumberFormat);                    
                }
                catch (Exception e)
                {
                    throw new Exception("Error in downloading the exchange rate." + e.Message);
                }
            }
            return exchangeRate;
        }

        private float CalculateTaxOnTotalIncome(float totalSalary, TaxKind taxKind)
        {
            float totalTax = 0;

            switch (taxKind)
            {
                case TaxKind.TaxFree:
                    break;

                case TaxKind.Linear:
                    totalTax = totalSalary * 0.19F;
                    break;

                case TaxKind.Progressive:

                    if (totalSalary <= 85528)
                    {
                        totalTax = totalSalary * 0.17F;
                    }
                    else if (totalSalary > 85528)
                    {
                        totalTax = 85528 * 0.17F + (totalSalary - 85528) * 0.32F;
                    }

                    totalTax -= 525.12F;

                    if (totalTax < 0)
                    {
                        totalTax = 0;
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
            return (float)Math.Floor(totalTax);
        }
    }
}
