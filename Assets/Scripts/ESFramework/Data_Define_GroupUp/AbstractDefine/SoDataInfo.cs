using ES.EvPointer;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ES
{
    public interface ISoDataInfo : IWithKey<KeyString_Direct>
    {
#if UNITY_EDITOR
        void DestroyThisInfo();
#endif
    }
    public abstract class SoDataInfo : SerializedScriptableObject, ISoDataInfo
    {
        public KeyString_Direct key => DataKey;
        [LabelText("数据键", SdfIconType.KeyFill), InlineProperty]
        public KeyString_Direct DataKey = new KeyString_Direct();
        public void SetKey(object o)
        {
            if (o is string s)
            {
                if (DataKey == null) DataKey = new KeyString_Direct() { str_direc = s };

            }
        }



#if UNITY_EDITOR

        [ContextMenu("删除自己")]
        public void DestroyThisInfo()
        {
            Undo.DestroyObjectImmediate(this);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
#endif
    }
}