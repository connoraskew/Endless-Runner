//
//  Tapdaq.h
//  Tapdaq
//
//  Created by Tapdaq <support@tapdaq.com>
//  Copyright (c) 2016 Tapdaq. All rights reserved.
//

#import <UIKit/UIKit.h>

#import <Tapdaq/TDOrientationEnum.h>
#import <Tapdaq/TDNativeAdUnitEnum.h>
#import <Tapdaq/TDNativeAdSizeEnum.h>

#import <Tapdaq/TDAdTypeEnum.h>
#import <Tapdaq/TDNativeAdTypeEnum.h>
#import <Tapdaq/TDMNetworkEnum.h>
#import <Tapdaq/TDMBannerSizeEnum.h>
#import <Tapdaq/TDReward.h>
#import <Tapdaq/TDPlacement.h>
#import <Tapdaq/TDMediatedNativeAd.h>

@protocol TapdaqDelegate;
@protocol TDAdRequestDelegate;

@class TDAdvert;
@class TDNativeAdvert;
@class TDInterstitialAdvert;
@class TDProperties;
@class TDPlacement;
@class TDMoreAppsConfig;
@class TDError;

@interface Tapdaq : NSObject

@property (readonly, nonatomic) BOOL isConfigLoaded __attribute__((deprecated("isConfigLoaded has been deprecated. Please use -isInitialised instead. This property will be removed in future releases.")));

/**
 * Current version of the SDK
 */
@property (readonly, nonatomic) NSString *sdkVersion;

@property (nonatomic, weak) id <TapdaqDelegate> delegate;

@property (nonatomic) BOOL isConsentGiven;
@property (nonatomic) BOOL isAgeRestrictedUser;

#pragma mark Singleton
/**
 The singleton Tapdaq object, use this for all method calls
 
 @return The Tapdaq singleton.
 */
+ (instancetype)sharedSession;

- (void)trackPurchaseForProductName:(NSString *)productName price:(double)price priceLocale:(NSString *)priceLocale;

#pragma mark Initializing Tapdaq

/**
 A setter for the Application ID of your app, and the Client Key associated with your Tapdaq account. 
 You can obtain these details when you sign up and add your app to https://tapdaq.com
 You must use this in the application:didFinishLaunchingWithOptions method.
 
 @param applicationId The application ID tied to your app.
 @param clientKey The client key tied to your app.
 @param properties The properties object that overrides the Tapdaq defaults. See TDProperties for info on all configuration options.
 */
- (void)setApplicationId:(NSString *)applicationId
               clientKey:(NSString *)clientKey
              properties:(TDProperties *)properties;

/**
 * Get current SDK initialisation status
 *
 * @return YES if the SDK is initialised. NO otherwise.
 */
- (BOOL)isInitialised;

#pragma mark Reward
- (NSString *)rewardIdForPlacementTag:(NSString *)placementTag;

#pragma mark Debugger
- (void)presentDebugViewController;

#pragma mark Banner

- (void)loadBanner:(TDMBannerSize)size;

- (BOOL)isBannerReady;

- (UIView *)getBanner;

- (void)loadBannerWithSize:(TDMBannerSize)size completion:(void(^)(UIView *))completion;

#pragma mark Interstitial
- (void)loadInterstitialForPlacementTag:(NSString *)placementTag delegate:(id<TDAdRequestDelegate>)delegate;

- (void)loadInterstitialForPlacementTag:(NSString *)placementTag;

- (BOOL)isInterstitialReadyForPlacementTag:(NSString *)placementTag;

- (void)showInterstitialForPlacementTag:(NSString *)placementTag;


#pragma mark Video
- (void)loadVideoForPlacementTag:(NSString *)placementTag delegate:(id<TDAdRequestDelegate>)delegate;

- (void)loadVideoForPlacementTag:(NSString *)placementTag;

- (BOOL)isVideoReadyForPlacementTag:(NSString *)placementTag;

- (void)showVideoForPlacementTag:(NSString *)placementTag;


#pragma mark Rewarded Video
- (void)loadRewardedVideoForPlacementTag:(NSString *)placementTag delegate:(id<TDAdRequestDelegate>)delegate;

- (void)loadRewardedVideoForPlacementTag:(NSString *)placementTag;

- (BOOL)isRewardedVideoReadyForPlacementTag:(NSString *)placementTag;

- (void)showRewardedVideoForPlacementTag:(NSString *)placementTag;

- (void)showRewardedVideoForPlacementTag:(NSString *)placementTag hashedUserId:(NSString *)hashedUserId;

    
#pragma mark Offerwall

- (void)loadOfferwallWithDelegate:(id<TDAdRequestDelegate>)delegate;

- (void)loadOfferwall;
    
- (BOOL)isOfferwallReady;
    
