namespace ZadanieCSharp
{
    /// <summary>
    /// Contain information about money value and its currency.
    /// </summary>
    public class MoneyAndCurrency
    {
        public float Money { get; set; }
        /// <summary>
        /// Currency in standard ISO 4217 format.
        /// </summary>
        public string Currency { get; set; }
    }
}
