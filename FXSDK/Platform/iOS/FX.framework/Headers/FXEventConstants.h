//
//  FXEventConstants.h
//  FX
//
//  Created by FXMVP on 2021/10/7.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN


extern NSString * const FXEventConstant_IAP_Name;
extern NSString * const FXEventConstant_IAP_Count;
extern NSString * const FXEventConstant_IAP_Amount;
extern NSString * const FXEventConstant_IAP_Currency;
extern NSString * const FXEventConstant_IAP_Paystatus;
extern NSString * const FXEventConstant_IAP_TransactionId;
extern NSString * const FXEventConstant_IAP_FailReason;
extern NSString * const FXEventConstant_IAP_Items;


typedef NS_ENUM(NSInteger, FXEventConstant_IAP_PayStatus) {

    FXEventConstant_IAP_PayStatus_none = 0,
    FXEventConstant_IAP_PayStatus_success = 1,
    FXEventConstant_IAP_PayStatus_fail = 2,
    FXEventConstant_IAP_PayStatus_restored = 3
};




@interface FXEvent_IAP_Attribute_Item : NSObject

@property (nonatomic,copy)NSString *name;
@property (nonatomic,assign)NSInteger count;

@end

@interface FXEvent_IAP_Attribute : NSObject

@property (nonatomic,strong) NSArray<FXEvent_IAP_Attribute_Item *>  *items;
@property (nonatomic,copy)NSString *currency;
@property (nonatomic,assign)double amount;
@property (nonatomic,assign)FXEventConstant_IAP_PayStatus paystatus;
@property (nonatomic,copy)NSString *transaction_id;
@property (nonatomic,copy)NSString *fail_reason;


@end
@interface FXEventConstants : NSObject

@end

NS_ASSUME_NONNULL_END
