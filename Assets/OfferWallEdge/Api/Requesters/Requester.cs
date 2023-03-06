using System;
using System.Collections.Generic;
using FyberPlugin.LitJson;

namespace FyberPlugin
{

    public abstract class Requester<T, V> 
        where V : class, Callback
        where T : Requester<T, V> 
    {

        protected const string CUSTOM_PARAMS_KEY = "parameters";
        private const string REQUEST_ID = "requestId";
        private const string REQUESTER = "requester";
        protected const string PLACEMENT_ID_KEY = "placementId";

        protected Dictionary<string, object> requesterAttributes;
        protected V requestCallback;

        public virtual void Request()
        {
            GetRequesterAttributes();
            string json = JsonMapper.ToJson(requesterAttributes);
            PluginBridge.Request(json);
        }

        protected abstract RequesterType GetRequester();


        // the main rational behind this method is to ease the 
        // vcs automatic query for the RV requester
        internal Dictionary<string, object> GetRequesterAttributes()
        {
            requesterAttributes[REQUEST_ID] = GenerateUidAndAddToQueue();
            requesterAttributes[REQUESTER] = GetRequester();
            return requesterAttributes;
        } 
                
        //same here 
        internal string GenerateUidAndAddToQueue()
        {
            var guid = Guid.NewGuid().ToString();

            if (requestCallback == null)
                requestCallback = FyberCallback.Instance as V;

            FyberCallbacksManager.AddCallback(guid, requestCallback);
            return guid;
        }

        public Requester()
        {
            requesterAttributes = new Dictionary<string, object>();
        }

        public T WithPlacementId(string placementId)
        {
            requesterAttributes[PLACEMENT_ID_KEY] = placementId;
            return this as T;
        }

        public T WithCallback(V requestCallback)
        {
            this.requestCallback = requestCallback;
            return this as T;
        }

        public T AddParameters(Dictionary<string, string> parameters)
        {
            Dictionary<string, string> customParams = GetCustomParameters();
            foreach (var item in parameters)
            {
                customParams[item.Key] = item.Value;
            }
            return this as T;
        }
        public T AddParameter(string key, string value)
        {
            Dictionary<string, string> customParams = GetCustomParameters();
            customParams[key] = value;
            return this as T;
        }
        public T ClearParameters()
        {
			// we can optimize this
            GetCustomParameters().Clear();
            return this as T;
        }
        public T RemoveParameter(string key)
        {
            GetCustomParameters().Remove(key);
            return this as T;
        }

        protected Dictionary<string, string> GetCustomParameters()
        {
            if (!requesterAttributes.ContainsKey(CUSTOM_PARAMS_KEY))
                requesterAttributes[CUSTOM_PARAMS_KEY] = new Dictionary<string, string>();
            
            return requesterAttributes[CUSTOM_PARAMS_KEY] as Dictionary<string, string>;
        }
        
        protected enum RequesterType
        {
            OfferWall = 0,
            VirtualCurrency = 4
        }

    }
}

