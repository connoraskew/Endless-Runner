using AOT;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using UnityEngine;

namespace Tapdaq {
	public class AdManager {
		
		private static AdManager reference;

		public static AdManager instance {
			get {
				if (AdManager.reference == null) {
					AdManager.reference = new AdManager ();
				}
				return AdManager.reference;
			}
		}

		internal AdManager () {}

		private const string unsupportedPlatformMessage = "We support iOS and Android platforms only.";
		private const string tapdaqDefaultPlacementTag = "default";
		#if UNITY_IPHONE
		
		//================================= Interstitials ==================================================
		[DllImport ("__Internal")]
		private static extern void _ConfigureTapdaq(string appIdChar, string clientKeyChar, 
			string enabledAdTypesChar, string testDevicesChar, bool isDebugMode, bool autoReloadAds,
		                                            string pluginVersion, bool isConsentGiven, bool isAgeRestrictedUser);

		[DllImport ("__Internal")]
		private static extern bool _IsInitialised();

		[DllImport ("__Internal")]
		private static extern void _LaunchMediationDebugger();

		[DllImport ("__Internal")]
		private static extern void _SetConsentGiven(bool isConsentGiven);

		[DllImport ("__Internal")]
		private static extern bool _IsConsentGiven();

		[DllImport ("__Internal")]
		private static extern void _SetAgeRestrictedUser(bool isAgeRestrictedUser);
		
		[DllImport ("__Internal")]
		private static extern bool _IsAgeRestrictedUser();

		// interstitial
		[DllImport ("__Internal")]
		private static extern void _ShowInterstitialWithTag(string tag);

		[DllImport ("__Internal")]
		private static extern void _LoadInterstitialWithTag(string tag);

		[DllImport ("__Internal")]
		private static extern bool _IsInterstitialReadyWithTag(string tag);

		// banner
		[DllImport ("__Internal")]
		private static extern void _LoadBannerForSize(string sizeString);

		[DllImport ("__Internal")]
        private static extern void _ShowBanner(string position);

        [DllImport("__Internal")]
        private static extern void _HideBanner();

		[DllImport("__Internal")]
		private static extern bool _IsBannerReady();

		// video
		[DllImport ("__Internal")]
		private static extern void _ShowVideoWithTag (string tag);

		[DllImport("__Internal")]
		private static extern void _LoadVideoWithTag(string tag);

		[DllImport("__Internal")]
		private static extern bool _IsVideoReadyWithTag(string tag);


		// reward video
		[DllImport ("__Internal")]
		private static extern void _ShowRewardedVideoWithTag (string tag);

		[DllImport ("__Internal")]
		private static extern void _LoadRewardedVideoWithTag(string tag);

		[DllImport ("__Internal")]
		private static extern bool _IsRewardedVideoReadyWithTag(string tag);


		//================================== Natives =================================================

		[DllImport ("__Internal")]
		public static extern void _LoadNativeAdvertForPlacementTag(string tag, string nativeType);

		[DllImport ("__Internal")]
		private static extern System.IntPtr _GetNativeAdWithTag (string tag, string nativeAdType);

		[DllImport ("__Internal")]
		private static extern void _SendNativeClick(string uniqueId);

		[DllImport ("__Internal")]
		private static extern void _SendNativeImpression(string uniqueId);

		//////////  Show More Apps

		[DllImport ("__Internal")]
		private static extern void _ShowMoreApps();

		[DllImport ("__Internal")]
		private static extern bool _IsMoreAppsReady();

		[DllImport ("__Internal")]
		private static extern void _LoadMoreApps();

		[DllImport ("__Internal")]
		private static extern void _LoadMoreAppsWithConfig(string config);

		//////////  Show Offerwall

		[DllImport ("__Internal")]
		private static extern void _ShowOfferwall();

		[DllImport ("__Internal")]
		private static extern bool _IsOfferwallReady();

		[DllImport ("__Internal")]
		private static extern void _LoadOfferwall();

		/////////// Stats
		[DllImport ("__Internal")]
		private static extern void _SendIAP(string name, double price, string locale);

		/////////// Rewards
		[DllImport ("__Internal")]
		private static extern System.IntPtr _GetRewardId(string tag);

		#endif

		#region Class Variables

		private TDSettings settings;

		#endregion

		public static void Init () {
			instance._Init (false, false);
		}

		public static void InitWithConsent (bool isConsentGiven) {
			instance._Init (isConsentGiven, false);
		}

		public static void InitWithConsent (bool isConsentGiven, bool isAgeRestrictedUser) {
			instance._Init (isConsentGiven, isAgeRestrictedUser);
		}

