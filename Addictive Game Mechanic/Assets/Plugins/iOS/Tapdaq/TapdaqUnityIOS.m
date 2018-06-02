//
//  TapdaqUnityIOS.m
//  TapdaqUnity
//
//  Created by Rheo Violenes on 05/05/15.
//  Copyright (c) 2015 Nerd. All rights reserved.
//
#import "TapdaqUnityIOS.h"
#import "TapdaqDelegates.h"
#import "TapdaqStandardAd.h"

static NSString *const kTapdaqLogPrefix = @"[TapdaqUnity]";

void _ConfigureTapdaq(const char* appIdChar,
                      const char* clientKeyChar,
                      const char* enabledAdTypesChar,
                      const char* testDevicesChar,
                      bool isDebugMode,
                      bool autoReloadAds,
                      const char* pluginVersion,
                      bool isConsentGiven,
                      bool isAgeRestrictedUser) {
    
    bool isValid = true;
    
    if (_isEmpty(appIdChar)) {
        NSLog(@"%@ iOS App ID is empty", kTapdaqLogPrefix);
        isValid = false;
    }
    
    if (_isEmpty(clientKeyChar)) {
        NSLog(@"%@ iOS Client Key is empty", kTapdaqLogPrefix);
        isValid = false;
    }
    
    if (_isEmpty(enabledAdTypesChar)) {
        NSLog(@"%@ No placements are registered", kTapdaqLogPrefix);
        isValid = false;
    }
    
    if (!isValid) {
        NSLog(@"%@ Tapdaq did not initialise", kTapdaqLogPrefix);
        return;
    }
    
    NSString *appId = [[NSString stringWithUTF8String:appIdChar] copy];
    NSString *clientKey = [[NSString stringWithUTF8String:clientKeyChar] copy];
    NSString *enabledAdTypes = [[NSString stringWithUTF8String:enabledAdTypesChar] copy];
    NSString *testDevices = [[NSString stringWithUTF8String:testDevicesChar] copy];
    NSString *version = [[NSString stringWithUTF8String:pluginVersion] copy];
    
    [[TapdaqUnityIOS sharedInstance] initWithApplicationId:appId
                                                 clientKey:clientKey
                                            enabledAdTypes:enabledAdTypes
                                               testDevices:testDevices
                                               isDebugMode:isDebugMode
                                             autoReloadAds:autoReloadAds
                                             pluginVersion:version
                                            isConsentGiven:isConsentGiven
                                       isAgeRestrictedUser:isAgeRestrictedUser];
    
}

bool _IsInitialised() {
    return  [[TapdaqUnityIOS sharedInstance] IsInitialised];
}

void _SetConsentGiven(bool isConsentGiven) {
    [[TapdaqUnityIOS sharedInstance] setConsentGiven:isConsentGiven];
}

bool _IsConsentGiven() {
    return (bool)[[TapdaqUnityIOS sharedInstance] isConsentGiven];
}

void _SetAgeRestrictedUser(bool isAgeRestrictedUser) {
    [[TapdaqUnityIOS sharedInstance] SetAgeRestrictedUser:isAgeRestrictedUser];
}

bool _IsAgeRestrictedUser() {
    return (bool)[[TapdaqUnityIOS sharedInstance] isAgeRestrictedUser];
}

#pragma mark - Banner (Bridge)

void _LoadBannerForSize(const char* sizeChar) {
    
    if (_isEmpty(sizeChar)) {
        NSLog(@"%@ No banner size specified, cannot load banner", kTapdaqLogPrefix);
        return;
    }
    
    NSString *sizeStr = [[NSString stringWithUTF8String:sizeChar] copy];
    
    [[TapdaqBannerAd sharedInstance] loadForSize:sizeStr];
    
}

bool _IsBannerReady() {
    
    return (bool) [[TapdaqBannerAd sharedInstance] isReady];
    
}

void _ShowBanner(const char* position) {
    
    if (_isEmpty(position)) {
        NSLog(@"%@ No banner position given, failed to show banner", kTapdaqLogPrefix);
        return;
    }
    
    [[TapdaqBannerAd sharedInstance] show: position];
    
}

void _HideBanner() {
    
    [[TapdaqBannerAd sharedInstance] hide];
    
}

#pragma mark - Interstitial (Bridge)

