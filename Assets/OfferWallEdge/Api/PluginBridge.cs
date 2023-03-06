using System;
using System.Reflection;
using UnityEngine;

namespace FyberPlugin
{

	public sealed class PluginBridge
	{
		static PluginBridge()
		{
			Resources.Load("FyberSettings");
		}

		public static IPluginBridge bridge;

		internal static void Start(string json)
		{
			if (bridge != null)
				bridge.StartSDK(json);
		}

		internal static void Request(string json)
		{
			if (bridge != null)
				bridge.Request(json);
		}

		internal static void StartAd(string json)
		{
			if (bridge != null)
				bridge.StartAd(json);
		}

		internal static String Settings(string json)
		{
			if (bridge != null)
				return bridge.Settings(json);
			return null;
		}

		internal static void EnableLogging(bool shouldLog)
		{
			if (bridge != null)
				bridge.EnableLogging(shouldLog);
		}

		internal static void GameObjectStarted()
		{
			if (bridge != null)
				bridge.GameObjectStarted();
		}

		internal static void ApplicationQuit()
		{
			if (bridge != null)
				bridge.ApplicationQuit();
		}

	}

	public interface IPluginBridge
	{
		void StartSDK(string json);

		void Request(string json);

		void StartAd(string json);

		String Settings(string json);

		void EnableLogging(bool shouldLog);

		void GameObjectStarted();

        void ApplicationQuit();

	}
}
