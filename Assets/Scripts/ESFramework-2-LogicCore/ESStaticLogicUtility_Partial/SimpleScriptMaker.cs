using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ES
{
    public static partial class ESStaticLogicUtility
    {

        public class SimpleScriptMaker
        {
            public static string defaultCodeClassName = "NewCSharp";
            public static string defaultSavePath = "Assets/Scripts/ESFramework/CodeGen/Default";

            public static string CreateScriptBounds(string Folderpath, string fileName,string using_="", string nameSpace = "ES",string content="",bool TruelyCreate=false)
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    Debug.LogError("文件名不可为空!");
                    return "";
                }

                string fullPath = Path.Combine(Folderpath, fileName);

                // 基础脚本模板
                string scriptContent =
$@"{using_}
    namespace {nameSpace}{{   
             {content}
    }}
";
                scriptContent = scriptContent._ToCode();
                // 创建目录（如果不存在）
                if (TruelyCreate)
                {
#if UNITY_EDITOR
                    if (!AssetDatabase.IsValidFolder(Folderpath))
#endif
                        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                    // 写入文件
                    File.WriteAllText(fullPath, scriptContent, encoding: Encoding.UTF8);
#if UNITY_EDITOR
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
#endif
                        Debug.Log($"创建脚本: {fullPath}");
                }
                return scriptContent;
               
            }

            public static void CreateScriptEasy(string Folderpath, string className, string parent = ":MonoBehaviour", string Attribute = "", string nameSpace = "ES",string AdditonFileName="")
            {
                if (string.IsNullOrEmpty(className))
                {
                    Debug.LogError("类名不可为空!");
                    return;
                }

                string fullPath = Path.Combine(Folderpath, className+AdditonFileName + ".cs");

                // 基础脚本模板
                string scriptContent =
        $@"
using UnityEngine;
    namespace {nameSpace}{{   
        {Attribute}
        public class {HandleString_ToValidName(className).Replace(" ", "")} {parent}
        {{
    
        }}
    }}
";
                scriptContent = scriptContent._ToCode();
                // 创建目录（如果不存在）
#if UNITY_EDITOR
                if (!AssetDatabase.IsValidFolder(Folderpath))
#endif
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // 写入文件
                File.WriteAllText(fullPath, scriptContent, encoding: Encoding.UTF8);

                Debug.Log($"创建脚本: {fullPath}");
            }
            public static void CreateScriptNormal(string Folderpath, string className, string beforeClassName = "", string insideClass = "", string parent = ":MonoBehaviour", string Attribute = "", string nameSpace = "ES", string using_ = "", string AdditonFileName = "")
            {
                if (string.IsNullOrEmpty(className))
                {
                    Debug.LogError("类名不可为空!");
                    return;
                }

                string fullPath = Path.Combine(Folderpath, HandleString_ToValidName(className).Replace(" ", "")+AdditonFileName + ".cs");

                // 基础脚本模板
                string scriptContent =
        $@"
using UnityEngine;
{using_}
    namespace {nameSpace}{{   
        {Attribute}
        public {beforeClassName}  class {className} {parent}
        {{
            {insideClass}
        }}
    }}
";
                scriptContent = scriptContent._ToCode();
                // 创建目录（如果不存在）
#if UNITY_EDITOR
                if (!AssetDatabase.IsValidFolder(Folderpath))
#endif
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // 写入文件
                File.WriteAllText(fullPath, scriptContent, encoding: Encoding.UTF8);

                Debug.Log($"创建脚本: {fullPath}");

#if UNITY_EDITOR
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
#endif
            }
            public static string CreateClassContentByString(string className, string beforeClassName = "", string insideClass = "", string parent = ":MonoBehaviour", string Attribute = "")
            {
                string ClassContent = $@"
                    {Attribute}
                    public {beforeClassName}  class {HandleString_ToValidName(className).Replace(" ", "")} {parent}
                    {{
                        {insideClass}
                    }}
                
";
                return ClassContent;
            }
            public static StringBuilder CreateClassContentByBuilder(string className, string beforeClassName = "", string insideClass = "", string parent = ":MonoBehaviour", string Attribute = "")
            {
                StringBuilder ClassContentBuilder = new StringBuilder($@"
                    {Attribute}
                    public {beforeClassName}  class {HandleString_ToValidName(className).Replace(" ", "")} {parent}
                    {{
                        {insideClass}
                    }}
                
");
                return ClassContentBuilder;
            }
            public static string CreateFieldContent(string typeName, string fieldName, string modifier = "public", string valueDefine = "", string attribute = "", bool nextLine = true)
            {
                string fieldContent =
$@"         {attribute} 
            {modifier} {typeName} {HandleString_ToValidName(fieldName).Replace(" ", "")} {valueDefine};{(nextLine ? "\n" : "")}
";
                return fieldContent;
            }
            public static string CreateParaOrDefineContent(string typeName, string itName, string modifier = "", string valueDefine = "",bool isDefine=true)
            {
                string defineContent =
$@"{(isDefine?"\n"+modifier:"")} {typeName} {HandleString_ToValidName(itName).Replace(" ", "")} {valueDefine+(isDefine ? ";" : "")}";
                return defineContent;
            }
            public static string CreateDicElement(string key, string value)
            {
                return "{" + key + "," + value + "},";
            }

            public static string CreateReturn (string returnWhat="")
            {
                return "            return " + returnWhat+";";
            }
            public static string CreateIf(string condition = "",string inside="")
            {
                string IfContent =
     $@"
                if ({condition})  
                {{
                    {inside}
                }}
                
";
                return IfContent;
            }
            public static string CreateMethod(string methodName, string modifier = "public",
                string back="void",string betweenNameAndParams="", string betweenParamsAndInside = ""
                ,string para="", string insideClass = "", string Attribute = "",string last="")
            {
                string scriptContent =
     $@"
                {Attribute}
                 {modifier}  {back} {methodName} {betweenNameAndParams} ({para}){betweenParamsAndInside}
                {{
                        {insideClass}
                        {last}
                }}
                
";
                return scriptContent;
            }
            public static string CreateNotes(string contont)
            {
                return "/*\n " + contont + "\n*/";
            }
            public static string HandleString_RemoveExtension(string fieldName)
            {
                int thePoint = fieldName.LastIndexOf('.');
                if (thePoint >= 0) return fieldName.Substring(0, thePoint);
                return fieldName;
            }
            public static string HandleString_ToValidName(string input)
            {
                if (string.IsNullOrEmpty(input))
                    return "_";

                // 步骤1：替换所有非字母、数字、下划线的字符为下划线[6,7](@ref)
                string normalized = Regex.Replace(input, @"[\W]", "_");

                // 步骤2：处理开头数字（添加下划线前缀）[3,4](@ref)
                if (char.IsDigit(normalized[0]))
                    normalized = "_" + normalized;

                // 步骤3：合并连续下划线[6](@ref)
                normalized = Regex.Replace(normalized, @"_{2,}", "_");

                // 步骤4：处理C#关键字冲突（添加@前缀）[3,10](@ref)
                if (IsCSharpKeyword(normalized))
                    return "@" + normalized;

                return normalized;
            }
            private static bool IsCSharpKeyword(string value)
            {
                string[] keywords = {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch",
            "char", "checked", "class", "const", "continue", "decimal", "default",
            "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
            "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit",
            "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new",
            "null", "object", "operator", "out", "override", "params", "private", "protected",
            "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof",
            "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true",
            "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using",
            "virtual", "void", "volatile", "while"
        };
                return Array.Exists(keywords, k => k.Equals(value, StringComparison.Ordinal));
            }
        }
    }
}

