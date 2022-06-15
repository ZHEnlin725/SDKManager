using System;

namespace SDK.Core.Attributes
{
    /// <summary>
    /// 标记SDKProxy中需要主动调用的方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SDKInvokeAttribute : Attribute
    {
    }
}