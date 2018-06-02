//
//  TDErrorCodes.h
//  Tapdaq
//
//  Created by Dmitry Dovgoshliubnyi on 15/01/2018.
//  Copyright © 2018 Tapdaq. All rights reserved.
//

#import <Foundation/Foundation.h>

typedef enum {
    TDErrorCodeUnknown = -999,
    TDErrorCodeNotInitialised = 10,
    TDErrorCodeInvalidCredentials = 20,
    TDErrorCodeNoAdepters = 100,
    TDErrorCodeNoAdeptersOrCrossPromo = 101,
    TDErrorCodeNoNetworksEnabled = 110,
    TDErrorCodeNoPlacementTagAvailable = 120,
    TDErrorCodeFailedToLoad = 130,
    TDErrorCodeNoAdLoadedForPlacement = 200,
    
    TDErrorCodeCrossPromoNoAdAvailable = 300,
    TDErrorCodeCrossPromoFrequencyCapReached = 301,
    TDErrorCodeCrossPromoFailedToDownloadCreative = 340,
    TDErrorCodeCrossPromoFailedToSaveCreative = 341,
    
    TDErrorCodeAdapterFailedToLoad = 1000,
    TDErrorCodeAdapterAdIdMissing = 1001,
    TDErrorCodeAdapterBannerSizeUnsupported= 1010,
    TDErrorCodeAdapterTimeout = 1011,
    TDErrorCodeAdapterNetworkNotInitialised = 1100,
    TDErrorCodeAdUnitSuspended = 1012,
    
    TDErrorCodeAdapterAppLovinFailedToLoad = 12000,
    TDErrorCodeAdapterChartboostFailedToLoad = 13000,
    TDErrorCodeAdapterHyprMXFailedToLoad = 15000,
    TDErrorCodeAdapterIronSourceFailedToLoad = 17000,
    TDErrorCodeAdapterKiipNoReward = 23000,
    TDErrorCodeAdapterMoPubFailedToLoad = 18000,
    TDErrorCodeAdapterReceptivFailedToLoad = 19000,
    TDErrorCodeAdapterTapjoyNoContent = 20100,
    TDErrorCodeAdapterUnityAdsDisabled = 21100,
    TDErrorCodeAdapterUnityAdsNoFill = 21101,
    TDErrorCodeAdapterUnityAdsUnavailable = 21102,
    TDErrorCodeAdapterVungleFailedToLoad = 22000
} TDErrorCode;
