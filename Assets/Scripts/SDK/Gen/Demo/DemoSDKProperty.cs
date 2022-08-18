// ********************************
//        自动生成请勿修改
//DateTime:2022-08-18 15:05:18
//********************************
namespace SDK.Core {
    
    
    public partial class SDKManager {
        
        public SDK.Impl.DemoSDK Demo {
            get {
                SDK.Core.SDKProxyBase sdkProxy = this.getSDKProxy(1);
                if ((sdkProxy == null)) {
                    sdkProxy = new SDK.Impl.DemoSDK();
                    this.registerSDKProxy(1, sdkProxy);
                }
                return ((SDK.Impl.DemoSDK)(sdkProxy));
            }
        }
    }
}
