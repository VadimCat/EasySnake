using System;

namespace FyberPlugin
{
    public class VirtualCurrencyResponse
    {
        /**
         *
         * @return number of coins the User has earned since the last request to Virtual Currency Server.
         */
        public double DeltaOfCoins { get; set; }

        /**
         *
         * @return the last transaction id.
         */
        public string LatestTransactionId { get; set; }
        /**
         *
         * @return Currency id.
         */
        public string CurrencyId { get; set; }

        /**
         *
         * @return currency name.
         */
        public string CurrencyName { get; set; }

        /**
         * @return true if currency id being used is the default one.
         */
        public Boolean DefaultCurrency { get; set; }
    }
}