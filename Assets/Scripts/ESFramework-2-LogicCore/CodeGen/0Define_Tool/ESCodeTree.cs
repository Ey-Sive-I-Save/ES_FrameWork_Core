using ES;
using ES.Pointer;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ES.EnumCollect;
namespace ES
{
    #region 基础声明定义
    [Serializable, TypeRegistryItem("ES代码目标生成器")]
    public class ESCodeTreeTarget : ESCodeNode_Standard, IESCodeTreeClass
    {
        #region 文件定义
        [LabelText("文件架路径"), FolderPath]
        public string folderPath = "Assets/Scripts/ESFramework/CodeGen/Default";
        [LabelText("文件名"), InlineButton("UnPreview", "取消预览"), InlineButton("Preview","预览")]
        public string fileName = "_CodeGen";
        [LabelText("添加首个类名在前"), InlineProperty] public bool UseClassNameAtFirst = true;
        [LabelText("添加.cs在后"), InlineProperty] public bool UseExtesionAtLast = true;
        #endregion

        #region 命名空间
        [LabelText("我的命名空间")]
        public string MyNameSpace = "ES";

        [LabelText("最终引用命名空间"), ReadOnly]
        public HashSet<string> EndNameSpaces = new HashSet<string>();

        [LabelText("手写引用命名空间")]
        public List<string> HandNameSpaces = new List<string>() {
        " Sirenix.Serialization"
         ," Sirenix.Utilities"
          ," System"
         ," System.Collections"
         ," System.Collections.Generic"
        ," UnityEngine"
       ," static ES.EnumCollect"};

        #endregion

        #region 内容
        [LabelText("法则"), SerializeReference]
        public ESCodeRune rune;
        [LabelText("生成树"), SerializeReference]
        public List<IESCodeTreeClass> trees = new List<IESCodeTreeClass>();

