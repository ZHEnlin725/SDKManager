using System.IO;
using UnityEditor;

namespace SDK.Core.Editor
{
    public static class SDKCodeGenerateMenuItems
    {
        [MenuItem("SDK Code Generate/生成SDK相关代码", priority = 0)]
        private static void GenerateSDKCode()
        {
            SDKCodeGenerator.GenerateSDKProperty();
            SDKCodeGenerator.GenerateSDKHandleMessage();
        }
        
        [MenuItem("SDK Code Generate/生成SDK实例调用代码", priority = 9)]
        private static void GenerateSDKProperty() =>
            SDKCodeGenerator.GenerateSDKProperty();

        // [MenuItem("SDK Code Generate/生成SDK调用代码", priority = 10)]
        // private static void GenerateSDKInvoke() =>
        //     SDKCodeGenerator.GenerateSDKInvoke();

        [MenuItem("SDK Code Generate/生成SDK处理消息代码", priority = 11)]
        private static void GenerateSDKHandleMessage() =>
            SDKCodeGenerator.GenerateSDKHandleMessage();

        [MenuItem("SDK Code Generate/清除生成的SDK代码", priority = 12)]
        private static void DeleteGeneratedCode()
        {
            if (Directory.Exists(SDKCodeGenerator.GeneratedScriptsFolder))
                Directory.Delete(SDKCodeGenerator.GeneratedScriptsFolder, true);
        }
    }
}