void _LoadInterstitialWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to load interstitial", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqInterstitialAd sharedInstance] loadForPlacementTag:tag];
    
}

bool _IsInterstitialReadyWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to check if interstitial is ready", kTapdaqLogPrefix);
        return false;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    return (bool) [[TapdaqInterstitialAd sharedInstance] isReadyForPlacementTag:tag];
    
}

void _ShowInterstitialWithTag(const char* tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show interstitial", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqInterstitialAd sharedInstance] showForPlacementTag:tag];
    
}

#pragma mark - Video (Bridge)

void _LoadVideoWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to load video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqVideoAd sharedInstance] loadForPlacementTag:tag];
    
}

bool _IsVideoReadyWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to check if video is ready", kTapdaqLogPrefix);
        return false;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    return (bool) [[TapdaqVideoAd sharedInstance] isReadyForPlacementTag:tag];
    
}

void _ShowVideoWithTag(const char* tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqVideoAd sharedInstance] showForPlacementTag:tag];
    
}

#pragma mark - Rewarded Video (Bridge)

void _LoadRewardedVideoWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to load rewarded video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqRewardedVideoAd sharedInstance] loadForPlacementTag:tag];
    
}

bool _IsRewardedVideoReadyWithTag(const char *tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to check if rewarded video is ready", kTapdaqLogPrefix);
        return false;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    return (bool) [[TapdaqRewardedVideoAd sharedInstance] isReadyForPlacementTag:tag];
    
}

void _ShowRewardedVideoWithTag(const char* tagChar) {
    
    if (_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show rewarded video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqRewardedVideoAd sharedInstance] showForPlacementTag:tag];
    
}

#pragma mark - Native Ads (Bridge)

void _LoadNativeAdvertForPlacementTag(const char* tag, const char* nativeAdType)
{
    bool isValid = true;
    
    if (_isEmpty(tag)) {
        NSLog(@"%@ No tag given", kTapdaqLogPrefix);
        isValid = false;
    }
    
    if (_isEmpty(nativeAdType)) {
        NSLog(@"%@ No nativeAdType given", kTapdaqLogPrefix);
        isValid = false;
    }
    
    if (!isValid) {
        NSLog(@"%@ Failed to load native ad", kTapdaqLogPrefix);
        return;
    }
    
    [[TapdaqNativeAd sharedInstance] loadNativeAdvertForPlacementTag: tag nativeAdType: nativeAdType];
}

const char* _GetNativeAdWithTag (const char* tag, const char* nativeAdType) {
    
    bool isValid = true;
    
    if (_isEmpty(tag)) {
        NSLog(@"%@ No tag given", kTapdaqLogPrefix);
        isValid = false;
    }
    
    if (_isEmpty(nativeAdType)) {
        NSLog(@"%@ No nativeAdType given", kTapdaqLogPrefix);
        isValid = false;
    }
    
    if (!isValid) {
        NSLog(@"%@ Failed to get native ad", kTapdaqLogPrefix);
        return "";
    }
    
    return [[TapdaqNativeAd sharedInstance] fetchNativeForAdWithTag:tag AdType:nativeAdType];
}

void _SendNativeClick(const char* uniqueId) {
    
    if (_isEmpty(uniqueId)) {
        NSLog(@"%@ No uniqueId given, failed to send native click", kTapdaqLogPrefix);
        return;
    }
    
    [[TapdaqNativeAd sharedInstance] triggerClickForNativeAdvert:uniqueId];
    
}

void _SendNativeImpression(const char* uniqueId) {
    
    if (_isEmpty(uniqueId)) {
        NSLog(@"%@ No uniqueId given, failed to send native impression", kTapdaqLogPrefix);
        return;
    }
    
    [[TapdaqNativeAd sharedInstance] triggerImpressionForNativeAdvert:uniqueId];
    
}

#pragma mark - More Apps

void _ShowMoreApps() {
    [[TapdaqMoreApps sharedInstance] show];
}

bool _IsMoreAppsReady() {
    return [[TapdaqMoreApps sharedInstance] isReady];
}

void _LoadMoreApps() {
    [[TapdaqMoreApps sharedInstance] load];
}

void _LoadMoreAppsWithConfig(const char* config) {
    [[TapdaqMoreApps sharedInstance] loadWithConfig: config];
}

#pragma mark - Offerwall