        public string NameCache => nameCache;
        private string nameCache;
        #endregion
        [Button("生成",ButtonHeight =50)]
        public void GeneTruely()
        {
            string content = ESStaticDesignUtility.SimpleScriptMaker.
               CreateNotes("该脚本为ES代码生成，\n时间：" + DateTime.Now + "\n");
            string nameCache = "";
            foreach (var i in trees)
            {
                if (i != null)
                {
                    content += i.Pick(this, this, rune);
                    if ((nameCache == null || nameCache == "" || nameCache.IsNullOrWhitespace()) && i is IESCodeTreeClass cla)
                    {
                        nameCache = cla.NameCache;
                    }
                }
            }
            foreach (var i in HandNameSpaces)
            {
                EndNameSpaces.Add(i._RemoveExtraSpaces());
            }
            string using_ = "";
            foreach (var i in EndNameSpaces)
            {
                using_ += "using " + i + ";\n";
            }
            
            string back = ESStaticDesignUtility.SimpleScriptMaker.CreateScriptBounds
                (Folderpath: folderPath,
                fileName: nameCache = fileName._AddPreAndLast(UseClassNameAtFirst ? nameCache : "", UseExtesionAtLast ? ".cs" : ""),
                using_: using_,
                nameSpace: MyNameSpace,
                content: content,
                TruelyCreate: true
                );
            ESStaticDesignUtility.SafeEditor.PingAssetByPath(Path.Combine(folderPath, nameCache));
        }
        public override string Pick(IESCodeTreeClass on = null, ESCodeTreeTarget from = null, ESCodeRune with = null)
        {
            string content = ESStaticDesignUtility.SimpleScriptMaker.
               CreateNotes("该脚本为ES代码生成，\n时间：" + DateTime.Now + "\n");
            string nameCache = "";
            foreach (var i in trees)
            {
                if (i != null)
                {
                    content += i.Pick(this, this, rune);
                    if ((nameCache == null || nameCache == "" || nameCache.IsNullOrWhitespace()) && i is IESCodeTreeClass cla)
                    {
                        nameCache = cla.NameCache;
                    }
                }
            }
            foreach(var i in HandNameSpaces)
            {
                EndNameSpaces.Add(i._RemoveExtraSpaces());
            }
            string using_ = "";
            foreach(var i in EndNameSpaces)
            {
                using_ += "using " + i + ";\n";
            }
            string back= ESStaticDesignUtility.SimpleScriptMaker.CreateScriptBounds
                (Folderpath: folderPath,
                fileName: nameCache = fileName._AddPreAndLast(UseClassNameAtFirst ? nameCache : "", UseExtesionAtLast ? ".cs" : ""),
                using_: using_,
                nameSpace: MyNameSpace,
                content: content,
                TruelyCreate:false
                );
            return back;

        }
        //指定文件路径/文件名
        //指定using 和 命名空间
        //指定内容树
    }
    public interface ESCodeRune
    {

    }
    /*ES 代码生成树节点(类具有无限嵌套特点，可以作为树，但是方法等就不行)*/
    public interface IESCodeTreeNode : IPointerForString<IESCodeTreeClass, ESCodeTreeTarget, ESCodeRune>
    {
        /* 返回的string是生成的代码
         * 参数On From With 可以约定俗成
           
         
         */
        object IPointer.Pick(object on, object from, object with)
        {
            return Pick(on as IESCodeTreeClass, from as ESCodeTreeTarget, with as ESCodeRune);
        }
    }
    /// <summary> 节点总树--》对应一个类/接口/结构体
    /// 
    /// </summary>
    public interface IESCodeTreeClass : IESCodeTreeNode
    {
         string NameCache { get; }
    }
    /// <summary>  组分
    /// IESCodeTreeNode On 作用于哪个节点
    /// IESCodeTree From 来自哪个类型
    /// string 自定义规范
    /// </summary>
    public interface IESCodePart : IPointerForString<IESCodeTreeNode, IESCodeTreeClass, string>
    {
        HashSet<string> RequireNamespace();
        object IPointer.Pick(object on, object from, object with)
        {
            return Pick(on as IESCodeTreeNode, from as IESCodeTreeClass, with.ToString());
        }
    }


    #endregion

    #region 特性(专属)声明定义

    public interface IESCodePart_AttributeGive : IESCodePart
    {
        public string Pick_ROOT(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            string content = Pick(on, from, with)._KeepBeforeByLastChar(',')._AddPreAndLast("[", "]");
            if (content._RemoveExtraSpaces().Length < 4) return "";
            return content;
        }
    }
    public interface IESCodePart_AttributeGive_ForField : IESCodePart_AttributeGive
    {

    }
    public interface IESCodePart_AttributeGive_ForMethod : IESCodePart_AttributeGive
    {

    }
    public interface IESCodePart_AttributeGive_ForClass : IESCodePart_AttributeGive
    {

    }
    #endregion

