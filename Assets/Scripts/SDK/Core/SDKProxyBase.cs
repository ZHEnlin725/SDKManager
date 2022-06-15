#if UNITY_IOS
using System.Reflection;
#endif
using UnityEngine;

namespace SDK.Core
{
    public abstract class SDKProxyBase
    {
#if UNITY_ANDROID
        #region Android

        /// <summary>
        /// 获取Java侧封装好sdk接口的类的类路径
        /// </summary>
        /// <returns></returns>
        protected abstract string getJavaClasspath();

        #endregion

#endif

        /// <summary>
        /// 调用java static method直接写java侧对应方法得名字
        /// 调用Object-C 由于是通过C#代码直接调用 那么method就写C#这边的方法名
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        internal void CallSdkAPI(string method, params object[] args)
        {
#if UNITY_ANDROID
            using (var javaClass = new AndroidJavaClass(getJavaClasspath()))
            {
                javaClass.CallStatic(method, args);
            }

#elif UNITY_IOS
            var methodInfo = this.GetType().GetMethod(method, BindingFlags.Static |
                                                              BindingFlags.Instance |
                                                              BindingFlags.Public |
                                                              BindingFlags.NonPublic);
            if (methodInfo != null)
            {
                methodInfo.Invoke(this, args);
            }
            else
            {
                Debug.LogError($"Can not find method {method} !!!");
            }
#endif
        }

        /// <summary>
        /// exp:
        /// switch(messageId)
        /// {
        ///    case 1:Callback1(json);
        ///        break;
        ///    case 2:Callback2(json);
        ///        break;
        ///    case 3:Callback3(json);
        ///        break;
        /// }
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="json"></param>
        internal virtual void HandleMessage(int messageId, string json)
        {
        }
    }
}