- (void)showOfferwall;

#pragma mark Mediated native ads

- (void)loadNativeAdInViewController:(UIViewController *)viewController
                        placementTag:(NSString *)placementTag
                             options:(TDMediatedNativeAdOptions)options
                            delegate:(id<TDAdRequestDelegate>)delegate;

#pragma mark Native adverts
/**
 Loads a native advert for a particular placement tag, this will fetch the native advert's creative, and call either -didLoadNativeAdvert:forPlacementTag:adType: if the advert is successfully loaded, or -didFailToLoadNativeAdvertForPlacementTag:adType: if it fails to load.
 We recommend you implement both delegate methods to handle the advert accordingly.
 
 @param tag The placement tag of the advert to be loaded.
 @param nativeAdType The native ad type of the advert to be loaded.
 @param delegate Delegate which handles result of ad's loading.
 */
- (void)loadNativeAdvertForPlacementTag:(NSString *)tag adType:(TDNativeAdType)nativeAdType delegate:(id<TDAdRequestDelegate>)delegate;

/**
 Loads a native advert for a particular placement tag, this will fetch the native advert's creative, and call either -didLoadNativeAdvert:forPlacementTag:adType: if the advert is successfully loaded, or -didFailToLoadNativeAdvertForPlacementTag:adType: if it fails to load.
 We recommend you implement both delegate methods to handle the advert accordingly.
 
 @param placementTag The placement tag of the advert to be loaded.
 @param nativeAdType The native ad type of the advert to be loaded.
 */
- (void)loadNativeAdvertForPlacementTag:(NSString *)placementTag adType:(TDNativeAdType)nativeAdType __attribute__((deprecated("loadNativeAdvertForPlacementTag:adType: has been deprecated. Please use loadNativeAdvertForPlacementTag:adType:delegate: instead. This method will be removed in future releases.")));

/**
 Fetches a TDNativeAdvert for a particular placement tag.
 You must register the tag in TDProperties otherwise adverts will not display.
 
 @param placementTag The placement tag
 @param nativeAdType The native advert type to be fetched.
 */
- (TDNativeAdvert *)getNativeAdvertForPlacementTag:(NSString *)placementTag adType:(TDNativeAdType)nativeAdType;

/**
 This method must be called when the advert is displayed to the user. You do not need to call this method when using -showInterstitial. 
 This should only be used when either a TDInterstitialAdvert or TDNativeAdvert has been fetched.
 
 @param advert The TDAdvert that has been displayed to the user, this can be a TDInterstitialAdvert or TDNativeAdvert.
 */
- (void)triggerImpression:(TDAdvert *)advert __attribute__((deprecated("triggerImpression: has been deprecated. Please use [advert triggerImpression] instead. This method will be removed in future releases.")));

/**
 This method must be called when a user taps on the advert, you do not need to call this method when using -showInterstitial. 
 This should only be used when either TDInterstitialAdvert or TDNativeAdvert has been fetched.
 Unlike -triggerImpression:, this method will also direct users to the the App Store, or to a custom URL, depending on the adverts configuration.
 
 @param advert The TDAdvert that has been displayed to the user, this can be a TDInterstitialAdvert or TDNativeAdvert.
 */
- (void)triggerClick:(TDAdvert *)advert __attribute__((deprecated("triggerClick: has been deprecated. Please use [advert triggerClick] instead. This method will be removed in future releases.")));

#pragma mark - More apps

- (void)loadMoreApps;

- (void)loadMoreAppsWithConfig:(TDMoreAppsConfig *)moreAppsConfig;

- (BOOL)isMoreAppsReady;

- (void)showMoreApps;

#pragma mark Misc

/**
 * This method is only used for plugins such as Unity which do not automatically trigger the launch request on application bootup.
 * Or in the case -setApplicationId:clientKey:properties: is called outside AppDelegate.
 */
- (void)launch;

@end

#pragma mark -
#pragma mark TapdaqDelegate

@protocol TapdaqDelegate <NSObject>

@optional

- (void)didLoadConfig;

- (void)didFailToLoadConfig __attribute__((deprecated("didFailToLoadConfig has been deprecated. Please use didFailToLoadConfigWithError: instead. This method will be removed in future releases.")));

- (void)didFailToLoadConfigWithError:(TDError *)error;

#pragma mark Banner delegate methods

/**
 Called immediately after the banner is loaded.
 This method should be used in conjunction with -getBanner:.
 */
- (void)didLoadBanner;

/**
 Called when, for whatever reason, the banner was not able to be loaded.
 Tapdaq will automatically attempt to load a banner again with a 1 second delay.
 */
- (void)didFailToLoadBanner __attribute__((deprecated("didFailToLoadBanner has been deprecated. Please use didFailToLoadBannerWithError: instead. This method will be removed in future releases.")));

