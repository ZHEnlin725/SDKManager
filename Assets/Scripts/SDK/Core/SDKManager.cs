using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace SDK.Core
{
    /// <summary>
    /// 专门接收第三方sdk消息
    /// </summary>
    public partial class SDKManager : MonoBehaviour
    {
        private static SDKManager _inst;

        public static SDKManager sharedInst
        {
            get
            {
                if (_inst == null)
                {
                    _inst = GameObject.FindObjectOfType<SDKManager>();
                    if (_inst == null)
                    {
                        var go = new GameObject("SDKManager");
                        _inst = go.AddComponent<SDKManager>();
                    }

                    DontDestroyOnLoad(_inst.gameObject);
                }

                return _inst;
            }
        }

        private const string SDKIdKey = "sdkId";
        private const string MessageKey = "message";

        private const string MessageIdKey = "id";
        private const string MessageContentKey = "content";

        private readonly Dictionary<int, SDKProxyBase> _sdkDict = new Dictionary<int, SDKProxyBase>();

        public void CallSdkAPI(int id, string method, params object[] args)
        {
            if (_sdkDict.TryGetValue(id, out var sdkProxy))
                sdkProxy.CallSdkAPI(method, args);
        }

        /**
         * 由于UnitySendMessage([string] className,[string] methodName, [string] arg)只能传三个string参数
         * 第三个参数应封装成如下格式:
         * {
         *     ["sdkId"]:1, 用于区分用于接收消息的sdkProxy
         *     ["message"]:{
         *        "id":1, 用于区分做出对应处理的方法
         *        "content":{}|[]|""
         *     }
         * }
         * 外部调用 UnitySendMessage("SDKManager","Dispatch",[序列化好的json串])
         */
        public void Dispatch(string json)
        {
            JsonData jsonData = null;
            try
            {
                jsonData = JsonMapper.ToObject(json);
            }
            catch (Exception e)
            {
                // ignored
            }

            if (jsonData == null)
            {
                return;
            }

            var message = jsonData[MessageKey];

            var messageId = (int) message[MessageIdKey];
            var content = (string) message[MessageContentKey];

            if (jsonData[SDKIdKey] != null &&
                int.TryParse((string) jsonData[SDKIdKey], out var sdkId) &&
                _sdkDict.TryGetValue(sdkId, out var sdkProxy))
            {
                sdkProxy.HandleMessage(messageId, content);
                return;
            }

            foreach (var proxy in _sdkDict.Values)
                proxy.HandleMessage(messageId, content);
        }

        private void registerSDKProxy(int id, SDKProxyBase sdkProxy) => _sdkDict[id] = sdkProxy;

        private SDKProxyBase getSDKProxy(int id)
        {
            _sdkDict.TryGetValue(id, out var sdkProxy);
            return sdkProxy;
        }
    }
}