		private void _Init (bool isConsentGiven, bool isAgeRestrictedUser) {
			if (!settings) {
				settings = TDSettings.getInstance();
			}

			TDEventHandler.instance.Init ();

			var applicationId = "";
			var clientKey = "";

			#if UNITY_IPHONE
			applicationId = settings.ios_applicationID;
			clientKey = settings.ios_clientKey;
			#elif UNITY_ANDROID
			applicationId = settings.android_applicationID;
			clientKey = settings.android_clientKey;
			#endif

			LogMessage(TDLogSeverity.debug, "TapdaqSDK/Application ID -- " + applicationId);
			LogMessage(TDLogSeverity.debug, "TapdaqSDK/Client Key -- " + clientKey);

			Initialize (applicationId, clientKey, isConsentGiven, isAgeRestrictedUser);
		}

		private void Initialize (string appID, string clientKey, bool isConsentGiven, bool isAgeRestrictedUser) {
			LogUnsupportedPlatform ();

			LogMessage (TDLogSeverity.debug, "TapdaqSDK/Initializing");
			var adTags = settings.tags.GetTagsJson();
			TDDebugLogger.Log ("tags:\n" + adTags);

			#if UNITY_IPHONE
			var testDevices = new TestDevicesList (settings.testDevices, TestDeviceType.iOS).ToString ();
			TDDebugLogger.Log ("testDevices:\n" + testDevices);
			CallIosMethod(() => _ConfigureTapdaq(appID, clientKey, adTags, testDevices, 
			                                     settings.isDebugMode, settings.autoReloadAds, TDSettings.pluginVersion, isConsentGiven, isAgeRestrictedUser));
			#elif UNITY_ANDROID
			var testDevices = new TestDevicesList (settings.testDevices, TestDeviceType.Android).ToString ();
			TDDebugLogger.Log ("testDevices:\n" + testDevices);
			CallAndroidStaticMethod("InitiateTapdaq", appID, clientKey, adTags, testDevices,
			                        settings.isDebugMode, settings.autoReloadAds, TDSettings.pluginVersion, isConsentGiven, isAgeRestrictedUser);
			#endif
		}

		#region Platform specific method calling

		#if UNITY_IPHONE 

		private static void CallIosMethod(Action action) {
			LogUnsupportedPlatform ();
			if(Application.platform == RuntimePlatform.IPhonePlayer) {
				if(AdManager.instance != null && action != null) {
					action.Invoke();
				}
			}
		}

		#elif UNITY_ANDROID

		private static T GetAndroidStatic<T>(string methodName, params object[] paramList) {
			LogUnsupportedPlatform();
			if(Application.platform == RuntimePlatform.Android) {
				try {
					using (AndroidJavaClass tapdaqUnity = new AndroidJavaClass("com.tapdaq.unityplugin.TapdaqUnity")) {
						return tapdaqUnity.CallStatic<T> (methodName, paramList);
					}
				} catch (Exception e) {
					TDDebugLogger.LogException (e);
				}
			}
			TDDebugLogger.LogError ("Error while call static method");
			return default(T);
		}
			
		private static void CallAndroidStaticMethod(string methodName, params object[] paramList) {
			CallAndroidStaticMethodFromClass ( "com.tapdaq.unityplugin.TapdaqUnity", methodName, true, paramList);
		}

		private static void CallAndroidStaticMethodFromClass(string className, 
			string methodName, bool logException, params object[] paramList) {
			LogUnsupportedPlatform();
			if(Application.platform == RuntimePlatform.Android) {
				try {
					using (AndroidJavaClass androidClass = new AndroidJavaClass(className)) {
						androidClass.CallStatic (methodName, paramList);
					}
				} catch (Exception e) {
					if (logException) {
						TDDebugLogger.Log ("CallAndroidStaticMethod:  " + methodName + "    FromClass: " 
							+ className + " failed. Message: " + e.Message);
					}
				}
			}
		}

		#endif
		#endregion

		private static void LogObsoleteWithTagMethod(string methodName) {
			TDDebugLogger.LogError("'" + methodName + "WithTag(string tag)' is Obsolete. Please, use '" + methodName +"(string tag)' instead");
		}

		private static void LogUnsupportedPlatform() {
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
				LogMessage (TDLogSeverity.warning, unsupportedPlatformMessage);
			}
		}

		public void _UnexpectedErrorHandler (string msg) {
			TDDebugLogger.Log (":: Ad test ::" + msg);
			LogMessage (TDLogSeverity.error, msg);
		}

