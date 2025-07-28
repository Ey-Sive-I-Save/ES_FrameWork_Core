using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    public static class ExtensionForEMS
    {
        #region 脚本与游戏对象万能的
        public static EMS _GetOrAddAnyEMS<EMS>(this Component com) where EMS : EMS_Abstract
        {
            var ems = com.GetComponent<EMS>();
            if (ems != null) return ems;
            return com.gameObject.AddComponent<EMS>();
        }
        public static EMS _GetOrAddAnyEMS<EMS>(this GameObject ga) where EMS : EMS_Abstract
        {
            var ems = ga.GetComponent<EMS>();
            if (ems != null) return ems;
            return ga.AddComponent<EMS>();
        }
        public static EMS _GetOrAddAnyEMSInParent<EMS>(this Component com) where EMS : EMS_Abstract
        {
            var ems = com.GetComponentInParent<EMS>();
            if (ems != null) return ems;
            return com.gameObject.AddComponent<EMS>();
        }
        public static EMS _GetOrAddAnyEMSInParent<EMS>(this GameObject ga) where EMS : EMS_Abstract
        {
            var ems = ga.GetComponentInParent<EMS>();
            if (ems != null) return ems;
            return ga.AddComponent<EMS>();
        }
        public static EMS _GetOrAddAnyEMSInChildren<EMS>(this Component com) where EMS : EMS_Abstract
        {
            var ems = com.GetComponentInChildren<EMS>();
            if (ems != null) return ems;
            return com.gameObject.AddComponent<EMS>();
        }
        public static EMS _GetOrAddAnyEMSInChildren<EMS>(this GameObject ga) where EMS : EMS_Abstract
        {
            var ems = ga.GetComponentInChildren<EMS>();
            if (ems != null) return ems;
            return ga.AddComponent<EMS>();
        }
        #endregion

        #region 域和剪影支持
        public static EMS _GetOrAddAnyEMS<EMS>(this IDomain domain) where EMS : EMS_Abstract
        {
            var ems = domain.Core_Object.GetComponent<EMS>();
            if (ems != null) return ems;
            return domain.Core_Object.gameObject.AddComponent<EMS>();
        }
        public static EMS _GetOrAddAnyEMS<EMS>(this IModule mo) where EMS : EMS_Abstract
        {
            var ems = mo.Core_Object.GetComponent<EMS>();
            if (ems != null) return ems;
            return mo.Core_Object.gameObject.AddComponent<EMS>();
        }
        public static EMS _GetOrAddAnyEMSInParent<EMS>(this IDomain domain) where EMS : EMS_Abstract
        {
            var ems = domain.Core_Object.GetComponentInParent<EMS>();
            if (ems != null) return ems;
            return domain.Core_Object.gameObject.AddComponent<EMS>();
        }
        public static EMS _GetOrAddAnyEMSInParent<EMS>(this IModule mo) where EMS : EMS_Abstract
        {
            var ems = mo.Core_Object.GetComponentInParent<EMS>();
            if (ems != null) return ems;
            return mo.Core_Object.gameObject.AddComponent<EMS>();
        }
        public static EMS _GetOrAddAnyEMSInChildren<EMS>(this IDomain domain) where EMS : EMS_Abstract
        {
            var ems = domain.Core_Object.GetComponentInChildren<EMS>();
            if (ems != null) return ems;
            return domain.Core_Object.gameObject.AddComponent<EMS>();
        }
        public static EMS _GetOrAddAnyEMSInChildren<EMS>(this IModule mo) where EMS : EMS_Abstract
        {
            var ems = mo.Core_Object.GetComponentInChildren<EMS>();
            if (ems != null) return ems;
            return mo.Core_Object.gameObject.AddComponent<EMS>();
        } 

        #endregion


        class EXAMPLE : IReceiveLink<Link_EMS_BeginDrag>
        {
            public MonoBehaviour mb;
            public IDomain domain1;
            public IModule module;
            void OnEnable()
            {
                mb._GetOrAddAnyEMS<EMS_BeginDrag_LinkList>();

                domain1._GetOrAddAnyEMSInChildren<EMS_BeginDrag_LinkSingle>();

                module._GetOrAddAnyEMSInParent<EMS_Drop_LinkList>();
            }
            public void OnLink(Link_EMS_BeginDrag link)
            {
                throw new NotImplementedException();
            }
        }
    }
}
