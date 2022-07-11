using System;

namespace SDK.Core.Attributes
{
    /// <summary>
    /// 标记SDKProxy中处理消息的回调方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SDKCallbackAttribute : Attribute
    {
        /// <summary>
        /// 区分回调用于处理哪个Message
        /// </summary>
        public int messageId;
    }
}