void _ShowOfferwall() {
    [[TapdaqOfferwall sharedInstance] show];
}

bool _IsOfferwallReady() {
    return [[TapdaqOfferwall sharedInstance] isReady];
}

void _LoadOfferwall() {
    return [[TapdaqOfferwall sharedInstance] load];
}

#pragma mart - Stats

void _SendIAP(const char* name, double price, const char* locale) {
    NSString *nameString = [NSString stringWithUTF8String:name];
    NSString *localeString = [NSString stringWithUTF8String:locale];
    
    [[Tapdaq sharedSession] trackPurchaseForProductName:nameString price:price priceLocale:localeString];
}

const char *_GetRewardId(const char* tag) {
    NSString *placementTag = [NSString stringWithUTF8String:tag];
    NSString *rewardId = [[Tapdaq sharedSession] rewardIdForPlacementTag:placementTag];
    return [rewardId cStringUsingEncoding:NSUTF8StringEncoding];
}

void _LaunchMediationDebugger() {
    [[Tapdaq sharedSession] presentDebugViewController];
}

bool _isEmpty(const char* str) {
    return str == NULL;
}

@implementation TapdaqUnityIOS

+ (instancetype)sharedInstance
{
    static dispatch_once_t once;
    static id sharedInstance;
    dispatch_once(&once, ^{
        sharedInstance = [[self alloc] init];
    });
    return sharedInstance;
}

+ (void)load
{
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(createPlugin:) name:UIApplicationDidFinishLaunchingNotification object:nil];
}

+ (void)createPlugin:(NSNotification *)notification
{
    [TapdaqUnityIOS sharedInstance];
}

// Init
//Configure Tapdaq with credentials and ad settings
- (void)initWithApplicationId:(NSString *)appID
                    clientKey:(NSString *)clientKey
               enabledAdTypes:(NSString *)enabledAdTypes
                  testDevices:(NSString *)testDevices
                  isDebugMode:(bool)isDebugMode
                autoReloadAds:(bool)autoReloadAds
                pluginVersion:(NSString *)pluginVersion
               isConsentGiven:(bool)isConsentGiven
          isAgeRestrictedUser:(bool)isAgeRestrictedUser
{
    NSLog(@"enabledAdTypes: %@", enabledAdTypes);
    
    TDProperties *properties = [[TDProperties alloc] init];
    [properties setPluginVersion:pluginVersion];
    properties.isDebugEnabled = isDebugMode;
    properties.logLevel = isDebugMode ? TDLogLevelDebug : TDLogLevelInfo;
    [properties setAutoReloadAds:autoReloadAds];
    properties.isConsentGiven = isConsentGiven;
    properties.isAgeRestrictedUser = isAgeRestrictedUser;
    
    if (enabledAdTypes && [enabledAdTypes length] > 0) {
        
        NSData *data = [enabledAdTypes dataUsingEncoding:NSUTF8StringEncoding];
        
        NSError *error = nil;
        NSArray *arr = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
        
        if (!error) {
            
            if (enabledAdTypes && [enabledAdTypes length] > 0) {
                
                NSDictionary *validAdTypes = @{
                                               
                                               @"TDAdTypeNone": @(0),
                                               @"TDAdTypeInterstitial": @(1),
                                               @"TDAdType1x1Large": @(2),
                                               @"TDAdType1x1Medium": @(3),
                                               @"TDAdType1x1Small": @(4),
                                               
                                               @"TDAdType1x2Large": @(5),
                                               @"TDAdType1x2Medium": @(6),
                                               @"TDAdType1x2Small": @(7),
                                               
                                               @"TDAdType2x1Large": @(8),
                                               @"TDAdType2x1Medium": @(9),
                                               @"TDAdType2x1Small": @(10),
                                               
                                               @"TDAdType2x3Large": @(11),
                                               @"TDAdType2x3Medium": @(12),
                                               @"TDAdType2x3Small": @(13),
                                               
                                               @"TDAdType3x2Large": @(14),
                                               @"TDAdType3x2Medium": @(15),
                                               @"TDAdType3x2Small": @(16),
                                               
                                               @"TDAdType1x5Large": @(17),
                                               @"TDAdType1x5Medium": @(18),
                                               @"TDAdType1x5Small": @(19),
                                               
                                               @"TDAdType5x1Large": @(20),
                                               @"TDAdType5x1Medium": @(21),
                                               @"TDAdType5x1Small": @(22),
                                               
                                               @"TDAdTypeVideo": @(23),
                                               @"TDAdTypeRewardedVideo": @(24),
                                               @"TDAdTypeBanner": @(25),
                                               @"TDAdTypeOfferwall": @(26)
                                               
                                               };
                
                NSMutableDictionary *tagsWithAdTypes = [[NSMutableDictionary alloc] init];
                
                for (NSDictionary *dict in arr) {
                    
                    NSString *adTypeStr = [dict objectForKey:@"ad_type"];
                    NSArray *placementTags = [dict objectForKey:@"placement_tags"];
                    
                    if ([placementTags count] > 0) {
                        for (NSString *placementTag in placementTags) {
                            
                            // update tagsWithAdTypes
                            NSNumber *combinedAdTypeNum = [tagsWithAdTypes objectForKey:placementTag];
                            
                            if (!combinedAdTypeNum) {
                                combinedAdTypeNum = @(0);
                            }
                            
                            TDAdTypes adTypesCombined = [combinedAdTypeNum integerValue];
                            
                            NSNumber *adTypeNum = [validAdTypes objectForKey:adTypeStr];
                            NSInteger adTypeInt = [adTypeNum integerValue];
                            
                            adTypesCombined |= 1 << adTypeInt;
                            
                            combinedAdTypeNum = @(adTypesCombined);
                            
                            [tagsWithAdTypes setObject:combinedAdTypeNum forKey:placementTag];
                            
                        }
                    }
                    
                }
                
                
                for (id key in tagsWithAdTypes) {
                    
                    if ([key isKindOfClass:[NSString class]] && [[tagsWithAdTypes objectForKey:key] integerValue] > 0) {
                        NSString *tag = (NSString *) key;
                        TDAdTypes adTypes = (TDAdTypes) [[tagsWithAdTypes objectForKey:key] integerValue];
                        
                        if (tag && [tag length] > 0) {
                            TDPlacement *placement = [[TDPlacement alloc] initWithAdTypes:adTypes forTag:tag];
                            NSLog(@"placement: %@", placement);
                            [properties registerPlacement:placement];
                        }
                        
                    }
                    
                }
                
            }
            
        }
    }
    
    [self setTestDevices: testDevices toProperties:properties];
    
    [[Tapdaq sharedSession] setApplicationId:appID
                                   clientKey:clientKey
                                  properties:properties];
    
    [(Tapdaq *)[Tapdaq sharedSession] setDelegate: [TapdaqDelegates sharedInstance]];
    
    
    [[Tapdaq sharedSession] launch];
}

