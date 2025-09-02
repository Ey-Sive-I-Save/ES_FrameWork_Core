using Sirenix.OdinInspector;
using System;
using UnityEngine;
namespace ES
{
    [Serializable, TypeRegistryItem("Message提供")]
    public class ESUIMessageProviderDomain : ESUIDomain_Original<ESUIElementCore, ESUIMessageProviderModule>
    {
        [ShowInInspector, HideInEditorMode, LabelText("主信息提供源(运行时实时更换测试)")]
        public IMessageProvider MainProvider { get=> _mainProvider;private set { if (_mainProvider != value) {  _mainProvider = value; if(EnableMainLink) DO_SendMainReaderLink(); } } }
        [HideInInspector]
        private IMessageProvider _mainProvider;
        [SerializeReference,LabelText("预注册主信息提供"),HideLabel,HideInPlayMode]
        public IMessageProv_Reg_Ab RegisterMain;
        [ESBoolOption("禁用信息更新事件发送", "启用信息更新事件发送")]
        public bool EnableMainLink = true;
        /* [ShowInInspector, DisableInEditorMode,LabelText("常规备用读取器")]
         protected Dictionary<string, IMessageProvider> Readers = new Dictionary<string, IMessageProvider>();
         [ESBoolOption("禁用常规事件发送", "启用常规事件发射")]
         public bool EnableReadersLink = false;*/
        [ShowInInspector,ReadOnly]
        public LinkReceiveList<Link_MessageProvider> LinkReceive = new LinkReceiveList<Link_MessageProvider>();

        public void DO_SendMainReaderLink()
        {
             LinkReceive.SendLink(new Link_MessageProvider() { key = "Main", provider = _mainProvider, isMain = true });
        }

        public void SetMainMessageProvider(IMessageProvider reader)
        {
            if (reader != _mainProvider)
            {
                _mainProvider = reader;
                if (EnableMainLink) DO_SendMainReaderLink();
            }
        }
    
        public IMessageProvider GetMainMessageProvider()
        {
            return _mainProvider;
        }
        public override void AwakeRegisterAllModules()
        {
            if (RegisterMain != null)
            {
                SetMainMessageProvider(RegisterMain.Registe);
            }
            base.AwakeRegisterAllModules();
        }

        /* public IMessageProvider GetKeyMessageProvider(string key)
         {
             if (Readers.TryGetValue(key, out var v))
             {
                 if (v != null)
                 {
                     return v;
                 }
             }
             return null;
         }
         public void SetKeyMessageProvider(string key, IMessageProvider reader, bool AddIfNull = false)
         {
             if (Readers.TryGetValue(key, out var v))
             {
                 if (v != reader)
                 {
                     Readers[key] = reader;
                     LinkReceive.SendLink(new Link_MessageProvider() { key = key, reader = reader, isMain = false });
                 }
             }
             else if (AddIfNull)
             {
                 Readers[key] = reader;
                 LinkReceive.SendLink(new Link_MessageProvider() { key = key, reader = reader, isMain = false });
             }
         }
 */

    }
    [Serializable, TypeRegistryItem("UI信息提供源扩展模块")]
    public abstract class ESUIMessageProviderModule : ESUIModule_Orignal<ESUIElementCore, ESUIMessageProviderDomain>
    {

    }
}