    #region 基础节点
    [Serializable, TypeRegistryItem("标准节点")]
    public abstract class ESCodeNode_Standard : IESCodeTreeNode
    {
        public abstract string Pick(IESCodeTreeClass on = null, ESCodeTreeTarget from = null, ESCodeRune with = null);
        [LabelText("预览"), DisplayAsString(TextAlignment.Left, Overflow = false), HideLabel, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_02")] public string preview = "";
        private void Preview()
        {
            preview = Pick();
        }
        private void UnPreview()
        {
            preview = "";
        }
    }
    [Serializable, TypeRegistryItem("基础生成类")]
    public class ESCodeTree_Class : PointerPackerBase<string, IESCodeTreeClass, ESCodeTreeTarget, ESCodeRune, IESCodeTreeNode>, IESCodeTreeClass
    {
        [LabelText("预览"), DisplayAsString(TextAlignment.Left, Overflow = false), HideLabel, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_02")] public string preview = "";
        private void Preview()
        {
            preview = Pick();
        }
        private void UnPreview()
        {
            preview ="";
        }
        [LabelText("附加修饰符"), BoxGroup("修饰符", showLabel: false)] public Modifier_Class_Addition modifier_addition = Modifier_Class_Addition.None;
        [LabelText("类"), InlineButton("UnPreview", "取消预览"), InlineButton("Preview", "预览(无环境)")] public string className = "aClass";
        [LabelText("修正类名"), ShowInInspector] public string className_FIX => className?._ToValidIdentName();

        public string NameCache => nameCache;
        private string nameCache;
        [LabelText("使用继承"), BoxGroup("类型与初始化", showLabel: false)] public bool AsSubclass = false;
        [LabelText("继承类型"), SerializeReference, BoxGroup("类型与初始化"), ShowIf("AsSubclass")] public ESCodePart_TypeDefineAndInit typeAndInit = new ESCodePart_TypeDefineAndInit_SelectUnityCore() { core = TypeDefine_UnityCore._MonoBehaviour };
        [LabelText("特性赋予"), SerializeReference, BoxGroup("特性", showLabel: false), ESBackGround("")] public IESCodePart_AttributeGive_ForClass attribute = null;
        public virtual string GetContent(IESCodeTreeClass on = null, ESCodeTreeTarget from = null, ESCodeRune with = null)
        {
            string content = "";
            foreach (var i in pointers)
            {
                if (i != null)
                {
                    content += i.Pick(on, from, with);
                }
            }
            return content;
        }
        public override string Pick(IESCodeTreeClass on = null, ESCodeTreeTarget from = null, ESCodeRune with = null)
        {
            string typeName = typeAndInit?.Pick(this, on, "AAA") ?? "int";
            string back = ESStaticDesignUtility.SimpleScriptMaker.CreateClassContentByString(
                className: nameCache = className_FIX,
                beforeClassName: modifier_addition._Get_ATT_ESStringMessage(),
                insideClass: GetContent(on, from, with),
                parent: AsSubclass ? ": " + typeAndInit?.Pick(this, on, "AAA") ?? "" : "",
                Attribute: attribute?.Pick_ROOT(this, on, "AAA") ?? ""
                );

            return back;
        }
    }


