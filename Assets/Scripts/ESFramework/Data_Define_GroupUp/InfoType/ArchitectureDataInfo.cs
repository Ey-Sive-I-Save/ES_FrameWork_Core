using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    //不融入全局配置
    [ESDisplayNameKeyToType("数据单元", "原型数据单元")]
    public class ArchitectureDataInfo : SoDataInfo
    {
        [SerializeReference,LabelText("携带参数")]
        public IArchitectureValue Value=new ArchitectureTypeValue_Float();
        public IArchitectureValue getArch => Value;

        public object TheKey => Value?.TheKey;

        public object TheValue => Value?.TheValue;
        private void OnValidate()
        {
          
        }
        public void SetValue(object o)
        {
            Value.SetValue(o); 
        }
    }
}
