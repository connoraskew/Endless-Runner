//
//  TDError.h
//  Tapdaq iOS SDK
//
//  Created by Dmitry Dovgoshliubnyi on 27/07/2017.
//  Copyright © 2017 Tapdaq. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "TDMNetworkEnum.h"

extern NSString *const TDErrorDomain;

@interface TDError : NSError
@property (strong, nonatomic) NSDictionary<TDMNetwork, NSError *> *subErrors;
@end
