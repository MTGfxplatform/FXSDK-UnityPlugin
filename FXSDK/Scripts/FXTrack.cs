using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Runtime.InteropServices;

using FXTrack.ThirdParty.LitJson;


namespace FXTrack
{

    [Serializable]
    public struct ProductItem
    {
        public string name { get; set; }
        public int count { get; set; }
    };

    [Serializable]
    public enum FXEventConstant_IAP_PayStatus
    {
        FXEventConstant_IAP_PayStatus_none = 0,
        FXEventConstant_IAP_PayStatus_success = 1,
        FXEventConstant_IAP_PayStatus_fail = 2,
        FXEventConstant_IAP_PayStatus_restored = 3      
    };


    [Serializable]
    public struct ProductsAttributes
    {
        public double amount { get; set; }
        public string currency { get; set; }
        public string transaction_id { get; set; }
        public string name { get; set; }
        public string fail_reason { get; set; }
        public FXEventConstant_IAP_PayStatus paystatus { get; set; }

    }

    public partial class Analytics
    {

        private static readonly string FXEventConstant_IAP_Name                  = "name";
        private static readonly string FXEventConstant_IAP_Count                 = "count";
        private static readonly string FXEventConstant_IAP_Amount                = "amount";
        private static readonly string FXEventConstant_IAP_Currency              = "currency";
        private static readonly string FXEventConstant_IAP_Paystatus             = "paystatus";
        private static readonly string FXEventConstant_IAP_TransactionId         = "transaction_id";
        private static readonly string FXEventConstant_IAP_FailReason            = "fail_reason";
        private static readonly string FXEventConstant_IAP_Items                 = "items";


        [Serializable]
        public class FXIAPEventAttribute
        {
            public List<Dictionary<string,string>> items;
            public ProductsAttributes attributes;
        }


#if UNITY_ANDROID

        protected static AndroidJavaClass FXAndroidSDK = new AndroidJavaClass("com.fxmvp.detailroi.unity.bridge.UnityAndroidAlphaSDKManager");
        protected static AndroidJavaObject FXAndroidSDKObject = new AndroidJavaObject("com.fxmvp.detailroi.unity.bridge.UnityAndroidAlphaSDKManager");

        protected static AndroidJavaObject Context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

#endif

        private const string FXTrackPluginVersion = "1.0";




        // 
        /// <summary>
        /// 开始FXTrack统计 发送策略为实时发送
        /// </summary>
        /// <param name="appKey">FX系统appId</param>
        /// <param name="channelId">渠道名称</param>
        ///
        ///

        public static void StartWithAppIdAndChannelId(string appId, string channelId = null)
        {

            initSDKWithAppIdAndChannelId(appId, channelId);
        }

        public static void SetChannel(string channelId)
        {

            setAppChannel(channelId);
        }

        public static void SetDebugLog(bool isDebug)
        {

            debugLog(isDebug);
        }

        public static void TrackIAP(List<ProductItem> items, ProductsAttributes attributes)
        {

            ReportIAPEvent(items,attributes);
        }


        public static void Track(string eventName, Dictionary<string, object> attributes)
        {

            ReportEvent(eventName,attributes);
        }

















#region 
        private static void initSDKWithAppIdAndChannelId(string appId, string channelId = null)
        {

            #if UNITY_EDITOR
                        Debug.Log("Unity Editor: StartWithAppIdAndChannelId");


            #elif UNITY_ANDROID

                        FXAndroidSDK.CallStatic("initAlphaSDK", Context, channelId,appId);

            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE

                    setAppID(appId);
            #else

            #endif
        }

        private static void setAppChannel(string channelId)
        {

            #if UNITY_EDITOR
                        Debug.Log("Unity Editor: SetChannel");

            #elif UNITY_ANDROID

                        FXAndroidSDK.CallStatic("updateChannel",channelId);

            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
                        //iOS not support  
            #else

            #endif
        }

        private static void debugLog(bool isDebug)
        {
            #if UNITY_EDITOR
                        Debug.Log("Unity Editor: SetDebugLog");

            #elif UNITY_ANDROID

                        FXAndroidSDK.CallStatic("changeDebugState",isDebug);

            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
                        setDebugModel(isDebug);
            #else

            #endif

        }


        private static void ReportIAPEvent(List<ProductItem> items, ProductsAttributes  attributes)
        {


            List<Dictionary<string, string>> productItems = new List<Dictionary<string, string>>();

            foreach (ProductItem item in items)
            {
                if (item.name != null)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add(FXTrack.Analytics.FXEventConstant_IAP_Name, item.name);
                    dict.Add(FXTrack.Analytics.FXEventConstant_IAP_Count, item.count.ToString());

                    productItems.Add(dict);
                }
            }


            Dictionary<string, object> attributesDict = new Dictionary<string, object>();

            attributesDict.Add(FXTrack.Analytics.FXEventConstant_IAP_Items,productItems);
            attributesDict.Add(FXTrack.Analytics.FXEventConstant_IAP_Amount, attributes.amount.ToString());
            int paystatus = (int)attributes.paystatus;
            attributesDict.Add(FXTrack.Analytics.FXEventConstant_IAP_Paystatus, paystatus.ToString());

            if (attributes.currency != null)
            {
                attributesDict.Add(FXTrack.Analytics.FXEventConstant_IAP_Currency, attributes.currency);
            }
            if (attributes.transaction_id != null)
            {
                attributesDict.Add(FXTrack.Analytics.FXEventConstant_IAP_TransactionId, attributes.transaction_id);
            }
            if (attributes.fail_reason != null)
            {
                attributesDict.Add(FXTrack.Analytics.FXEventConstant_IAP_FailReason, attributes.fail_reason);
            }


            string attributesJSONString = JsonMapper.ToJson(attributesDict);

            #if UNITY_EDITOR
                    Debug.Log("Unity Editor: TrackIAP");

            #elif UNITY_ANDROID

                    FXAndroidSDKObject.Call("sendIapEvent",attributesJSONString);

            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE

                    trackIAPWithAttributes(attributesJSONString);
            #else

            #endif


        }




        private static void ReportEvent(string eventName, Dictionary<string, object> attributes)
        {
            string eventDataJSONString = JsonMapper.ToJson(attributes);

            #if UNITY_EDITOR
                    Debug.Log("Unity Editor: Track");


            #elif UNITY_ANDROID

                    FXAndroidSDKObject.Call("sendCustomEvent",eventName,eventDataJSONString);

            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE

                    track(eventName,eventDataJSONString);
            #else

            #endif
        }




        #if UNITY_IPHONE

            [DllImport("__Internal")]
	        private static extern void setAppID(string appId);
		    
		    [DllImport("__Internal")]
		    private static extern void setDebugModel(bool isDebug);
	    
	        [DllImport("__Internal")]
	        private static extern void track(string eventName, string attributes);
	    
	        [DllImport("__Internal")]
	        private static extern void trackIAPWithAttributes(string attributes);

        #endif




#endregion


    }






}