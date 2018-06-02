//
//  TDAdTypePlacement.h
//  Tapdaq
//
//  Created by Tapdaq <support@tapdaq.com>
//  Copyright (c) 2016 Tapdaq. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "TDAdTypeEnum.h"
#import "TDNativeAdTypeEnum.h"

typedef NSString *const TDPTag;

// Default.
extern TDPTag const TDPTagDefault;
// Bootup - Initial bootup of game.
extern TDPTag const TDPTagBootup;
// Home Screen - Home screen the player first sees.
extern TDPTag const TDPTagHomeScreen;
// Main Menu - Menu that provides game options.
extern TDPTag const TDPTagMainMenu;
// Pause - Pause screen.
extern TDPTag const TDPTagPause;
// Level Start - Start of the level.
extern TDPTag const TDPTagLevelStart;
// Level Complete - Completion of the level.
extern TDPTag const TDPTagLevelComplete;
// Game Center - After a user visits the Game Center.
extern TDPTag const TDPTagGameCenter;
// IAP Store - The store where the player pays real money for currency or items.
extern TDPTag const TDPTagIAPStore;
// Item Store - The store where a player buys virtual goods.
extern TDPTag const TDPTagItemStore;
// Game Over - The game over screen after a player is finished playing.
extern TDPTag const TDPTagGameOver;
// Leaderboard - List of leaders in the game.
extern TDPTag const TDPTagLeaderBoard;
// Settings - Screen where player can change settings such as sound.
extern TDPTag const TDPTagSettings ;
// Quit - Screen displayed right before the player exits a game.
extern TDPTag const TDPTagQuit;

@interface TDPlacement : NSObject <NSCopying>

@property (nonatomic, readonly) TDAdTypes adTypes;
@property (nonatomic, readonly) NSString *tag;

+ (instancetype)defaultPlacementForAdTypes:(TDAdTypes)adTypes;
+ (instancetype)placementWithAdTypes:(TDAdTypes)adTypes tag:(TDPTag)tag;

- (instancetype)initWithAdTypes:(TDAdTypes)adTypes forTag:(TDPTag)tag;
- (instancetype)initWithDefaultPlacementForAdTypes:(TDAdTypes)adTypes;

@end

@interface TDPlacement (NativeAds)
+ (instancetype)defaultPlacementForNativeAdType:(TDNativeAdType)nativeAdType;
+ (instancetype)placementWithNativeAdType:(TDNativeAdType)nativeAdType tag:(TDPTag)tag;

- (instancetype)initWithNativeAdType:(TDNativeAdType)nativeAdType forTag:(TDPTag)tag;
- (instancetype)initWithDefaultPlacementForNativeAdType:(TDNativeAdType)nativeAdType;
@end
