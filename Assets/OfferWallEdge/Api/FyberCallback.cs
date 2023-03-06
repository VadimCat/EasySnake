using System;
using FyberPlugin.LitJson;

namespace FyberPlugin
{

    public class FyberCallback : RequestCallback, VirtualCurrencyCallback, AdCallback
    {
        
        public static event Action<Ad> AdAvailable;
        public static event Action<AdFormat> AdNotAvailable;
        public static event Action<VirtualCurrencyResponse> VirtualCurrencySuccess;
        public static event Action<VirtualCurrencyErrorResponse> VirtualCurrencyError;
        public static event Action<RequestError> RequestFail;
        public static event Action<Ad> AdStarted;
        public static event Action<AdResult> AdFinished;
        public static event Action<string> NativeError;

        public static event Action<Boolean> VideoCached;

        private static FyberCallback instance;
        
        public static FyberCallback Instance
        {
            get
            {
                if (instance == null)
                    instance = new FyberCallback();
                return instance;
            }
        }
        
        internal FyberCallback()
        {
        }
        
        public void OnAdAvailable(Ad ad)
        {
            if (AdAvailable != null)
                AdAvailable(ad);
        }

        public void OnAdNotAvailable(AdFormat adFormat)
        {
            if (AdNotAvailable != null)
                AdNotAvailable(adFormat);
        }

        public void OnError(VirtualCurrencyErrorResponse response)
        {
            if (VirtualCurrencyError != null)
                VirtualCurrencyError(response);
        }

        public void OnSuccess(VirtualCurrencyResponse response)
        {
            if (VirtualCurrencySuccess != null)
                VirtualCurrencySuccess(response);
        }

        public void OnRequestError(RequestError error)
        {
            if (RequestFail != null)
                RequestFail(error);
        }
        
        public void OnAdStarted(Ad ad)
        {
            if (AdStarted != null)
                AdStarted(ad);
        }
  
        public void OnAdFinished(AdResult result)
        {
            if (AdFinished != null)
                AdFinished(result);
        }

		public void OnNativeError(String message)
        {
            if (NativeError != null)
                NativeError(message);
        }

		public void OnVideoCached(Boolean videosAvailable)
        {
			if (VideoCached != null)
				VideoCached(videosAvailable);
		}

    }

}
