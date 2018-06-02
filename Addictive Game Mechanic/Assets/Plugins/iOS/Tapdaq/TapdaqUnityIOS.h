//
//  TapdaqUnityIOS.h
//  TapdaqUnity
//
//  Created by Rheo Violenes on 05/05/15.
//  Copyright (c) 2015 Nerd. All rights reserved.
//
#import <Foundation/Foundation.h>
#import <Tapdaq/Tapdaq.h>
#import "TapdaqNativeAd.h"

void _LaunchMediationDebugger();

void _ConfigureTapdaq(const char* appIdChar,
                      const char* clientKeyChar,
                      const char* enabledAdTypesChar,
                      const char* testDevices,
                      bool isDebugMode,
                      bool autoReloadAds,
                      const char* pluginVersion,
                      bool isConsentGiven,
                      bool isAgeRestrictedUser);

bool _IsInitialised();

void _SetConsentGiven(bool isConsentGiven);
bool _IsConsentGiven();

void _SetAgeRestrictedUser(bool isAgeRestrictedUser);
bool _IsAgeRestrictedUser();

// banner

/**
 * Loads a banner.
 * @param size A string. Must be one of following values: TDMBannerStandard, TDMBannerLarge, TDMBannerMedium, TDMBannerFull, TDMBannerLeaderboard, TDMBannerSmartPortrait, TDMBannerSmartLandscape
 */
void _LoadBannerForSize(const char* sizeChar);

bool _IsBannerReady();

void _ShowBanner(const char* position);

void _HideBanner();

// interstitial

void _LoadInterstitialWithTag(const char *tagChar);

bool _IsInterstitialReadyWithTag(const char *tagChar);

void _ShowInterstitialWithTag(const char* tagChar);

// video

void _LoadVideoWithTag(const char *tagChar);

bool _IsVideoReadyWithTag(const char *tagChar);

void _ShowVideoWithTag(const char* tagChar);

// reward video

void _LoadRewardedVideoWithTag(const char *tagChar);

bool _IsRewardedVideoReadyWithTag(const char *tagChar);

void _ShowRewardedVideoWithTag(const char* tagChar);

// native ad

void _LoadNativeAdvertForPlacementTag(const char* tag, const char* nativeAdType);

void _LoadNativeAdvertForAdType(const char* nativeAdType);

void _FetchNative(const char* adTypeInt);

void _FetchNativeAdWithTag (const char *tag, const char* nativeAdType);

void _SendNativeClick(const char *uniqueId);

void _SendNativeImpression(const char *uniqueId);

void _LaunchMediationDebugger();

bool _isEmpty(const char *str);


// more apps

void _ShowMoreApps();

bool _IsMoreAppsReady();

void _LoadMoreApps();

void _LoadMoreAppsWithConfig(const char* config);


// offerwall

void _ShowOfferwall();

bool _IsOfferwallReady();

void _LoadOfferwall();

// stats

void _SendIAP(const char* name, double price, const char* locale);

// rewards

const char *_GetRewardId(const char* tag);

@interface TapdaqUnityIOS : NSObject
    
+ (instancetype)sharedInstance;
    
- (void)initWithApplicationId:(NSString *)appID
                    clientKey:(NSString *)clientKey
               enabledAdTypes:(NSString *)enabledAdTypes
                  testDevices:(NSString *)testDevices
                  isDebugMode:(bool)isDebugMode
                autoReloadAds:(bool)autoReloadAds
                pluginVersion:(NSString *)pluginVersion
               isConsentGiven:(bool)isConsentGiven
          isAgeRestrictedUser:(bool)isAgeRestrictedUser;
    
-(BOOL) IsInitialised;
    
- (void) setConsentGiven:(BOOL)isConsentGiven;
    
- (BOOL) isConsentGiven;
    
- (void) SetAgeRestrictedUser:(BOOL)isAgeRestrictedUser;
    
- (BOOL) isAgeRestrictedUser;
    @end
