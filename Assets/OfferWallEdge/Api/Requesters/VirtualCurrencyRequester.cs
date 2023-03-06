namespace FyberPlugin
{

    public sealed class VirtualCurrencyRequester : Requester<VirtualCurrencyRequester, VirtualCurrencyCallback>
    {
        private const string NOTIFY_ON_REWARD_KEY = "notifyUserOnReward";
        private const string TRANSACTION_ID_KEY = "transactionId";
        private const string CURRENCY_ID_KEY = "currencyId";
        
        private VirtualCurrencyRequester()
        {
        }
        
        public static VirtualCurrencyRequester Create()
        {
            return new VirtualCurrencyRequester();
        }
        
        public VirtualCurrencyRequester WithTransactionId(string transactionId)
        {
            requesterAttributes[TRANSACTION_ID_KEY] = transactionId;
            return this;
        }
        
        public VirtualCurrencyRequester ForCurrencyId(string currencyId)
        {
            requesterAttributes[CURRENCY_ID_KEY] = currencyId;
            return this;
        }
        
        public VirtualCurrencyRequester NotifyUserOnReward(bool shouldNotifyUserOnReward)
        {
            requesterAttributes[NOTIFY_ON_REWARD_KEY] = shouldNotifyUserOnReward;
            return this;
        }

        protected override RequesterType GetRequester()
        {
            return RequesterType.VirtualCurrency;
        }

    }
}