		public static void LogMessage (TDLogSeverity severity, string message) {
			string prefix = "Tapdaq Unity SDK: ";
			if (severity == TDLogSeverity.warning) {
				TDDebugLogger.LogWarning (prefix + message);
			} else if (severity == TDLogSeverity.error) {
				TDDebugLogger.LogError (prefix + message);
			} else {
				TDDebugLogger.Log (prefix + message);
			}
		}

		public void FetchFailed (string msg) {
			TDDebugLogger.Log (msg);
			LogMessage (TDLogSeverity.debug, "unable to fetch more ads");
		}

		public static void OnApplicationPause(bool isPaused) {
			#if UNITY_IPHONE
			#elif UNITY_ANDROID
			if (isPaused) {
				CallAndroidStaticMethod("OnPause");
			} else {
				CallAndroidStaticMethod("OnResume");
			}
			#endif

		}

		public static bool IsInitialised() {
			bool ready = false;
			#if UNITY_IPHONE
			CallIosMethod(() => ready = _IsInitialised());
			#elif UNITY_ANDROID
			ready = GetAndroidStatic<bool>("IsInitialised");
			#endif
			return ready;
		}

		public static void LaunchMediationDebugger () {
			#if UNITY_IPHONE
			_LaunchMediationDebugger ();
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("ShowMediationDebugger");
			#endif
		}

		public static void SetConsentGiven (bool isConsentGiven) {
			#if UNITY_IPHONE
			CallIosMethod(() => _SetConsentGiven(isConsentGiven));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("SetConsentGiven", isConsentGiven);
			#endif
		}

		public static bool IsConsentGiven() {
			bool result = false;
			#if UNITY_IPHONE
			CallIosMethod(() => result = _IsConsentGiven());
			#elif UNITY_ANDROID
			result = GetAndroidStatic<bool>("IsConsentGiven");
			#endif
			return result;
		}

		public static void SetIsAgeRestrictedUser (bool isAgeRestrictedUser) {
			#if UNITY_IPHONE
			CallIosMethod(() => _SetAgeRestrictedUser(isAgeRestrictedUser));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("SetAgeRestrictedUser", isAgeRestrictedUser);
			#endif
		}
		
		public static bool IsAgeRestrictedUser() {
			bool result = false;
			#if UNITY_IPHONE
			CallIosMethod(() => result = _IsAgeRestrictedUser());
			#elif UNITY_ANDROID
			result = GetAndroidStatic<bool>("IsAgeRestrictedUser");
			#endif
			return result;
		}

		// More Apps

		public static void ShowMoreApps () {
			#if UNITY_IPHONE
			CallIosMethod(_ShowMoreApps);
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("ShowMoreApps");
			#endif
		}

		public static bool IsMoreAppsReady () {
			bool ready = false;
			#if UNITY_IPHONE
			CallIosMethod(() => ready = _IsMoreAppsReady());
			#elif UNITY_ANDROID
			ready = GetAndroidStatic<bool>("IsMoreAppsReady");
			#endif
			return ready;
		}

		public static void LoadMoreApps () {
			#if UNITY_IPHONE
			CallIosMethod(_LoadMoreApps);
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("LoadMoreApps", "{}");
			#endif
		}

		public static void LoadMoreAppsWithConfig (TDMoreAppsConfig config) {

			#if UNITY_IPHONE
			CallIosMethod(() => _LoadMoreAppsWithConfig(config.ToString()));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("LoadMoreApps", config.ToString());
			#endif
		}

		// interstitial

		[Obsolete ("Please, use 'ShowInterstitial (string tag)' method.")]
		public static void ShowInterstitial () {
			ShowInterstitial (tapdaqDefaultPlacementTag);
		}

		public static void ShowInterstitial (string tag) {
			#if UNITY_IPHONE
			CallIosMethod(() => _ShowInterstitialWithTag(tag));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("ShowInterstitialWithTag", tag);
			#endif
		}

		[Obsolete ("Please, use 'LoadInterstitial (string tag)' method.")]
		public static void LoadInterstitial() {
			LoadInterstitial (tapdaqDefaultPlacementTag);
		}

		public static void LoadInterstitial(string tag) {
			#if UNITY_IPHONE
			CallIosMethod(() => _LoadInterstitialWithTag(tag));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("LoadInterstitialWithTag", tag);
			#endif
		}

		[Obsolete ("Please, use 'LoadInterstitial(string tag)' method.")]
		public static void LoadInterstitialWithTag(string tag) {
			LogObsoleteWithTagMethod("LoadInterstitial");
			LoadInterstitial (tag);
		}

		[Obsolete ("Please, use 'IsInterstitialReady (string tag)' method.")]
		public static bool IsInterstitialReady() {
			return IsInterstitialReady(tapdaqDefaultPlacementTag);
		}

