using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ES
{
    public partial class Item
    {
        [ToggleGroup("IsNet"), LabelText("网络行为支持")]
        public ItemNetBehaviour NetBehaviour;
      
        
    }
}