/**
 Called when, for whatever reason, the banner was not able to be loaded.
 Tapdaq will automatically attempt to load a banner again with a 1 second delay.
 */
- (void)didFailToLoadBannerWithError:(TDError *)error;

/**
 Called when the user clicks the banner.
 */
- (void)didClickBanner;

/**
 Called when the ad within the banner view loads another ad.
 */
- (void)didRefreshBanner;

#pragma mark Interstitial delegate methods

/**
 Called immediately after an interstitial is available to the user for a specific placement tag.
 This method should be used in conjunction with -showInterstitialForPlacementTag:.
 @param placementTag A placement tag.
 */
- (void)didLoadInterstitialForPlacementTag:(NSString *)placementTag;

/**
 Called when the interstitial was not able to be loaded for a specific placement tag.
 Tapdaq will automatically attempt to load an interstitial again with a 1 second delay.
 @param placementTag A placement tag.
 */
- (void)didFailToLoadInterstitialForPlacementTag:(NSString *)placementTag __attribute__((deprecated("didFailToLoadInterstitialForPlacementTag: has been deprecated. Please use didFailToLoadInterstitialForPlacementTag:withError: instead. This method will be removed in future releases.")));

/**
 Called when the interstitial was not able to be loaded for a specific placement tag.
 Tapdaq will automatically attempt to load an interstitial again with a 1 second delay.
 @param placementTag A placement tag.
 @param error Error object.
 */
- (void)didFailToLoadInterstitialForPlacementTag:(NSString *)placementTag withError:(TDError *)error;

/**
 Called immediately before the interstitial is to be displayed to the user.
 */
- (void)willDisplayInterstitialForPlacementTag:(NSString *)placementTag;

/**
 Called immediately after the interstitial is displayed to the user.
 */
- (void)didDisplayInterstitialForPlacementTag:(NSString *)placementTag;

/**
 Called when the user closes interstitial, either by tapping the close button, or the background surrounding the interstitial.
 */
- (void)didCloseInterstitialForPlacementTag:(NSString *)placementTag;

/**
 Called when the user clicks the interstitial.
 */
- (void)didClickInterstitialForPlacementTag:(NSString *)placementTag;

#pragma mark Video delegate methods

/**
 Called immediately after a video is available to the user for a specific placement tag.
 This method should be used in conjunction with -showVideoForPlacementTag:.
 @param placementTag A placement tag.
 */
- (void)didLoadVideoForPlacementTag:(NSString *)placementTag;

/**
 Called when, for whatever reason, the video was not able to be loaded.
 Tapdaq will automatically attempt to load a video again with a 1 second delay.
 @param placementTag A placement tag.
 */
- (void)didFailToLoadVideoForPlacementTag:(NSString *)placementTag __attribute__((deprecated("didFailToLoadVideoForPlacementTag: has been deprecated. Please use didFailToLoadVideoForPlacementTag:withError: instead. This method will be removed in future releases.")));

/**
 Called when, for whatever reason, the video was not able to be loaded.
 Tapdaq will automatically attempt to load a video again with a 1 second delay.
 @param placementTag A placement tag.
 @param error Error object.
 */
- (void)didFailToLoadVideoForPlacementTag:(NSString *)placementTag withError:(TDError *)error;

/**
 Called immediately before the video is to be displayed to the user.
 */
- (void)willDisplayVideoForPlacementTag:(NSString *)placementTag;

/**
 Called immediately after the video is displayed to the user.
 */
- (void)didDisplayVideoForPlacementTag:(NSString *)placementTag;

/**
 Called when the user closes the video.
 */
- (void)didCloseVideoForPlacementTag:(NSString *)placementTag;

/**
 Called when the user clicks the video ad.
 */
- (void)didClickVideoForPlacementTag:(NSString *)placementTag;


#pragma mark Rewarded Video delegate methods

/**
 Called immediately after a rewarded video is available to the user for a specific placement tag.
 This method should be used in conjunction with -showRewardedVideoForPlacementTag:.
 @param placementTag A placement tag.
 */
- (void)didLoadRewardedVideoForPlacementTag:(NSString *)placementTag;

/**
 Called when, for whatever reason, the rewarded video was not able to be loaded.
 Tapdaq will automatically attempt to load a rewarded video again with a 1 second delay.
 @param placementTag A placement tag.
 */
- (void)didFailToLoadRewardedVideoForPlacementTag:(NSString *)placementTag __attribute__((deprecated("didFailToLoadRewardedVideoForPlacementTag: has been deprecated. Please use didFailToLoadRewardedVideoForPlacementTag:withError: instead. This method will be removed in future releases.")));

/**
 Called when, for whatever reason, the rewarded video was not able to be loaded.
 Tapdaq will automatically attempt to load a rewarded video again with a 1 second delay.
 @param placementTag A placement tag.
 @param error Error object.
 */
