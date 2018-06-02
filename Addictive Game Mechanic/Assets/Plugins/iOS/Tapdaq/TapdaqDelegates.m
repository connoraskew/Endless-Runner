#import "TapdaqDelegates.h"
#import "JsonHelper.h"
#import "TapdaqNativeAd.h"

@implementation TapdaqDelegates

+ (instancetype)sharedInstance
{
    static dispatch_once_t once;
    static id sharedInstance;
    dispatch_once(&once, ^{
        sharedInstance = [[self alloc] init];
    });
    return sharedInstance;
}

- (NSDictionary *)errorDictWithError:(TDError *)error {
    if (error != nil) {
        NSMutableDictionary *subErrorDicts = [NSMutableDictionary dictionary];
        for (NSString *subErrorKey in error.subErrors.allKeys) {
            NSError *subError = error.subErrors[subErrorKey];
            NSDictionary *dict = @{
                                   @"code": @(subError.code),
                                   @"message": (subError.localizedDescription == nil ? @"" : subError.localizedDescription)
                                   };
            subErrorDicts[subErrorKey] = dict;
        }
        return @{
                 @"code": @(error.code),
                 @"message": (error.localizedDescription == nil ? @"" : error.localizedDescription),
                 @"subErrors": subErrorDicts
                 };
    }
    return nil;
}

- (void) send:(NSString *) methodName adType:(NSString *) adType tag:(NSString *) tag message: (NSString *) message
{
    [self send:methodName adType:adType tag:tag message:message error:nil];
}

- (void) send:(NSString *) methodName adType:(NSString *) adType tag:(NSString *) tag message: (NSString *) message error:(TDError *)error
{
    NSMutableDictionary* dict = [@{
                                   @"adType": adType,
                                   @"tag": tag,
                                   @"message": message
                                   } mutableCopy];
    if (error != nil) {
        dict[@"error"] = [self errorDictWithError:error];
    }
    [self send: methodName dictionary: dict];
}

- (void) send:(NSString *) methodName error:(TDError *)error
{
    NSDictionary * errorDict = [self errorDictWithError:error];
    [self send: methodName dictionary: errorDict != nil ? errorDict : @{}];
}

- (void) send:(NSString *) methodName dictionary:(NSDictionary *) dictionary
{
    [self send: methodName message: [JsonHelper toJsonString: dictionary]];
}

- (void) send:(NSString *) methodName message:(NSString *) message
{
    UnitySendMessage("TapdaqV1", [methodName UTF8String], [message UTF8String]);
}

#pragma mark - TapdaqDelegate

- (void)didLoadConfig
{
    [self send:@"_didLoadConfig" message:@""];
}

- (void)didFailToLoadConfigWithError:(TDError *)error
{
    [self send:@"_didFailToLoadConfig" error:error];
}

#pragma mark Banner delegate methods

- (void)didLoadBanner
{
    [self send: @"_didLoad" adType: @"BANNER" tag: @"" message: @"LOADED"];
}

- (void)didFailToLoadBannerWithError:(TDError *)error
{
    [self send: @"_didFailToLoad" adType: @"BANNER" tag: @"" message: @"LOAD_FAILED" error:error];
}

- (void)didClickBanner
{
    [self send: @"_didClick" adType: @"BANNER" tag: @"" message: @"CLICK"];
}

- (void)didRefreshBanner
{
    [self send: @"_didRefresh" adType: @"BANNER" tag: @"" message: @"REFRESH"];
}

#pragma mark Interstitial delegate methods

- (void)didLoadInterstitialForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didLoad" adType: @"INTERSTITIAL" tag: placementTag message: @"LOADED"];
}

- (void)didFailToLoadInterstitialForPlacementTag:(NSString *)placementTag withError:(TDError *)error
{
    [self send: @"_didFailToLoad" adType: @"INTERSTITIAL" tag: placementTag message: @"LOAD_FAILED" error:error];
}

- (void)willDisplayInterstitialForPlacementTag:(NSString *) placementTag
{
    [self send: @"_willDisplay" adType: @"INTERSTITIAL" tag: placementTag message: @""];
}

- (void)didDisplayInterstitialForPlacementTag: (NSString *) placementTag
{
    [self send: @"_didDisplay" adType: @"INTERSTITIAL" tag: placementTag message: @""];
}

- (void)didCloseInterstitialForPlacementTag: (NSString *) placementTag
{
    [self send: @"_didClose" adType: @"INTERSTITIAL" tag: placementTag message: @""];
}

- (void)didClickInterstitialForPlacementTag: (NSString *) placementTag
{
    [self send: @"_didClick" adType: @"INTERSTITIAL" tag: placementTag message: @""];
}

#pragma mark Video delegate methods

- (void)didLoadVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didLoad" adType: @"VIDEO" tag: placementTag message: @""];
}

- (void)didFailToLoadVideoForPlacementTag:(NSString *)placementTag withError:(TDError *)error
{
    [self send: @"_didFailToLoad" adType: @"VIDEO" tag: placementTag message: @"" error:error];
}

- (void)willDisplayVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_willDisplay" adType: @"VIDEO" tag: placementTag message: @""];
}

- (void)didDisplayVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didDisplay" adType: @"VIDEO" tag: placementTag message: @""];
}

- (void)didCloseVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didClose" adType: @"VIDEO" tag: placementTag message: @""];
}

- (void)didClickVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didClick" adType: @"VIDEO" tag: placementTag message: @""];
}

#pragma mark Rewarded Video delegate methods

- (void)didLoadRewardedVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didLoad" adType: @"REWARD_AD" tag: placementTag message: @""];
}

