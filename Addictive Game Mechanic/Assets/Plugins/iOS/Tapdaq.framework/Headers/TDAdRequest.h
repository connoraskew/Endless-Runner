//
//  TDAdRequest.h
//  Tapdaq
//
//  Created by Dmitry Dovgoshliubnyi on 16/01/2018.
//  Copyright © 2018 Tapdaq. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "TDAdTypeEnum.h"
#import "TDPlacement.h"

@protocol TDAdRequestDelegate;
@protocol TDDisplayableAdRequestDelegate;
@protocol TDClickableAdRequestDelegate;
@protocol TDRewardedVideoAdRequestDelegate;
@protocol TDBannerAdRequestDelegate;
@protocol TDOfferwallAdRequestDelegate;


@class TDReward;
@class TDError;
@class TDAdRequest;

typedef void (^TDAdRequestCompletion)(TDAdRequest * _Nonnull request, TDError * _Nullable error);

#pragma mark - Base Ad Request
@interface TDAdRequest : NSObject
/**
 * This object contains placement information such as placement tag and ad unit that is being loaded.
 */
@property (readonly, nonatomic, nonnull) TDPlacement *placement;

/**
 * A hashed user identifier to be passed through web-hook when a reward is received.
 *
 * Supported Ad types: TDAdTypeRewardedVideo
 */
@property (strong, nonatomic, nullable) NSString *hashedUserId;

/**
 * Delegate that will received all events associated with this ad request.
 */
@property (weak, nonatomic, nullable) id<TDAdRequestDelegate>delegate;

/**
 * Returns YES if this ad is ready to be shown.
 */
- (BOOL)isReady;
@end