- (BOOL) IsInitialised
{
    return [[Tapdaq sharedSession] isInitialised];
}

- (void) setConsentGiven:(BOOL)isConsentGiven
{
    [[Tapdaq sharedSession] setIsConsentGiven:isConsentGiven];
}

- (BOOL) isConsentGiven
{
    return [[Tapdaq sharedSession] isConsentGiven];
}

- (void) SetAgeRestrictedUser:(BOOL)isAgeRestrictedUser
{
    [[Tapdaq sharedSession] setIsAgeRestrictedUser:isAgeRestrictedUser];
}

- (BOOL) isAgeRestrictedUser
{
    return [[Tapdaq sharedSession] isAgeRestrictedUser];
}

- (void) setTestDevices:(NSString *)testDevicesJson toProperties:(TDProperties *)properties
{
    NSData *data = [testDevicesJson dataUsingEncoding:NSUTF8StringEncoding];
    
    NSError *error = nil;
    NSDictionary *testDevicesDictionary = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
    
    if(error == nil) {
        NSArray* amArray = testDevicesDictionary[@"adMobDevices"];
        NSArray* fbArray = testDevicesDictionary[@"facebookDevices"];
        
        if(amArray != nil) {
            TDTestDevices *amTestDevices = [[TDTestDevices alloc] initWithNetwork:@"admob" testDevices:amArray];
            [properties registerTestDevices: amTestDevices];
        }
        
        if(fbArray != nil) {
            TDTestDevices *fbTestDevices = [[TDTestDevices alloc] initWithNetwork:@"facebook" testDevices:fbArray];
            [properties registerTestDevices: fbTestDevices];
        }
    }
}

@end
