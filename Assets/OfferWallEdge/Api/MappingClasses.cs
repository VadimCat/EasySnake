using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FyberPlugin.LitJson;

namespace FyberPlugin
{
	 	
#if !UNITY_WP8
   [System.Reflection.Obfuscation(Exclude = true)]
#endif	
    internal class NativeError
    {
        public string Error { get; set; }

        public string Id { get; set; }
    }

#if !UNITY_WP8
   [System.Reflection.Obfuscation(Exclude = true)]
#endif	
    internal class NativeMessage
    {
        // one of those 2
        // - 0 -> request
        // - 1 -> ad
        public int Type { get; set; }

        public string Id { get; set; }

        // for request (same except currency for ads)
        // offerwall, interstitials, videos, currency 
        public int Origin { get; set; }
        public RequestPayload RequestPayload { get; set; }
		public AdPayload AdPayload { get; set; }		
    }
    
#if !UNITY_WP8
   [System.Reflection.Obfuscation(Exclude = true)]
#endif	
    internal class RequestPayload
    {
        public int? RequestError { get; set; }
        public string PlacementId { get; set; }
        public bool? AdAvailable { get; set; }
        public VirtualCurrencyResponse CurrencyResponse { get; set; }
        public VirtualCurrencyErrorResponse CurrencyErrorResponse { get; set; }
    }
    
#if !UNITY_WP8
   [System.Reflection.Obfuscation(Exclude = true)]
#endif	
    internal class AdPayload
    {
        public bool AdStarted { get; set; }
        public string Error { get; set; }
        public string Status { get; set; }
    }

#if !UNITY_WP8
	[System.Reflection.Obfuscation(Exclude = true)]
#endif
	internal class BannerAdPayload
	{
		public int? Event { get; set; }
		public string ErrorMessage { get; set; }
	}
	
}
