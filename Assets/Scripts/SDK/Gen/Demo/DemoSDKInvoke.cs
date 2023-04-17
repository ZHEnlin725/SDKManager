// ********************************
//        自动生成请勿修改
//DateTime:2023-04-17 10:50:23
//********************************
namespace SDK.Core {
    
    
    public partial class SDKManager {
        
        public SDK.Impl.DemoSDK demo {
            get {
                SDK.Core.SDKProxyBase sdkProxy = this.getSDKProxy(1);
                if ((sdkProxy == null)) {
                    sdkProxy = new SDK.Impl.DemoSDK();
                    this.registerSDKProxy(1, sdkProxy);
                }
                return ((SDK.Impl.DemoSDK)(sdkProxy));
            }
        }
        
        public void Demo_Init() {
            this.demo.Init();
        }
        
        public void Demo_Login() {
            this.demo.Login();
        }
        
        public void Demo_StartHeartbeat() {
            this.demo.StartHeartbeat();
        }
        
        public void Demo_StopHeartbeat() {
            this.demo.StopHeartbeat();
        }
        
        public void Demo_GetSdkType() {
            this.demo.GetSdkType();
        }
        
        public void Demo_GetChannelId() {
            this.demo.GetChannelId();
        }
        
        public void Demo_GetUserInfo() {
            this.demo.GetUserInfo();
        }
        
        public void Demo_ShowAgreementWithLicence() {
            this.demo.ShowAgreementWithLicence();
        }
        
        public void Demo_ShowAgreementWithPrivacy() {
            this.demo.ShowAgreementWithPrivacy();
        }
        
        public void Demo_ReqDeviceId() {
            this.demo.ReqDeviceId();
        }
        
        public void Demo_Logout() {
            this.demo.Logout();
        }
        
        public void Demo_NotifyZone(string roleId, string roleName) {
            this.demo.NotifyZone(roleId, roleName);
        }
        
        public void Demo_CreateRole(string roleName, string roleId) {
            this.demo.CreateRole(roleName, roleId);
        }
        
        public void Demo_Pay(string outTradeNo, int money, int gameMoney, string itemName, string itemDesc, string extInfo, string notifyUrl, string orderSign) {
            this.demo.Pay(outTradeNo, money, gameMoney, itemName, itemDesc, extInfo, notifyUrl, orderSign);
        }
        
        public void Demo_CloseAccount(string roleJson) {
            this.demo.CloseAccount(roleJson);
        }
    }
}