- (void)didFailToLoadRewardedVideoForPlacementTag:(NSString *)placementTag withError:(TDError *)error;
/**
 Called immediately before the rewarded video is to be displayed to the user.
 */
- (void)willDisplayRewardedVideoForPlacementTag:(NSString *)placementTag;

/**
 Called immediately after the rewarded video is displayed to the user.
 */
- (void)didDisplayRewardedVideoForPlacementTag:(NSString *)placementTag;

/**
 Called when the user closes the rewarded video.
 */
- (void)didCloseRewardedVideoForPlacementTag:(NSString *)placementTag;

/**
 Called when the user clicks the rewarded video ad.
 */
- (void)didClickRewardedVideoForPlacementTag:(NSString *)placementTag;

/**
 Called when a reward is ready for the user.
 @param rewardName The name of the reward.
 @param rewardAmount The value of the reward.
 */
- (void)rewardValidationSucceededForRewardName:(NSString *)rewardName
                                  rewardAmount:(int)rewardAmount __attribute__((deprecated("rewardValidationSucceededForRewardName:rewardAmount: has been deprecated. Please use rewardValidationSucceeded: instead. This method will be removed in future releases.")));
/**
 Called when a reward is ready for the user.
 @param placementTag Placement tag.
 @param rewardName The name of the reward.
 @param rewardAmount The value of the reward.
 */
- (void)rewardValidationSucceededForPlacementTag:(NSString *)placementTag
                                      rewardName:(NSString *)rewardName
                                    rewardAmount:(int)rewardAmount __attribute__((deprecated("rewardValidationSucceededForPlacementTag:rewardName:rewardAmount: has been deprecated. Please use rewardValidationSucceeded: instead. This method will be removed in future releases.")));
/**
 Called when a reward is ready for the user.
 @param placementTag Placement tag.
 @param rewardName The name of the reward.
 @param rewardAmount The value of the reward.
 @param payload Dictionary payload configured on the dashboard.
 */
- (void)rewardValidationSucceededForPlacementTag:(NSString *)placementTag
                                      rewardName:(NSString *)rewardName
                                    rewardAmount:(int)rewardAmount
                                         payload:(NSDictionary *)payload  __attribute__((deprecated("rewardValidationSucceededForPlacementTag:rewardName:rewardAmount:payload: has been deprecated. Please use rewardValidationSucceeded: instead. This method will be removed in future releases.")));

/**
Called when a reward is ready for the user.
@param reward Reward object.
@see TDReward
*/
- (void)rewardValidationSucceeded:(TDReward *)reward;

/**
 Called if an error occurred when rewarding the user.
 */
- (void)rewardValidationErroredForPlacementTag:(NSString *)placementTag;

#pragma mark Native advert delegate methods

/**
 Called when a native advert is successfully loaded, used in conjunction with -loadNativeAdvertForPlacementTag:adType:.
 
 @param placementTag The placement tag of the native advert that loaded.
 @param nativeAdType The ad type of the native advert that loaded.
 */
- (void)didLoadNativeAdvertForPlacementTag:(NSString *)placementTag
                                    adType:(TDNativeAdType)nativeAdType;

/**
 Called when the native ad failed to load, used in conjunction with -loadNativeAdvertForPlacementTag:adType:.
 
 @param placementTag The placement tag that failed to load the native ad.
 @param nativeAdType The ad type of the native advert that failed to load.
 */
- (void)didFailToLoadNativeAdvertForPlacementTag:(NSString *)placementTag
                                          adType:(TDNativeAdType)nativeAdType;

#pragma mark More apps delegate methods

- (void)didLoadMoreApps;

- (void)didFailToLoadMoreApps __attribute__((deprecated("didFailToLoadMoreApps has been deprecated. Please use didFailToLoadMoreAppsWithError: instead. This method will be removed in future releases.")));

- (void)didFailToLoadMoreAppsWithError:(TDError *)error;

- (void)willDisplayMoreApps;

- (void)didDisplayMoreApps;

- (void)didCloseMoreApps;

#pragma mark Offerwall delegate methods
    
- (void)didLoadOfferwall;
    
- (void)didFailToLoadOfferwall __attribute__((deprecated("didFailToLoadOfferwall has been deprecated. Please use didFailToLoadOfferwallWithError: instead. This method will be removed in future releases.")));

- (void)didFailToLoadOfferwallWithError:(TDError *)error;
    
- (void)willDisplayOfferwall;
    
- (void)didDisplayOfferwall;
    
- (void)didCloseOfferwall;
    
- (void)didReceiveOfferwallCredits:(NSDictionary *)creditInfo;
    
- (void)didFailToReceiveOfferwallCredits;
@end
