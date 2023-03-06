using FyberPlugin.LitJson;
using System;
using System.Collections.Generic;

namespace FyberPlugin
{

	public class Ad
	{
		public AdFormat AdFormat { get; internal set; }
		public string PlacementId { get; internal set; }

		protected bool Started = false;
		protected string guid;

		private AdCallback adCallback { get; set; }

		internal Ad(string guid)
		{
			this.guid = guid;
		}

		public Ad WithCallback(AdCallback callback)
		{
			adCallback = callback;
			return this;
		}

        // TODO check naming
		public void Start()
		{
			if (!Started && CanStart())
			{
				Started = true;
				var guid = GetGuid();
				UnityEngine.Debug.Log("Start - " + guid);


				RegisterCallbacks(guid);

				Dictionary<string, object> dict = new Dictionary<string,object>(3);
				dict["id"] = guid;
				dict["ad"] = AdFormat;

				if (!string.IsNullOrEmpty(PlacementId))
					dict["placementId"] = PlacementId;

				UnityEngine.Debug.Log("dict - " + dict["id"]);


				AddExtraInfo(dict);

				PluginBridge.StartAd(JsonMapper.ToJson(dict));
			}
			else if (!Started && !CanStart())
			{
				FyberCallback.Instance.OnNativeError("A request operation is in progress");
			}
			else if (Started)
			{
				FyberCallback.Instance.OnNativeError("This ad was already shown. You should request a new one");
			}
			else 
			{
				FyberCallback.Instance.OnNativeError("An unexpected condition happened. Please request for a new ad");
			}
		}

		protected virtual Boolean CanStart()
		{
			return true;
		}

		protected virtual string GetGuid()
		{
			if ( guid == null)
				guid = Guid.NewGuid().ToString();

			return guid;
		}

		protected virtual void RegisterCallbacks (string guid)
		{
			if (adCallback == null)
				adCallback = FyberCallback.Instance;

			FyberCallbacksManager.AddCallback (guid, adCallback);
			FyberCallbacksManager.AddAd (guid, this);
		}

		protected virtual void AddExtraInfo(Dictionary<string, object> dict)
		{
			// nothing to do here
		}

	}
}