		public static bool IsInterstitialReady(string tag) {
			bool ready = false;
			#if UNITY_IPHONE
			CallIosMethod(() => ready = _IsInterstitialReadyWithTag(tag));
			#elif UNITY_ANDROID
			ready = GetAndroidStatic<bool>("IsInterstitialReady", tag);
			#endif
			return ready;
		}

		[Obsolete ("Please, use 'IsInterstitialReady(string tag)' method.")]
		public static bool IsInterstitialReadyWithTag(string tag) {
			LogObsoleteWithTagMethod("IsInterstitialReady");
			return IsInterstitialReady(tag);
		}
			
		// banner

		public static bool IsBannerReady() {
			bool ready = false;
			#if UNITY_IPHONE
			CallIosMethod(() => ready = _IsBannerReady());
			#elif UNITY_ANDROID
			ready = GetAndroidStatic<bool>("IsBannerReady");
			#endif
			return ready;
		}

		public static void RequestBanner (TDMBannerSize size) {
			#if UNITY_IPHONE
			CallIosMethod(() => _LoadBannerForSize(size.ToString()));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("LoadBannerOfType", size.ToString());
			#endif
		}

		public static void ShowBanner (TDBannerPosition position) {
			#if UNITY_IPHONE
			CallIosMethod(() => _ShowBanner(position.ToString()));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("ShowBanner", position.ToString());
			#endif
		}

	    public static void HideBanner()
	    {
			#if UNITY_IPHONE
			CallIosMethod(_HideBanner);
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("HideBanner");
			#endif
	    }


		// video

		[Obsolete ("Please, use 'ShowVideo (string tag)' method.")]
		public static void ShowVideo () {
			ShowVideo (tapdaqDefaultPlacementTag);
		}

		public static void ShowVideo (string tag) {
			#if UNITY_IPHONE
			CallIosMethod(() => _ShowVideoWithTag (tag));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("ShowVideoWithTag", tag);
			#endif
		}

		[Obsolete ("Please, use 'LoadVideo (string tag)' method.")]
		public static void LoadVideo() {
			LoadVideo (tapdaqDefaultPlacementTag);
		}

		public static void LoadVideo(string tag) {
			#if UNITY_IPHONE
			CallIosMethod(() => _LoadVideoWithTag (tag));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("LoadVideoWithTag", tag);
			#endif
		}

		[Obsolete ("Please, use 'LoadVideo(string tag)' method.")]
		public static void LoadVideoWithTag(string tag) {
			LogObsoleteWithTagMethod("LoadVideo");
			LoadVideo (tag);
		}

		[Obsolete ("Please, use 'IsVideoReady (string tag)' method.")]
		public static bool IsVideoReady() {
			return IsVideoReady (tapdaqDefaultPlacementTag);
		}

		public static bool IsVideoReady(string tag) {
			bool ready = false;
			#if UNITY_IPHONE
			CallIosMethod(() => ready = _IsVideoReadyWithTag(tag));
			#elif UNITY_ANDROID
			ready = GetAndroidStatic<bool>("IsVideoReady", tag);
			#endif
			return ready;
		}

		[Obsolete ("Please, use 'IsVideoReady(string tag)' method.")]
		public static bool IsVideoReadyWithTag(string tag) {
			LogObsoleteWithTagMethod("IsVideoReady");
			return IsVideoReady(tag);
		}

		// rewarded video

		[Obsolete ("Please, use 'ShowRewardVideo (string tag)' method.")]
		public static void ShowRewardVideo () {
			ShowRewardVideo (tapdaqDefaultPlacementTag);
		}

		public static void ShowRewardVideo (string tag) {
			#if UNITY_IPHONE
			CallIosMethod(() => _ShowRewardedVideoWithTag (tag));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("ShowRewardAdWithTag", tag);
			#endif
		}

		[Obsolete ("Please, use 'LoadRewardedVideo (string tag)' method.")]
		public static void LoadRewardedVideo() {
			LoadRewardedVideo (tapdaqDefaultPlacementTag);
		}

		public static void LoadRewardedVideo(string tag) {
			#if UNITY_IPHONE
			CallIosMethod(() => _LoadRewardedVideoWithTag (tag));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("LoadRewardAdWithTag", tag);
			#endif
		}

		[Obsolete ("Please, use 'LoadRewardedVideo(string tag)' method.")]
		public static void LoadRewardedVideoWithTag(string tag) {
			LogObsoleteWithTagMethod("LoadRewardedVideo");
			LoadRewardedVideo (tag);
		}