- (void)didFailToLoadRewardedVideoForPlacementTag:(NSString *)placementTag withError:(TDError *)error
{
    [self send: @"_didFailToLoad" adType: @"REWARD_AD" tag: placementTag message: @"" error:error];
}

- (void)willDisplayRewardedVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_willDisplay" adType: @"REWARD_AD" tag: placementTag message: @""];
}

- (void)didDisplayRewardedVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didDisplay" adType: @"REWARD_AD" tag: placementTag message: @""];
}

- (void)didCloseRewardedVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didClose" adType: @"REWARD_AD" tag: placementTag message: @""];
}

- (void)didClickRewardedVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didClick" adType: @"REWARD_AD" tag: placementTag message: @""];
}

- (void)rewardValidationSucceeded:(TDReward *)reward {
    NSMutableDictionary* dict = [[NSMutableDictionary alloc] init];
    
    dict[@"RewardName"] = reward.name;
    dict[@"RewardAmount"] = @(reward.value);
    dict[@"RewardValid"] = @YES;
    dict[@"Tag"] = reward.tag;
    dict[@"EventId"] = reward.eventId;
    
    if (reward.customJson != nil) {
        dict[@"RewardJson"] = [JsonHelper toJsonString:reward.customJson];
    } else {
        dict[@"RewardJson"] = [JsonHelper toJsonString:[NSDictionary dictionary]];
    }
    [self send: @"_didVerify" dictionary: dict];
}

-(void)rewardValidationErroredForPlacementTag:(NSString *)placementTag {
    [self send: @"_onValidationFailed" adType: @"REWARD_AD" tag: placementTag message: @""];
}


#pragma mark Native delegate methods

- (void)didLoadNativeAdvertForPlacementTag:(NSString *)placementTag
                                    adType:(TDNativeAdType)nativeAdType
{
    NSDictionary* nativeMessageDict = @{
                                        @"nativeType": [[TapdaqNativeAd sharedInstance] getStringFromAdType:  nativeAdType]
                                        };
    [self send: @"_didLoad" adType: @"NATIVE_AD" tag: placementTag message: [JsonHelper toJsonString: nativeMessageDict]];
}

- (void)didFailToLoadNativeAdvertForPlacementTag:(NSString *)placementTag
                                          adType:(TDNativeAdType)nativeAdType
{
    NSDictionary* nativeMessageDict = @{
                                        @"nativeType": [[TapdaqNativeAd sharedInstance] getStringFromAdType:  nativeAdType]
                                        };
    [self send: @"_didFailToLoad" adType: @"NATIVE_AD" tag: placementTag message: [JsonHelper toJsonString: nativeMessageDict]];
}

#pragma mark MoreApps delegate methods

- (void)didLoadMoreApps
{
    NSDictionary* dict = @{
                           @"adType": @"MORE_APPS"
                           };
    [self send:@"_didLoad" dictionary: dict];
}

- (void)didFailToLoadMoreAppsWithError:(TDError *)error
{
    NSMutableDictionary* dict = [@{
                                   @"adType": @"MORE_APPS"
                                   } mutableCopy];
    if (error != nil) {
        dict[@"error"] = [self errorDictWithError:error];
    }
    [self send:@"_didFailToLoad" dictionary: dict];
}

- (void)willDisplayMoreApps
{
    NSDictionary* dict = @{
                           @"adType": @"MORE_APPS"
                           };
    [self send:@"_willDisplay" dictionary: dict];
}

- (void)didDisplayMoreApps
{
    NSDictionary* dict = @{
                           @"adType": @"MORE_APPS"
                           };
    [self send:@"_didDisplay" dictionary: dict];
}

- (void)didCloseMoreApps
{
    NSDictionary* dict = @{
                           @"adType": @"MORE_APPS"
                           };
    [self send:@"_didClose" dictionary: dict];
}


#pragma mark Offerwall delegate methods

- (void)didLoadOfferwall
{
    NSDictionary* dict = @{
                           @"adType": @"OFFERWALL"
                           };
    [self send:@"_didLoad" dictionary: dict];
}

- (void)didFailToLoadOfferwallWithError:(TDError *)error
{
    NSMutableDictionary* dict = [@{
                                   @"adType": @"OFFERWALL"
                                   } mutableCopy];
    if (error != nil) {
        dict[@"error"] = [self errorDictWithError:error];
    }
    [self send:@"_didFailToLoad" dictionary: dict];
}

- (void)willDisplayOfferwall
{
    NSDictionary* dict = @{
                           @"adType": @"OFFERWALL"
                           };
    [self send:@"_willDisplay" dictionary: dict];
}

- (void)didDisplayOfferwall
{
    NSDictionary* dict = @{
                           @"adType": @"OFFERWALL"
                           };
    [self send:@"_didDisplay" dictionary: dict];
}

- (void)didCloseOfferwall
{
    NSDictionary* dict = @{
                           @"adType": @"OFFERWALL"
                           };
    [self send:@"_didClose" dictionary: dict];
}

- (void)didReceiveOfferwallCredits:(NSDictionary *)creditInfo {
    
    NSMutableDictionary * dict = [NSMutableDictionary dictionaryWithDictionary: creditInfo];
    dict[@"Event"] = @"onOfferwallAdCredited";
    [self send:@"_didCustomEvent" dictionary: dict];
}

- (void)didFailToReceiveOfferwallCredits {
    NSDictionary* dict = @{
                           @"Event": @"onGetOfferwallCreditsFailed"
                           };
    [self send:@"_didCustomEvent" dictionary: dict];
}

@end

