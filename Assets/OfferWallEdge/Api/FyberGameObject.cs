using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FyberPlugin.LitJson;

namespace FyberPlugin
{

	public class FyberGameObject : MonoBehaviour
    {

        private const string ObjectName = "FyberGameObject";

        public static void Init()
        {
			// This needs to be added here in order to make the static 
			// constructor to be run
        }

        static FyberGameObject()
        {
            var type = typeof(FyberGameObject);
            try
            {
                //  let's check if we're already added in the scene
                var obj = FindObjectOfType(type) as MonoBehaviour;
                if (obj != null)
                {
                    return;
                }
                // let's create a new game object here
                var delegateGameObject = new GameObject(ObjectName);
                delegateGameObject.AddComponent(type);
				DontDestroyOnLoad(delegateGameObject);
            }
            catch (UnityException)
            {
                Debug.LogWarning("It looks like you have the " + type + " on a GameObject in your scene. Please remove the script from your scene.");
            }
        }

		void OnEnable()
		{
			PluginBridge.GameObjectStarted();
		}

		void OnApplicationFocus(bool focusStatus) {
			if (focusStatus)
				PluginBridge.GameObjectStarted();
		}

        void OnNativeMessageReceived(string json)
        {
            NativeMessage message = JsonMapper.ToObject<NativeMessage>(json);
            
            if (string.IsNullOrEmpty(message.Id))
                FyberCallback.Instance.OnNativeError("An unknown error occurred while processing the ads. Please request again.");
            else
                FyberCallbacksManager.Instance.Process(message);
        }
        
		void OnPrecachingFinished(string cachedVideosAvailable)
		{
			if (!string.IsNullOrEmpty (cachedVideosAvailable)) 
				FyberCallback.Instance.OnVideoCached(Boolean.Parse(cachedVideosAvailable));

		}

        void OnNativeErrorOccurred(string json)
        {
            NativeError message = JsonMapper.ToObject<NativeError>(json);
            
            FyberCallback.Instance.OnNativeError(message.Error);
            if (!string.IsNullOrEmpty(message.Id))
                FyberCallbacksManager.Instance.ClearCallbacks(message.Id);
        }


		void OnApplicationQuit() 
		{
			PluginBridge.ApplicationQuit ();
		}

        /**
		* Executes the supplied Action after skipping one frame.
		**/
        internal IEnumerator SkipFrameCoroutineWithBlock(Action<int> block)
        {
            // returning here skips one frame and the rest of the code will be executed on the next one
            yield return null;
            block(0);
        }

		public void SkipFrameWithBlock(Action<int> block) 
		{
			StartCoroutine (SkipFrameCoroutineWithBlock (block));
		}

    }

}