    [Serializable, TypeRegistryItem("标准注释")]
    public class ESCodeNode_Comment : ESCodeNode_Standard
    {
        [LabelText("开始换行")] public bool nextLine = true;
        [LabelText("多行(加\\n都会被认为)")] public bool mutiLines = true;
        [LabelText("注释内容"), TextArea(2, 10), InlineButton("Preview", "预览(无环境)")] public string content = "注释内容";
        public override string Pick(IESCodeTreeClass on = null, ESCodeTreeTarget from = null, ESCodeRune with = null)
        {
            string contentStart = (nextLine ? "\n" : "") + content;
            bool muti = contentStart.Contains('\n') ? true : mutiLines;
            if (muti) return contentStart._AddPreAndLast("/*", "*/");
            return "//" + contentStart;
        }
    }
    [Serializable, TypeRegistryItem("标准方法")]
    public class ESCodeNode_Method : ESCodeNode_Standard
    {
        [LabelText("访问修饰符"), BoxGroup("修饰符", showLabel: false)] public Modifier_Access modifier_Access = Modifier_Access.Public;
        [LabelText("附加修饰符"), BoxGroup("修饰符", showLabel: false)] public Modifier_Method_Addition modifier_addition = Modifier_Method_Addition.None;
        [LabelText("方法名"), InlineButton("Preview", "预览(无环境)")] public string methodName = "a";
        [LabelText("修正方法名"), ShowInInspector] public string methodName_FIX => methodName?._ToValidIdentName();
        [LabelText("有返回值"), BoxGroup("类型与初始化", showLabel: false)] public bool UseReturn = false;
        [LabelText("返回类型与默认返回"), ShowIf("UseReturn"), SerializeReference, BoxGroup("类型与初始化")] public ESCodePart_TypeDefineAndInit typeAndInit = new ESCodePart_TypeDefineAndInit_SelectOrignial();
        [LabelText("参数"), SerializeReference, BoxGroup("参数", showLabel: false)] public IESCodePart_Param param = new ESCodePart_Param_Standard();
        [LabelText("特性赋予"), SerializeReference, BoxGroup("特性", showLabel: false), ESBackGround("")] public IESCodePart_AttributeGive_ForMethod attribute = null;
        [LabelText("使用泛型(暂不支持)"), BoxGroup("特性", showLabel: false)] public bool UseGeneric = false;
        public override string Pick(IESCodeTreeClass on = null, ESCodeTreeTarget from = null, ESCodeRune with = null)
        {
            string typeName = typeAndInit.Pick(this, on, "AAA");
            string back = ESStaticDesignUtility.SimpleScriptMaker.CreateMethod(
                methodName: methodName,
                modifier: modifier_Access._Get_ATT_ESStringMessage() + " " + modifier_addition._Get_ATT_ESStringMessage(),
                back: UseReturn ? typeName : "void",
                betweenNameAndParams:/**/"",
                betweenParamsAndInside: "",
                para: param?.Pick_ROOT(),
                Attribute: attribute?.Pick_ROOT() ?? "",
                last: UseReturn ? "return " + typeAndInit.ReturnValue(typeName) + ";" : ""
                );
            return back;
        }
    }
    [Serializable, TypeRegistryItem("标准字段")]
    public class ESCodeNode_Field : ESCodeNode_Standard
    {
        [LabelText("访问修饰符"), BoxGroup("修饰符", showLabel: false)] public Modifier_Access modifier_Access = Modifier_Access.Public;
        [LabelText("附加修饰符"), BoxGroup("修饰符", showLabel: false)] public Modifier_Field_Addition modifier_addition = Modifier_Field_Addition.None;
        [LabelText("字段名"), InlineButton("Preview", "预览(无环境)")] public string fieldName = "a";
        [LabelText("修正字段名"), ShowInInspector] public string fieldName_FIX => fieldName?._ToValidIdentName();
        [LabelText("类型与初始化定义"), SerializeReference, BoxGroup("类型与初始化", showLabel: false)] public ESCodePart_TypeDefineAndInit typeAndInit = new ESCodePart_TypeDefineAndInit_SelectOrignial();
        [LabelText("特性赋予"), SerializeReference, BoxGroup("特性", showLabel: false), ESBackGround("")] public IESCodePart_AttributeGive_ForField attribute = null;

        public override string Pick(IESCodeTreeClass on = null, ESCodeTreeTarget from = null, ESCodeRune with = null)
        {
            string typeName = typeAndInit?.Pick(this, on, "AAA") ?? "int";
            string back = ESStaticDesignUtility.SimpleScriptMaker.CreateFieldContent(
                typeName: typeName,
                fieldName: fieldName._ToValidIdentName(),
                modifier: modifier_Access._Get_ATT_ESStringMessage() + " " + modifier_addition._Get_ATT_ESStringMessage(),
                valueDefine: typeAndInit.InitValue(typeName),
                attribute: attribute?.Pick_ROOT() ?? ""
                );

            return back;
        }
    }


    [Serializable, TypeRegistryItem("标准声明语句")]
    public class ESCodeNode_Define_Define_ : ESCodeNode_Standard
    {
        [LabelText("参数名")] public string paraName = "a";
        [LabelText("修正名"), ShowInInspector] public string paraName_FIX => paraName?._ToValidIdentName();
        [LabelText("类型与初始化定义"), SerializeReference, BoxGroup("类型与初始化", showLabel: false)] public ESCodePart_TypeDefineAndInit typeAndInit = new ESCodePart_TypeDefineAndInit_SelectOrignial();

