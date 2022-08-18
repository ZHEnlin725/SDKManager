using System;

namespace SDK.Core.Attributes
{
    /// <summary>
    /// 标记需要生成代码的SDK 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SDKAttribute : Attribute
    {
        /// <summary>
        /// 用来区分SDK
        /// </summary>
        public readonly int id;

        public readonly string name;

        public SDKAttribute(int id)
        {
            this.id = id;
        }

        public SDKAttribute(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}