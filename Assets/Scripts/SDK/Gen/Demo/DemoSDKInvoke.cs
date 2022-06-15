namespace SDK.Core {
    
    
    public partial class SDKManager {
        
        public void InitDemo() {
            this.registerSDKProxy(1, new SDK.Impl.DemoSDK());
        }
        
        public void DemoInit() {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).Init();
        }
        
        public void DemoLogin() {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).Login();
        }
        
        public void DemoStartHeartbeat() {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).StartHeartbeat();
        }
        
        public void DemoStopHeartbeat() {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).StopHeartbeat();
        }
        
        public void DemoGetSdkType() {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).GetSdkType();
        }
        
        public void DemoGetChannelId() {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).GetChannelId();
        }
        
        public void DemoGetUserInfo() {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).GetUserInfo();
        }
        
        public void DemoShowAgreementWithLicence() {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).ShowAgreementWithLicence();
        }
        
        public void DemoShowAgreementWithPrivacy() {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).ShowAgreementWithPrivacy();
        }
        
        public void DemoReqDeviceId() {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).ReqDeviceId();
        }
        
        public void DemoLogout() {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).Logout();
        }
        
        public void DemoNotifyZone(string roleId, string roleName) {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).NotifyZone(roleId, roleName);
        }
        
        public void DemoCreateRole(string roleName, string roleId) {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).CreateRole(roleName, roleId);
        }
        
        public void DemoPay(string outTradeNo, int money, int gameMoney, string itemName, string itemDesc, string extInfo, string notifyUrl, string orderSign) {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).Pay(outTradeNo, money, gameMoney, itemName, itemDesc, extInfo, notifyUrl, orderSign);
        }
        
        public void DemoCloseAccount(string roleJson) {
            ((SDK.Impl.DemoSDK)(this.getSDKProxy(1))).CloseAccount(roleJson);
        }
    }
}