        public override string Pick(IESCodeTreeClass on = null, ESCodeTreeTarget from = null, ESCodeRune with = null)
        {
            string typeName = typeAndInit?.Pick(this, on, "AAA") ?? "int";
            string back = ESStaticDesignUtility.SimpleScriptMaker.CreateParaOrDefineContent(
                typeName: typeName,
                itName: paraName?._ToValidIdentName(),
                modifier: "",
                valueDefine: typeAndInit.ParamValue(typeName),
                isDefine: false
                ); ;
            return back;
        }
    }
    #endregion

    #region 基本组分

    #region 组分定义
    [Serializable, TypeRegistryItem("特性列表")]
    public class ESCodePart_XXList<T> : IESCodePart where T : IESCodePart
    {
        [LabelText("特性列表"), SerializeReference]
        public List<T> Parts = new List<T>();
        //可以嵌套
        public string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            string content = "";
            for (int i = 0; i < Parts.Count; i++)
            {
                if (Parts[i] != null)
                {
                    content += Parts[i].Pick(on, from, with) + ",";
                }
            }
            return content;
        }

        public virtual HashSet<string> RequireNamespace()
        {
            HashSet<string> nameSpaces = new HashSet<string>();
            foreach (var i in Parts)
            {
                var get = i.RequireNamespace();
                if (get != null)
                {
                    foreach (var ii in get)
                    {
                        nameSpaces.Add(ii);
                    }
                }
            }
            return nameSpaces;
        }
        //作为总输出


    }

    #endregion

    #region 类型定义
    [Serializable, TypeRegistryItem("类型定义")]
    public abstract class ESCodePart_TypeDefineAndInit : IESCodePart
    {
        //Pick 返回类型定义 再额外产生赋值
        [LabelText("初始化赋值")] public ValueInitType fieldInitValue = ValueInitType.None;
        [LabelText("直接输入"), ShowIf("@fieldInitValue== EnumCollect.ValueInitType.DirectGiveValue")] public string inputValue = "...";
        [LabelText("填充内容"), ShowIf("@fieldInitValue == EnumCollect.ValueInitType.NewItAndWith")] public string with = "暂时不支持";
        public virtual string InitValue(string pickBack = null)
        {
            if (fieldInitValue == ValueInitType.None) return "";
            if (fieldInitValue == ValueInitType.NULL) return "=null";
            if (fieldInitValue == ValueInitType.Default) return "=default";
            if (fieldInitValue == ValueInitType.DirectGiveValue) return inputValue.IsNullOrWhitespace() ? "" : "=" + inputValue._RemoveString(";", "=");
            if (fieldInitValue == ValueInitType.NewIt) return "=new " + pickBack ?? Pick();
            if (fieldInitValue == ValueInitType.NewItAndWith) return "=new " + pickBack ?? Pick();
            return "";
        }
        public virtual string ReturnValue(string pickBack = null)
        {
            if (fieldInitValue == ValueInitType.None) return "throw new NotImplementedException()";
            if (fieldInitValue == ValueInitType.NULL) return "null";
            if (fieldInitValue == ValueInitType.Default) return "default";
            if (fieldInitValue == ValueInitType.DirectGiveValue) return inputValue.IsNullOrWhitespace() ? "" : inputValue._RemoveString(";", "=", "return");
            if (fieldInitValue == ValueInitType.NewIt) return "new " + pickBack ?? Pick();
            if (fieldInitValue == ValueInitType.NewItAndWith) return "new " + pickBack ?? Pick();
            return "";
        }
        public virtual string ParamValue(string pickBack = null)
        {
            if (fieldInitValue == ValueInitType.None) return "";
            if (fieldInitValue == ValueInitType.NULL) return "=null";
            if (fieldInitValue == ValueInitType.Default) return "=default";
            if (fieldInitValue == ValueInitType.DirectGiveValue) return inputValue.IsNullOrWhitespace() ? "" : "=" + inputValue._RemoveString(";", "=", "return");
            if (fieldInitValue == ValueInitType.NewIt) return "=null";
            if (fieldInitValue == ValueInitType.NewItAndWith) return "=null";
            return "";
        }
        public abstract string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null);

