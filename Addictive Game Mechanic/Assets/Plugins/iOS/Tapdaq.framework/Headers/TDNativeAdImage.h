//
//  TDNativeAdImage.h
//  Tapdaq
//
//  Created by Dmitry Dovgoshliubnyi on 14/03/2018.
//  Copyright © 2018 Tapdaq. All rights reserved.
//

#import <UIKit/UIKit.h>

typedef void(^TDNativeAdImageCompletion)(UIImage * _Nullable image);

@interface TDNativeAdImage : NSObject
@property (strong, nonatomic, nullable) NSURL *imageUrl;

- (instancetype _Nullable)initWithImageURL:(NSURL * _Nullable)url;
- (void)getAsyncImage:(TDNativeAdImageCompletion _Nullable)completion;
@end