		[Obsolete ("Please, use 'IsRewardedVideoReady (string tag)' method.")]
		public static bool IsRewardedVideoReady() {
			return IsRewardedVideoReady(tapdaqDefaultPlacementTag);
		}

		public static bool IsRewardedVideoReady(string tag) {
			bool ready = false;
			#if UNITY_IPHONE
			CallIosMethod(() => ready = _IsRewardedVideoReadyWithTag(tag));
			#elif UNITY_ANDROID
			ready = GetAndroidStatic<bool>("IsRewardAdReady", tag);
			#endif
			return ready;
		}

		[Obsolete ("Please, use 'IsRewardedVideoReady(string tag)' method.")]
		public static bool IsRewardedVideoReadyWithTag(string tag) {
			LogObsoleteWithTagMethod("IsRewardedVideoReady");
			return IsRewardedVideoReady(tag);
		}

		public static bool IsOfferwallReady() {
			bool ready = false;
			#if UNITY_IPHONE
			CallIosMethod(() => ready = _IsOfferwallReady());
			#elif UNITY_ANDROID
			ready = GetAndroidStatic<bool>("IsOfferwallReady");
			#endif
			return ready;
		}

		public static void ShowOfferwall() {
			#if UNITY_IPHONE
			CallIosMethod(_ShowOfferwall);
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("ShowOfferwall");
			#endif
		}

		public static void LoadOfferwall() {
			#if UNITY_IPHONE
			CallIosMethod(_LoadOfferwall);
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("LoadOfferwall");
			#endif
		}

		// native ad

		[Obsolete ("Please, use 'GetNativeAd(TDNativeAdType adType, string tag)' method.")]
		public static TDNativeAd GetNativeAd (TDNativeAdType adType) {
			return GetNativeAd(adType, tapdaqDefaultPlacementTag);
		}

		public static TDNativeAd GetNativeAd (TDNativeAdType adType, string tag) {

			var nativeAdJson = "{}";

			#if UNITY_IPHONE
			nativeAdJson = Marshal.PtrToStringAnsi(_GetNativeAdWithTag(tag, adType.ToString()));
			#elif UNITY_ANDROID
			nativeAdJson = GetAndroidStatic<string>("GetNativeAdWithTag", adType.ToString (), tag);
			#else
			return null;
			#endif

			return TDNativeAd.CreateNativeAd (nativeAdJson);
		}

		public static void LoadNativeAdvertForTag(string tag, TDNativeAdType nativeType) {
			#if UNITY_IPHONE
			_LoadNativeAdvertForPlacementTag (tag, nativeType.ToString());
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("LoadNativeAd", nativeType.ToString(), tag);
			#endif
		}

		[Obsolete ("Please, use 'LoadNativeAdvertForAdType(string tag, TDNativeAdType adType)' method.")]
		public static void LoadNativeAdvertForAdType(TDNativeAdType nativeType) {
			LoadNativeAdvertForTag (tapdaqDefaultPlacementTag, nativeType);
		}

		public static bool IsNativeAdReady(TDNativeAdType adType) {
			bool ready = false;
			#if UNITY_ANDROID
			ready = GetAndroidStatic<bool>("IsNativeAdReady", adType.ToString());
			#endif
			return ready;
		}

		public static bool IsNativeAdReady(TDNativeAdType adType, string tag) {
			bool ready = false;
			#if UNITY_ANDROID
			ready = GetAndroidStatic<bool>("IsNativeAdReady", adType.ToString(), tag);
			#endif
			return ready;
		}

		public static void SendNativeImpression (TDNativeAd ad) {
			#if UNITY_IPHONE
			CallIosMethod(() => _SendNativeImpression(ad.uniqueId)); // todo change to Id
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("SendNativeImpression", ad.uniqueId); // todo change to Id
			#endif
		}

		public static void SendNativeClick (TDNativeAd ad) {
			#if UNITY_IPHONE
			CallIosMethod(() => _SendNativeClick(ad.uniqueId)); // todo change to Id
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("SendNativeClick", ad.uniqueId); // todo change to Id
			#endif
		}

		public static void SendIAP (String name, double price, String locale) {
			#if UNITY_IPHONE
			CallIosMethod(() => _SendIAP(name, price, locale));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("SendIAP", name, price, locale);
			#endif
		}

		public static String GetRewardId (String tag) {
			#if UNITY_IPHONE
			return Marshal.PtrToStringAnsi(_GetRewardId(tag));
			#elif UNITY_ANDROID
			return GetAndroidStatic<string>("GetRewardId", tag);
			#endif

			return null;
		}
	}
}