        public virtual HashSet<string> RequireNamespace()
        {
            return null;
        }
    }
    [Serializable, TypeRegistryItem("类型定义-直接选择原始类型", "类型定义/直接")]
    public class ESCodePart_TypeDefineAndInit_SelectOrignial : ESCodePart_TypeDefineAndInit
    {
        [LabelText("类型筛选(原始)")]
        public EnumCollect.TypeDefine_Orignial orignial = TypeDefine_Orignial.Int;

        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return orignial._Get_ATT_ESStringMessage();
        }
    }
    [Serializable, TypeRegistryItem("类型定义-直接选择Unity核心类型", "类型定义/直接")]
    public class ESCodePart_TypeDefineAndInit_SelectUnityCore : ESCodePart_TypeDefineAndInit
    {
        [LabelText("类型筛选(原始)")]
        public EnumCollect.TypeDefine_UnityCore core = TypeDefine_UnityCore._Transform;

        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return core._Get_ATT_ESStringMessage();
        }
    }
    [Serializable, TypeRegistryItem("类型定义-直接选择数学类型", "类型定义/直接")]
    public class ESCodePart_TypeDefineAndInit_SelectMath : ESCodePart_TypeDefineAndInit
    {
        [LabelText("类型筛选(原始)")]
        public EnumCollect.TypeDefine_Math math = TypeDefine_Math.Int;

        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return math._Get_ATT_ESStringMessage();
        }
    }
    [Serializable, TypeRegistryItem("类型定义-直接选择旧版UI", "类型定义/直接")]
    public class ESCodePart_TypeDefineAndInit_SelectUI : ESCodePart_TypeDefineAndInit
    {
        [LabelText("类型筛选(原始)")]
        public EnumCollect.TypeDefine_UI_OLD ui = TypeDefine_UI_OLD._Image;

        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return ui._Get_ATT_ESStringMessage();
        }
    }
    [Serializable, TypeRegistryItem("类型定义-直接选择TMPUI", "类型定义/直接")]
    public class ESCodePart_TypeDefineAndInit_SelectTMPUI : ESCodePart_TypeDefineAndInit
    {
        [LabelText("类型筛选(原始)")]
        public EnumCollect.TypeDefine_UI_TMP ui = TypeDefine_UI_TMP._Image;

        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return ui._Get_ATT_ESStringMessage();
        }
    }
    [Serializable, TypeRegistryItem("类型定义-手写", "类型定义/直接")]
    public class ESCodePart_TypeDefineAndInit_HandMake : ESCodePart_TypeDefineAndInit
    {
        [LabelText("手写类型")]
        public string hand = "aType";

        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return hand;
        }
    }
    #endregion

    #region 赋值与填充


    #endregion

    #region 特性赋予

    public abstract class ESCodePart_AttributeGive : IESCodePart_AttributeGive
    {
        public abstract string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null);

        public virtual HashSet<string> RequireNamespace()
        {
            return null;
        }
    }

    #region 字段特性
    [Serializable, TypeRegistryItem("特性_字段专属")]
    public abstract class ESCodePart_AttributeGive_Field : ESCodePart_AttributeGive, IESCodePart_AttributeGive_ForField
    {

    }
    [Serializable, TypeRegistryItem("特性_列表_字段专属")]
    public class ESCodePart_AttributeGiveList_Field : ESCodePart_XXList<ESCodePart_AttributeGive_Field>, IESCodePart_AttributeGive_ForField
    {

    }
    [Serializable, TypeRegistryItem("特性_字段_序列化字段(SerilizedField)", "特性定义/序列化与显示")]
    public class ESCodePart_AttributeGive_Field_SerilizedField : ESCodePart_AttributeGive_Field
    {
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return "SerializeField";
        }
    }
    [Serializable, TypeRegistryItem("特性_字段_不序列化(NonSerialized)", "特性定义/序列化与显示")]
    public class ESCodePart_AttributeGive_Field_NonSerialized : ESCodePart_AttributeGive_Field
    {
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return "NonSerialized";
        }
    }
    [Serializable, TypeRegistryItem("特性_字段_Odin序列化(OdinSerialize)", "特性定义/序列化与显示")]
    public class ESCodePart_AttributeGive_Field_OdinSerialize : ESCodePart_AttributeGive_Field
    {
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return "NonSerialized,OdinSerialize";
        }
    }
    [Serializable, TypeRegistryItem("特性_字段_多态序列化(SerializeReference)", "特性定义/序列化与显示")]
    public class ESCodePart_AttributeGive_Field_SerializeReference : ESCodePart_AttributeGive_Field
    {
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return "SerializeReference";
        }
    }
    [Serializable, TypeRegistryItem("特性_字段_强制显示(ShowInInspector)", "特性定义/序列化与显示")]
    public class ESCodePart_AttributeGive_Field_ShowInInspector : ESCodePart_AttributeGive_Field
    {
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return "ShowInInspector";
        }
    }
    [Serializable, TypeRegistryItem("特性_字段_空格(Space)", "特性定义/原生排版")]
    public class ESCodePart_AttributeGive_Field_Space : ESCodePart_AttributeGive_Field
    {
        [LabelText("空格")] public int space = 10;
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return $"Space({space})";
        }
    }
    [Serializable, TypeRegistryItem("特性_字段_小标题头版(Header)", "特性定义/原生排版")]
    public class ESCodePart_AttributeGive_Field_Header : ESCodePart_AttributeGive_Field
    {
        [LabelText("头版")] public string header = "...";
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return $"[Header({header._AsStringValue()})";
        }
    }
    #endregion

    #region 方法特性
    [Serializable, TypeRegistryItem("特性_方法专属")]
    public abstract class ESCodePart_AttributeGive_Method : ESCodePart_AttributeGive, IESCodePart_AttributeGive_ForMethod
    {

    }
    [Serializable, TypeRegistryItem("特性_列表_方法专属")]
    public class ESCodePart_AttributeGiveList_Method : ESCodePart_XXList<ESCodePart_AttributeGive_Method>, IESCodePart_AttributeGive_ForMethod
    {

    }
    [Serializable, TypeRegistryItem("特性_方法_手写")]
    public class ESCodePart_AttributeGiveList_Method_HandMake : ESCodePart_AttributeGive_Method
    {
        [LabelText("手写方法")]
        public string HandMake = "Button";
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return HandMake;
        }
    }

    #endregion

    #region 类特性
    public abstract class ESCodePart_AttributeGive_Class : ESCodePart_AttributeGive, IESCodePart_AttributeGive_ForClass
    {

    }
    [Serializable, TypeRegistryItem("特性_列表_类专属")]
    public class ESCodePart_AttributeGiveList_Class : ESCodePart_XXList<ESCodePart_AttributeGive_Class>, IESCodePart_AttributeGive_ForClass
    {

    }
    [Serializable, TypeRegistryItem("特性_类_手写")]
    public class ESCodePart_AttributeGiveList_Class_HandMake : ESCodePart_AttributeGive_Class
    {
        [LabelText("手写特性")]
        public string HandMake = "Serializable";
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return HandMake;
        }
    }
    [Serializable, TypeRegistryItem("特性_类_可序列化")]
    public class ESCodePart_AttributeGiveList_Class_Serializable : ESCodePart_AttributeGive_Class
    {
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return "Serializable";
        }
    }
    [DisallowMultipleComponent]
    [Serializable, TypeRegistryItem("特性_类_依赖脚本")]
    public class ESCodePart_AttributeGiveList_Class_RequireComponent : ESCodePart_AttributeGive_Class
    {
        [LabelText("依赖脚本"), SerializeReference] public ESCodePart_TypeDefineAndInit type = new ESCodePart_TypeDefineAndInit_SelectUnityCore() { core = TypeDefine_UnityCore._Rigidbody };
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            string typeName = type.Pick(on, from, with);
            return $"RequireComponent(typeof({typeName}))";
        }
    }
    [Serializable, TypeRegistryItem("特性_类_禁止多挂载")]
    public class ESCodePart_AttributeGiveList_Class_DisallowMultipleComponent : ESCodePart_AttributeGive_Class
    {
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return $"DisallowMultipleComponent";
        }
    }
    [Serializable, TypeRegistryItem("特性_类_编辑器时运行")]
    public class ESCodePart_AttributeGiveList_Class_ExecuteInEditMode : ESCodePart_AttributeGive_Class
    {
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return $"ExecuteInEditMode";
        }
    }
    [Serializable, TypeRegistryItem("特性_类_添加到Add菜单")]
    public class ESCodePart_AttributeGiveList_Class_AddComponentMenu : ESCodePart_AttributeGive_Class
    {
        [LabelText("菜单")] public string menu = "MyMono/This";
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return $"AddComponentMenu({menu._AsStringValue()})";
        }
    }
    [Serializable, TypeRegistryItem("特性_类_创建菜单(SO)")]
    public class ESCodePart_AttributeGiveList_Class_CreateAssetMenu : ESCodePart_AttributeGive_Class
    {
        [LabelText("文件名")] public string file = "aSO";
        [LabelText("菜单")] public string menu = "MySO/This";
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return $"CreateAssetMenu(fileName ={file._AsStringValue()},menuName ={menu._AsStringValue()}))";
        }
    }
    [Serializable, TypeRegistryItem("特性_类_类型注册")]
    public class ESCodePart_AttributeGiveList_Class_TypeRegistryItem : ESCodePart_AttributeGive_Class
    {
        [LabelText("注册名")] public string ReName = "注册名";
        [LabelText("使用分组")] public bool UseGroup = false;
        [LabelText("组名")] public string Group = "组";
        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            return UseGroup ? $"TypeRegistryItem({ReName._AsStringValue()},{Group._AsStringValue()})" : $"TypeRegistryItem({ReName._AsStringValue()})";
        }
    }
    #endregion
    #endregion

    #region 参数
    public interface IESCodePart_Param : IESCodePart
    {
        public string Pick_ROOT(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            string content = Pick(on, from, with)._KeepBeforeByLastChar(',');
            if (content._RemoveExtraSpaces().Length < 2) return "";
            return content;
        }
    }
    [Serializable, TypeRegistryItem("参数_")]
    public abstract class ESCodePart_Param : IESCodePart_Param
    {
        public abstract string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null);

        public virtual HashSet<string> RequireNamespace()
        {
            return null;
        }
    }
    [Serializable, TypeRegistryItem("参数_列表")]
    public class ESCodePart_ParamList : ESCodePart_XXList<ESCodePart_Param>, IESCodePart_Param
    {

    }
    [Serializable, TypeRegistryItem("标准参数")]
    public class ESCodePart_Param_Standard : ESCodePart_Param
    {
        [LabelText("参数名")] public string paraName = "a";
        [LabelText("修正名"), ShowInInspector] public string paraName_FIX => paraName?._ToValidIdentName();
        [LabelText("类型与初始化定义"), SerializeReference, BoxGroup("类型与初始化", showLabel: false)] public ESCodePart_TypeDefineAndInit typeAndInit = new ESCodePart_TypeDefineAndInit_SelectOrignial();

        public override string Pick(IESCodeTreeNode on = null, IESCodeTreeClass from = null, string with = null)
        {
            string typeName = typeAndInit?.Pick(on, from, with) ?? "int";
            string back = ESStaticDesignUtility.SimpleScriptMaker.CreateParaOrDefineContent(
                typeName: typeName,
                itName: paraName?._ToValidIdentName(),
                modifier: "",
                valueDefine: typeAndInit.ParamValue(typeName),
                isDefine: false
                );
            return back;
        }

    }

    #endregion

    #endregion
}

