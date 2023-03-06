using UnityEngine;

namespace FyberPlugin
{

#if UNITY_ANDROID && !UNITY_EDITOR
	internal class PluginBridgeComponent : IPluginBridge
	{

		static PluginBridgeComponent()
		{
			FyberGameObject.Init();
		}

		public void StartSDK(string json)
		{

			using (AndroidJavaObject plugin = new AndroidJavaObject("com.fyber.unity.FyberPlugin"))
			{
				plugin.CallStatic("setPluginParameters", Fyber.Version, Application.unityVersion);
				plugin.CallStatic("start", json);
			}
			
		}

		public void Request(string json)
		{
			using (AndroidJavaObject requesterWrapper = new AndroidJavaObject("com.fyber.unity.requesters.RequesterWrapper"))
			{
				requesterWrapper.CallStatic("request", json);
			}
		}

		public void StartAd(string json)
		{
			using (AndroidJavaObject adWrapper = new AndroidJavaObject("com.fyber.unity.ads.AdWrapper"))
			{
				adWrapper.CallStatic("start", json);
			}
		}

		public bool Banner(string json)
		{
			using (AndroidJavaObject adWrapper = new AndroidJavaObject("com.fyber.unity.ads.AdWrapper"))
			{
				return adWrapper.CallStatic<bool>("performAdActions", json);
			}
		}

		public void Report(string json)
		{
			using (AndroidJavaObject reportWrapper = new AndroidJavaObject("com.fyber.unity.reporters.ReporterWrapper"))
			{
				reportWrapper.CallStatic("report", json);
			}
		}

		public string Settings(string json)
		{
			using (AndroidJavaObject settingsWrapper = new AndroidJavaObject("com.fyber.unity.settings.SettingsWrapper"))
			{
				return settingsWrapper.CallStatic<string>("perform", json);
			}
		}

		public void EnableLogging(bool shouldLog)
		{
			//com.fyber.utils.FyberLogger.enableLogging
			using (AndroidJavaObject logger = new AndroidJavaObject("com.fyber.utils.FyberLogger"))
			{
				logger.CallStatic<bool>("enableLogging", shouldLog);
			}
		}

		public void GameObjectStarted()
		{
			using (AndroidJavaClass androidNativeMessageJavaClass = new AndroidJavaClass("com.fyber.unity.helpers.NativeMessage"))
 			{
 				androidNativeMessageJavaClass.CallStatic("resendFailedMessages");
 			}
		}

		public void ApplicationQuit()
		{
			//DO NOTHING
		}

		public void ShowTestSuite()
		{
			using (AndroidJavaClass androidNativeMessageJavaClass = new AndroidJavaClass("com.fyber.unity.utils.testsuite.IntegrationAnalyzerWrapper"))
			{
				androidNativeMessageJavaClass.CallStatic("showTestSuite");
			}
		}

		public void AnalyzeTestSuite()
		{
			using (AndroidJavaClass androidNativeMessageJavaClass = new AndroidJavaClass("com.fyber.unity.utils.testsuite.IntegrationAnalyzerWrapper"))
			{
				androidNativeMessageJavaClass.CallStatic("analyzeTestSuite");
			}
		}
	}
#endif

}
