//
//  FXSDK.h
//  FX
//
//  Created by FXMVP on 2021/9/26.
//

#import <Foundation/Foundation.h>
#import "FXEventConstants.h"

#define FXSDKVersion @"1.0.0"


NS_ASSUME_NONNULL_BEGIN

@interface FXSDK : NSObject



+ (nonnull instancetype)sharedInstance;

- (void)setAppID:(nonnull NSString *)appId;

- (void)setDebugModel:(BOOL)isDebug;

- (void)track:(NSString *)eventName withProperties:(NSDictionary *)properties;

- (void)trackIAPWithAttributes:(FXEvent_IAP_Attribute *)attribute;


@end

NS_ASSUME_NONNULL_END
