// ********************************
//        自动生成请勿修改
//DateTime:2023-04-17 10:50:23
//********************************
namespace SDK.Impl {
    
    
    public partial class DemoSDK {
        
        internal override void HandleMessage(int messageId, string json) {
            if ((messageId == 1)) {
                this.InitCallback(json);
            }
            else {
                if ((messageId == 2)) {
                    this.LoginCallback(json);
                }
                else {
                    if ((messageId == 3)) {
                        this.GameExit();
                    }
                    else {
                        if ((messageId == 4)) {
                            this.PayCallback(json);
                        }
                        else {
                            if ((messageId == 5)) {
                                this.GotDeviceId(json);
                            }
                            else {
                                if ((messageId == 6)) {
                                    this.GotSDKType(json);
                                }
                                else {
                                    if ((messageId == 7)) {
                                        this.GotChannelId(json);
                                    }
                                    else {
                                        if ((messageId == 8)) {
                                            this.GotUserInfo(json);
                                        }
                                        else {
                                            if ((messageId == 9)) {
                                                this.ShowedLicence(json);
                                            }
                                            else {
                                                if ((messageId == 10)) {
                                                    this.ShowedAPrivacy(json);
                                                }
                                                else {
                                                    if ((messageId == 11)) {
                                                        this.CloseAccountCallback(json);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
