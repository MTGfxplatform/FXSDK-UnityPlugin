//
//  FXAPIEndpoints.h
//  FX
//
//  Created by M1 on 2021/10/9.
//

#import <Foundation/Foundation.h>
#import <FX/FX.h>

extern "C" {
    
    void setAppID(const char *appId){
        
        NSString *_appId = [NSString stringWithUTF8String:appId];
        [[FXSDK sharedInstance] setAppID:_appId];
    }
    

    void setDebugModel(bool isDebug){
        
        [[FXSDK sharedInstance] setDebugModel:isDebug];
    }
    
    void track(const char *eventName, const char *attributes){
        
        NSString *_eventName = [NSString stringWithUTF8String:eventName];
        NSString *_attributes = [NSString stringWithUTF8String:attributes];

        NSData *data = [_attributes dataUsingEncoding:NSUTF8StringEncoding];

        NSError *error = nil;
        NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];

        if (error) {
            NSString *msg = [NSString stringWithFormat:@"%@ is not an invalid JSON data",_attributes];
            NSLog(@"error is :%@",msg);
            return;
        }
        
        [[FXSDK sharedInstance] track:_eventName withProperties:dict];
    }


    void trackIAPWithAttributes(const char *IAPAttribute){
        
        NSString *jsonString = [NSString stringWithUTF8String:IAPAttribute];

        NSData *data = [jsonString dataUsingEncoding:NSUTF8StringEncoding];

        NSError *error = nil;
        NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];

        
        if (error) {
            NSString *msg = [NSString stringWithFormat:@"%@ is not an invalid JSON data",jsonString];
            NSLog(@"error is :%@",msg);
            return;
        }

        
        NSArray *items = [dict objectForKey:@"items"];
        
        NSMutableArray *itemObjects = [NSMutableArray new];
        for (NSDictionary *obj in items) {
            if ([obj isKindOfClass:NSDictionary.class]) {
                FXEvent_IAP_Attribute_Item *item = [[FXEvent_IAP_Attribute_Item alloc] init];
                
                NSString *name = [obj objectForKey:FXEventConstant_IAP_Name];
                NSString *count = [obj objectForKey:FXEventConstant_IAP_Count];
                if ([name isKindOfClass:NSString.class] ) {
                    item.name = name;
                }
                if ([count isKindOfClass:NSString.class]) {
                    item.count = [count integerValue];
                }
                [itemObjects addObject:item];
            }
        }

        NSString *amount = [dict objectForKey:FXEventConstant_IAP_Amount];
        NSString *currency = [dict objectForKey:FXEventConstant_IAP_Currency];
        NSString *paystatus = [dict objectForKey:FXEventConstant_IAP_Paystatus];
        NSString *transaction_id = [dict objectForKey:FXEventConstant_IAP_TransactionId];
        NSString *fail_reason = [dict objectForKey:FXEventConstant_IAP_FailReason];

        
        FXEvent_IAP_Attribute *attribute = [[FXEvent_IAP_Attribute alloc] init];
        attribute.items = itemObjects;
        if ([amount isKindOfClass:NSString.class]) {
            attribute.amount = [amount doubleValue];
        }
        if ([currency isKindOfClass:NSString.class] ) {
            attribute.currency = currency;
        }
        if ([paystatus isKindOfClass:NSString.class] ) {
            attribute.paystatus = (FXEventConstant_IAP_PayStatus)[paystatus integerValue];;
        }
        if ([transaction_id isKindOfClass:NSString.class] ) {
            attribute.transaction_id = transaction_id;
        }
        if ([fail_reason isKindOfClass:NSString.class] ) {
            attribute.fail_reason = fail_reason;
        }

        [[FXSDK sharedInstance] trackIAPWithAttributes:attribute];
    }
    

 
}
