using System;
using System.Collections.Generic;

using FyberPlugin.LitJson;

namespace FyberPlugin
{
	public class Fyber
	{

		public const string Version = "9.2.6";

		private static Fyber fyber;
		private bool initialized { get; set; }
		private Settings settings { get; set; }

		private Dictionary<string, string> startParams;
		private Dictionary<string, Object> startOptions;

		private Fyber()
		{
			startOptions = new Dictionary<string, Object>(4);
			settings = new Settings();
		}

		public static Fyber With(string appId)
		{
			if (string.IsNullOrEmpty(appId))
				throw new ArgumentException("App ID cannot be null nor empty");

			if (fyber == null)
				fyber = new Fyber();

			if (!fyber.initialized)
				fyber.startOptions["appId"] = appId;

			return fyber;
		}

		public Fyber WithUserId(string userId)
		{
			if (!initialized)
				startOptions["userId"] = userId;
			return this;
		}

		public Fyber WithManualPrecaching()
		{
			if (!initialized)
			{
				startOptions["startVideoPrecaching"] = false;
			}
			return this;
		}

		public Fyber WithSecurityToken(string securityToken)
		{
			if (!initialized)
				startOptions["securityToken"] = securityToken;
			return this;
		}

		public Fyber WithParameters(Dictionary<string, string> parameters)
		{
			if (!initialized)
				startParams = new Dictionary<string, string>(parameters);
			return this;
		}

        public Fyber WithoutAdId()
        {
            if (!initialized)
                startOptions["adIdDisabled"] = true;
            return this;
        }

		public Settings Start()
		{
			if (!initialized)
			{
				if (startParams != null)
					startOptions["parameters"] = startParams;
				string json = JsonMapper.ToJson(startOptions);
				PluginBridge.Start(json);
			}

			return settings;
		}
	}

	public class Settings
	{

		private Dictionary<string, object> auxDict;

		internal Settings()
		{
			auxDict = new Dictionary<string, object>(2);
		}

		public Settings NotifyUserOnCompletion(bool shouldNotifyUserOnCompletion)
		{
			RunInBridge("notifyUserOnCompletion", shouldNotifyUserOnCompletion);
			return this;
		}

		public Settings NotifyUserOnReward(bool shouldNotifyUserOnReward)
		{
			RunInBridge("notifyUserOnReward", shouldNotifyUserOnReward);
			return this;
		}

		public Settings CloseOfferWallOnRedirect(bool shouldCloseOfferWallOnRedirect)
		{
			RunInBridge("closeOfferWallOnRedirect", shouldCloseOfferWallOnRedirect);
			return this;
		}

		public Settings AddParameters(Dictionary<string, string> parameters)
		{
			RunInBridge("addParameters", JsonMapper.ToJson(parameters));
			return this;
		}

		public Settings AddParameter(string key, string value)
		{
			RunInBridge("addParameter", string.Format("{\"{0}\":\"{1}\"}", key, value));
			return this;
		}

		public Settings ClearParameters()
		{
			RunInBridge("clearParameters", string.Empty);
			return this;
		}

		public Settings RemoveParameter(string key)
		{
			RunInBridge("removeParameter", key);
			return this;
		}

		public Settings UpdateUserId(string userId)
		{
			RunInBridge("updateUserId", userId);
			return this;
		}

		public String GetUserId()
		{
			// TODO: we should convert to string here
			return RunInBridge("getUserId", null);
		}

		private String RunInBridge(string action, object value)
		{
			auxDict["action"] = action;
			auxDict["value"] = value;
			return PluginBridge.Settings(JsonMapper.ToJson(auxDict));
		}
	}

}
