using System;
using System.Collections.Generic;

namespace FyberPlugin
{
	internal sealed class FyberCallbacksManager
	{

		private static FyberCallbacksManager instance;

		public static FyberCallbacksManager Instance
		{
			get
			{
				if (instance == null)
					instance = new FyberCallbacksManager();
				return instance;
			}
		}

		private Dictionary<string, object> callbacks;
		private Dictionary<string, Ad>  ads;

		private FyberCallbacksManager()
		{
			callbacks = new Dictionary<string, object>();
			ads = new Dictionary<string, Ad>();
		}

		internal static void AddCallback(string guid, object callback)
		{
			Instance.callbacks[guid] = callback;
		}

		internal static void AddAd(string guid, Ad ad)
		{
			Instance.ads[guid] = ad;
		}

		internal void Process(NativeMessage message)
		{
            if (callbacks.ContainsKey(message.Id) || message.Id.Equals("testsuite", StringComparison.Ordinal)) // for test suite there are no callbacks, that's why we check it separately
			{
				switch (message.Type)
				{
				case 0: //Request
					ProcessRequestCallback(message);
					break;
				case 1: //Ad
					ProcessAdCallback(message);
                    break;                
				default:
					ClearCallbacks(message.Id);
					FyberCallback.Instance.OnNativeError("An unknown error occurred.");
					break;
				}
			}
			else
			{
				ClearCallbacks(message.Id);
				FyberCallback.Instance.OnNativeError("An unknown error occurred. Please, request Ads again.");
			}

		}

		private void ProcessAdCallback(NativeMessage message)
		{
			if (message.AdPayload == null)
			{
				FyberCallback.Instance.OnNativeError("An unknown error occurred. Please, request Ads again.");
				ClearCallbacks(message.Id);
				return;
			}

			if (message.Origin == 0)			
			 	//"offerwall":
				FireAdCallback(message, AdFormat.OFFER_WALL);

			else
			{
				ClearCallbacks(message.Id);
				FyberCallback.Instance.OnNativeError("An unknown error occurred. Please, request Ads again.");
			}
		}

		internal void ClearCallbacks(string id)
		{
			if (ads.ContainsKey(id)) {
				Ad ad = ads [id];
				ads.Remove (id);
				callbacks.Remove (id);
			} 
		}

		private void FireAdCallback(NativeMessage message, AdFormat adFormat)
		{
			var adPayload = message.AdPayload;
			AdCallback callback = GetCallback<AdCallback>(message.Id, !adPayload.AdStarted);
			if (adPayload.AdStarted)
			{
				if (ads.ContainsKey(message.Id))
				{
					Ad ad = ads[message.Id];
					// only remove if it's not a banner
					ads.Remove(message.Id);
					callback.OnAdStarted(ad);
				}
				else
				{
					ClearCallbacks(message.Id);
					FyberCallback.Instance.OnNativeError("An unknown error occurred. Please, request Ads again.");
				}
			}
			else
			{
				AdResult result = new AdResult();
				if (string.IsNullOrEmpty(adPayload.Error))
				{
					result.Status = AdStatus.OK;
					result.Message = adPayload.Status;
				}
				else
				{
					result.Status = AdStatus.Error;
					result.Message = adPayload.Error;
				}
				result.AdFormat = adFormat;
				callback.OnAdFinished(result);
			}
		}

		private void ProcessRequestCallback(NativeMessage message)
		{
			if (message.RequestPayload == null)
			{
				ClearCallbacks(message.Id);
				FyberCallback.Instance.OnNativeError("An unknown error occurred. Please, request Ads again.");
				return;
			}
			if (message.RequestPayload.RequestError != null)
			{
				Callback callback = GetCallback<Callback>(message.Id);
				callback.OnRequestError(RequestError.FromNative(message.RequestPayload.RequestError.Value));
			}
			else
			{
				switch (message.Origin)
				{
				case 0: //"offerwall":
					FireRequestCallback(message, AdFormat.OFFER_WALL);
					break;				
				case 4: //"currency":
					VirtualCurrencyCallback callback = GetCallback<VirtualCurrencyCallback>(message.Id);
					if (message.RequestPayload.CurrencyResponse != null)
						callback.OnSuccess(message.RequestPayload.CurrencyResponse);
					else
						callback.OnError(message.RequestPayload.CurrencyErrorResponse);
					break;
				default:
					ClearCallbacks(message.Id);
					FyberCallback.Instance.OnNativeError("An unknown error occurred. Please, request Ads again.");
					break;
				}
			}
		}

		private void FireRequestCallback(NativeMessage message, AdFormat format)
		{
			RequestCallback callback = GetCallback<RequestCallback>(message.Id);
			if (message.RequestPayload.AdAvailable.GetValueOrDefault())
			{
				Ad ad = new Ad(message.Id);
                ad.AdFormat = format;

				if (!string.IsNullOrEmpty(message.RequestPayload.PlacementId))
					ad.PlacementId = message.RequestPayload.PlacementId;

				callback.OnAdAvailable(ad as Ad);
			}
			else
				callback.OnAdNotAvailable(format);
		}

		private T GetCallback<T>(string id, bool remove = true) where T : class
		{
			T callback = callbacks[id] as T;
			if (remove)
				callbacks.Remove(id);
			return callback;
		}

	}

}
