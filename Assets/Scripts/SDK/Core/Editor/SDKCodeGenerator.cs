using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SDK.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace SDK.Core.Editor
{
    public static class SDKCodeGenerator
    {
        public const string GeneratedScriptsFolder = "Assets/Scripts/SDK/Gen";

        private const string GetSDKProxyMethod = "getSDKProxy";
        private const string RegisterSDKProxyMethod = "registerSDKProxy";

        public static void GenerateSDKInvoke()
        {
            EnsureGeneratedScriptsFolder();
            var assembly = Assembly.GetAssembly(typeof(SDKAttribute));
            var exportedTypes = assembly.GetExportedTypes();

            var types = exportedTypes.Where(o =>
                AttributeFilter<SDKAttribute>(Attribute.GetCustomAttributes(o))).ToArray();
            if (Duplicated(types)) return;
            foreach (var type in types)
            {
                var methodInfos = type.GetMethods(BindingFlags.Public |
                                                  BindingFlags.NonPublic |
                                                  BindingFlags.Instance)
                    .Where(o
                        => AttributeFilter<SDKInvokeAttribute>(Attribute
                            .GetCustomAttributes(o)))
                    .ToList();
                GenerateSDKInvoke(type, methodInfos);
            }

            AssetDatabase.Refresh();
        }

        public static void GenerateSDKHandleMessage()
        {
            EnsureGeneratedScriptsFolder();

            var assembly = Assembly.GetAssembly(typeof(SDKAttribute));
            var exportedTypes = assembly.GetExportedTypes();

            var types = exportedTypes.Where(o =>
                AttributeFilter<SDKAttribute>(Attribute.GetCustomAttributes(o))).ToArray();
            if (Duplicated(types)) return;
            foreach (var type in types)
            {
                var methodInfos = type.GetMethods(BindingFlags.Public |
                                                  BindingFlags.NonPublic |
                                                  BindingFlags.Instance)
                    .Where(o
                        => AttributeFilter<SDKCallbackAttribute>(Attribute
                            .GetCustomAttributes(o)))
                    .ToList();
                GenerateSDKHandleMessage(type, methodInfos);
            }

            AssetDatabase.Refresh();
        }

        private static void GenerateSDKInvoke(Type classType, List<MethodInfo> methodInfoList)
        {
            var provider = CodeDomProvider.CreateProvider("c#");
            var codeNamespace = new CodeNamespace("SDK.Core");
            var classname = classType.Name;
            var codeTypeDeclaration = new CodeTypeDeclaration("SDKManager") {IsPartial = true};

            var sdkAttribute = classType.GetCustomAttribute<SDKAttribute>();
            var name = string.IsNullOrEmpty(sdkAttribute?.name) ? classname : sdkAttribute.name;

            var codeMemberMethod = new CodeMemberMethod
            {
                Name = $"Init{name}",
                Attributes = MemberAttributes.Public |
                             MemberAttributes.Final,
            };

            codeMemberMethod.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(),
                    RegisterSDKProxyMethod, 
                    new CodePrimitiveExpression(sdkAttribute.id),
                    new CodeObjectCreateExpression(classType)));

            codeTypeDeclaration.Members.Add(codeMemberMethod);
            foreach (var methodInfo in methodInfoList)
            {
                var memberMethod = new CodeMemberMethod
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Final, Name = $"{name}{methodInfo.Name}"
                };
                var parameterInfos = methodInfo.GetParameters();
                var invokeGetSDKProxy = new CodeMethodInvokeExpression(
                    new CodeThisReferenceExpression(),
                    GetSDKProxyMethod, 
                    new CodePrimitiveExpression(sdkAttribute.id)
                );
                var methodInvokeExpression = new CodeMethodInvokeExpression(
                    new CodeCastExpression(classType,
                    invokeGetSDKProxy), 
                    methodInfo.Name);
                foreach (var parameterInfo in parameterInfos)
                {
                    memberMethod.Parameters.Add(
                        new CodeParameterDeclarationExpression(parameterInfo.ParameterType, parameterInfo.Name));
                    methodInvokeExpression.Parameters.Add(new CodeArgumentReferenceExpression(parameterInfo.Name));
                }

                var codeExpressionStatement = new CodeExpressionStatement {Expression = methodInvokeExpression};
                memberMethod.Statements.Add(codeExpressionStatement);
                codeTypeDeclaration.Members.Add(memberMethod);
            }

            codeNamespace.Types.Add(codeTypeDeclaration);

            var folderPath = Path.Combine(GeneratedScriptsFolder, name);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            var scriptPath = Path.Combine(folderPath, $"{classname}Invoke.cs");
            if (File.Exists(scriptPath)) File.Delete(scriptPath);
            using (var writer = new StreamWriter(scriptPath, false, Encoding.UTF8))
                provider.GenerateCodeFromNamespace(codeNamespace, writer, null);
        }

        private static void GenerateSDKHandleMessage(Type classType, List<MethodInfo> methodInfoList)
        {
            var provider = CodeDomProvider.CreateProvider("c#");
            var codeNamespace = new CodeNamespace(classType.Namespace);

            var classname = classType.Name;

            var codeTypeDeclaration = new CodeTypeDeclaration(classType.Name) {IsPartial = true};
            var codeMemberMethod = new CodeMemberMethod {Name = "HandleMessage"};
            const string arg_json = "json";
            const string arg_messageId = "messageId";
            codeMemberMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(int)), arg_messageId));
            codeMemberMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(string)), arg_json));
            codeMemberMethod.Attributes = MemberAttributes.Assembly | MemberAttributes.Override;

            var methodIdDict = new Dictionary<int, List<MethodInfo>>();
            methodInfoList.Sort((x, y)
                => x.GetCustomAttribute<SDKCallbackAttribute>().messageId -
                   y.GetCustomAttribute<SDKCallbackAttribute>().messageId);
            foreach (var methodInfo in methodInfoList)
            {
                var messageId = methodInfo.GetCustomAttribute<SDKCallbackAttribute>().messageId;
                if (!methodIdDict.TryGetValue(messageId,
                    out var list)) methodIdDict.Add(messageId, list = new List<MethodInfo>());
                list.Add(methodInfo);
            }

            CodeConditionStatement lastCodeConditionStatement = null;

            foreach (var pair in methodIdDict)
            {
                var codeConditionStatement = new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeArgumentReferenceExpression(arg_messageId),
                        CodeBinaryOperatorType.IdentityEquality,
                        new CodePrimitiveExpression(pair.Key)));
                foreach (var methodInfo in pair.Value)
                {
                    var handMessageInvoke =
                        new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), methodInfo.Name);
                    if (methodInfo.GetParameters().Length == 1)
                        handMessageInvoke.Parameters.Add(new CodeArgumentReferenceExpression(arg_json));
                    codeConditionStatement.TrueStatements.Add(new CodeExpressionStatement(handMessageInvoke));
                }

                if (lastCodeConditionStatement != null)
                {
                    lastCodeConditionStatement.FalseStatements.Add(codeConditionStatement);
                }
                else
                {
                    codeMemberMethod.Statements.Add(codeConditionStatement);
                }

                lastCodeConditionStatement = codeConditionStatement;
            }

            codeTypeDeclaration.Members.Add(codeMemberMethod);

            codeNamespace.Types.Add(codeTypeDeclaration);

            var sdkAttribute = classType.GetCustomAttribute<SDKAttribute>();
            var folderName = string.IsNullOrEmpty(sdkAttribute?.name) ? classname : sdkAttribute.name;
            var folderPath = Path.Combine(GeneratedScriptsFolder, folderName);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            var scriptPath = Path.Combine(folderPath, $"{classname}HandleMessage.cs");
            if (File.Exists(scriptPath)) File.Delete(scriptPath);
            using (var writer = new StreamWriter(scriptPath, false, Encoding.UTF8))
                provider.GenerateCodeFromNamespace(codeNamespace, writer, null);
        }

        private static bool AttributeFilter<T>(IEnumerable<Attribute> attributes) where T : Attribute =>
            attributes.OfType<T>().Any();

        private static void EnsureGeneratedScriptsFolder()
        {
            if (!Directory.Exists(GeneratedScriptsFolder))
                Directory.CreateDirectory(GeneratedScriptsFolder);
        }

        private static bool Duplicated(IEnumerable<Type> types)
        {
            var distinct = new Dictionary<int, List<Type>>();
            foreach (var type in types)
            {
                var sdkAttribute = type.GetCustomAttribute<SDKAttribute>();
                if (!distinct.TryGetValue(sdkAttribute.id, out var list))
                    distinct.Add(sdkAttribute.id, list = new List<Type>());
                list.Add(type);
            }

            foreach (var pair in distinct)
            {
                if (pair.Value.Count > 1)
                {
                    Debug.LogError($"Failed to generate SDK code, duplicate SDK Id [{pair.Key}] ! ! !");
                    return true;
                }
            }

            return false;
        }
    }
}