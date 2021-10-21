//
//  FXSDK.h
//  FX
//
//  Created by Mintegral on 2021/9/26.
//

#import <Foundation/Foundation.h>



extern "C" {
void setAppID2(const char *appId);

void setDebugModel(bool isDebug);

void track(const char *eventName, const char *attributes);

void trackIAPWithAttributes(const char *IAPAttribute);
    
}
    

NS_ASSUME_NONNULL_END
