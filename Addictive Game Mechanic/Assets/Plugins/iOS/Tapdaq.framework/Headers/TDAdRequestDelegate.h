//
//  TDAdRequestDelegate.h
//  Tapdaq
//
//  Created by Dmitry Dovgoshliubnyi on 24/01/2018.
//  Copyright © 2018 Tapdaq. All rights reserved.
//

#import <Foundation/Foundation.h>

typedef NS_ENUM(NSUInteger, TDNativeAdType);
@class TDAdRequest;
@class TDError;

/**
 * Implementing this protocol allows listening to loading events from ad requests.
 */
@protocol TDAdRequestDelegate <NSObject>
@optional
- (void)didLoadAdRequest:(TDAdRequest * _Nonnull)adRequest;
- (void)adRequest:(TDAdRequest * _Nonnull)adRequest didFailToLoadWithError:(TDError * _Nullable)error;
@end

/**
 * Implementing this protocol allows listening to presentation events from ad requests.
 */
@protocol TDDisplayableAdRequestDelegate <TDAdRequestDelegate>
@optional
- (void)willDisplayAdRequest:(TDAdRequest * _Nonnull)adRequest;
- (void)didDisplayAdRequest:(TDAdRequest * _Nonnull)adRequest;
- (void)didCloseAdRequest:(TDAdRequest * _Nonnull)adRequest;
@end

/**
 * Implementing this protocol allows listening to click events from ad requests.
 */
@protocol TDClickableAdRequestDelegate <TDAdRequestDelegate>
@optional
- (void)didClickAdRequest:(TDAdRequest * _Nonnull)adRequest;
@end

/**
 * Implementing this protocol allows listening to reward related events from rewarded ad requests.
 */
@protocol TDRewardedVideoAdRequestDelegate <TDAdRequestDelegate>
@optional
- (void)adRequest:(TDAdRequest * _Nonnull)adRequest didValidateReward:(TDReward * _Nonnull)reward;
- (void)didFailToValidateRewardAdRequest:(TDAdRequest * _Nonnull)adRequest;
@end

/**
 * Implementing this protocol allows listening to banner related events from ad requests.
 */
@protocol TDBannerAdRequestDelegate <TDAdRequestDelegate>
@optional
- (void)didRefreshBannerForAdRequest:(TDAdRequest * _Nonnull)adRequest;
@end

/**
 * Implementing this protocol allows listening to offerwall related events from ad requests.
 */
@protocol TDOfferwallAdRequestDelegate <TDAdRequestDelegate>
@optional
- (void)adRequest:(TDAdRequest * _Nonnull)adRequest didReceiveOfferwallCredits:(NSDictionary * _Nullable)creditInfo;
- (void)didFailToReceiveOfferwallCreditsAdRequest:(TDAdRequest * _Nonnull)adRequest;
@end

/**
 * Implementing this protocol allows listening to native ads related events from ad requests.
 */
@protocol TDNativeAdRequestDelegate <TDAdRequestDelegate>
@optional
/**
 Called when a native advert is successfully loaded, used in conjunction with -loadNativeAdvertForPlacementTag:adType:.
 
 @param nativeAdType The ad type of the native advert that loaded.
 */
- (void)didLoadNativeAdvertOfType:(TDNativeAdType)nativeAdType;

/**
 Called when the native ad failed to load, used in conjunction with -loadNativeAdvertForPlacementTag:adType:.
 
 @param nativeAdType The ad type of the native advert that failed to load.
 */
- (void)didFailToLoadNativeAdvertOfType:(TDNativeAdType)nativeAdType;
@end
