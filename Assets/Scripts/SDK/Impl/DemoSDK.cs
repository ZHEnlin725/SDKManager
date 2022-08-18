using System;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif
using System.Runtime.InteropServices;
using SDK.Core;
using SDK.Core.Attributes;
using UnityEngine;

namespace SDK.Impl
{
    [SDK(1, "Demo")]
    public partial class DemoSDK : SDKProxyBase
    {
        private const int OnInitCallback = 1;
        private const int OnLoginCallback = 2;
        private const int OnLogoutCallback = 3;
        private const int OnPayCallback = 4;
        private const int OnDeviceIdCallback = 5;
        private const int OnSdkTypeCallback = 6;
        private const int OnChannelIdCallback = 7;
        private const int OnUserInfoCallback = 8;
        private const int OnLicencedCallback = 9;
        private const int OnPrivacyCallback = 10;
        private const int OnCloseAccountCallback = 11;

        public Action<string> LoginLuaCallback;
        public Action<string> PayLuaCallback;
        public Action<string> DeviceIdLuaCallback;
        public Action<string> SDKTypeLuaCallback;
        public Action<string> ChannelIdLuaCallback;
        public Action<string> UserInfoLuaCallback;
        public Action<string> LicencedLuaCallback;
        public Action<string> PrivacyLuaCallback;
        public Action<string> CloseAccountLuaCallback;

        private bool _init;

        private string _deviceId;

        public string deviceId => _deviceId;

        private string _channelId;

        public string channelId => _channelId;

#if UNITY_ANDROID
        string _merchantId = "534";
        string _appId = "7266";
        string _serverId = "6258";
        string _appKey = "63q82y5aebb46172sbgcd178cbc9627r";

        string _serverName = "TEST";

#elif UNITY_IOS
        string _merchantId = "534";
        string _appId = "7645";
        string _serverId = "224";
        string _appKey = "7d4093ed16b4652819ee42c8887dc70e";
        string _serverName = "TEST";

        [DllImport("__Internal")]
        private static extern void init(string merchantId, string appId, string serverId, string appKey);

        [DllImport("__Internal")]
        private static extern void login();

        [DllImport("__Internal")]
        private static extern void reqDeviceId();

        [DllImport("__Internal")]
        private static extern void logout();

        [DllImport("__Internal")]
        private static extern void notifyZone(string serverId, string serverName, string roleId, string roleName);

        [DllImport("__Internal")]
        private static extern void createRole(string roleName, string roleId);

        [DllImport("__Internal")]
        private static extern void pay(string outTradeNo, int money, int gameMoney, string itemName, string itemDesc,
            string extInfo, string notifyUrl, string orderSign);
#endif

        [SDKInvoke]
        public void Init()
        {
            if (_init) return;
            _init = true;
            CallSdkAPI("init", _merchantId, _appId, _serverId, _appKey);
        }

        [SDKInvoke]
        public void Login()
        {
            CallSdkAPI("login");
        }

        [SDKInvoke]
        public void StartHeartbeat()
        {
            CallSdkAPI("startHeartbeat");
        }

        [SDKInvoke]
        public void StopHeartbeat()
        {
            CallSdkAPI("stopHeartbeat");
        }

        [SDKInvoke]
        public void GetSdkType()
        {
            CallSdkAPI("getSdkType");
        }

        [SDKInvoke]
        public void GetChannelId()
        {
            CallSdkAPI("getChannelId");
        }

        [SDKInvoke]
        public void GetUserInfo()
        {
            CallSdkAPI("getUserInfo");
        }

        [SDKInvoke]
        public void ShowAgreementWithLicence()
        {
            CallSdkAPI("showAgreementWithLicence");
        }

        [SDKInvoke]
        public void ShowAgreementWithPrivacy()
        {
            CallSdkAPI("showAgreementWithPrivacy");
        }

        [SDKInvoke]
        public void ReqDeviceId()
        {
            CallSdkAPI("reqDeviceId");
        }

        [SDKInvoke]
        public void Logout()
        {
            CallSdkAPI("logout");
        }

        [SDKInvoke]
        public void NotifyZone(string roleId, string roleName)
        {
            CallSdkAPI("notifyZone", _serverId, _serverName, roleId, roleName);
        }

        [SDKInvoke]
        public void CreateRole(string roleName, string roleId)
        {
            CallSdkAPI("createRole", roleName, roleId);
        }

        [SDKInvoke]
        public void Pay(string outTradeNo, int money, int gameMoney, string itemName, string itemDesc, string extInfo,
            string notifyUrl, string orderSign)
        {
            CallSdkAPI("pay", outTradeNo, money, gameMoney, itemName, itemDesc, extInfo, notifyUrl, orderSign);
        }

        [SDKInvoke]
        public void CloseAccount(string roleJson)
        {
            CallSdkAPI("closeAccount", roleJson);
        }

        [SDKCallback(messageId = OnInitCallback)]
        public void InitCallback(string jsonstr)
        {
        }

        [SDKCallback(messageId = OnLoginCallback)]
        public void LoginCallback(string jsonstr)
        {
            if (LoginLuaCallback != null)
                LoginLuaCallback(jsonstr);
        }

        [SDKCallback(messageId = OnPayCallback)]
        public void PayCallback(string jsonstr)
        {
            if (PayLuaCallback != null)
                PayLuaCallback(jsonstr);
        }

        [SDKCallback(messageId = OnCloseAccountCallback)]
        public void CloseAccountCallback(string jsonstr)
        {
            if (CloseAccountLuaCallback != null)
                CloseAccountLuaCallback(jsonstr);
        }

        [SDKCallback(messageId = OnLogoutCallback)]
        public void GameExit()
        {
            //退出游戏
            Application.Quit();
        }

        [SDKCallback(messageId = OnDeviceIdCallback)]
        public void GotDeviceId(string id)
        {
            _deviceId = id;
            if (DeviceIdLuaCallback != null)
                DeviceIdLuaCallback(id);
        }

        [SDKCallback(messageId = OnSdkTypeCallback)]
        public void GotSDKType(string sdkType)
        {
            if (SDKTypeLuaCallback != null)
                SDKTypeLuaCallback(sdkType);
        }

        [SDKCallback(messageId = OnChannelIdCallback)]
        public void GotChannelId(string channelId)
        {
            _channelId = channelId;
            if (ChannelIdLuaCallback != null)
                ChannelIdLuaCallback(channelId);
        }

        [SDKCallback(messageId = OnUserInfoCallback)]
        public void GotUserInfo(string userInfo)
        {
            if (UserInfoLuaCallback != null)
                UserInfoLuaCallback(userInfo);
        }

        [SDKCallback(messageId = OnPrivacyCallback)]
        public void ShowedAPrivacy(string status)
        {
            if (PrivacyLuaCallback != null)
                PrivacyLuaCallback(status);
        }

        [SDKCallback(messageId = OnLicencedCallback)]
        public void ShowedLicence(string status)
        {
            if (LicencedLuaCallback != null)
                LicencedLuaCallback(status);
        }

#if UNITY_ANDROID
        protected override string getJavaClasspath() => "com.xxx.xxx.android.MainActivity";
#endif
    }
}