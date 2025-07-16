using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class testCopyTo : MonoBehaviour
{
    public SHJ from;
    public SuperSHJ from2;

    public SHJ to1;
    public SHJ to2;
    [Button]
    void copyTo()
    {
        to1.SharedData = from.SharedData;
        to1.VariableData.DeepCloneFrom(from.VariableData);

        KeyValueMatchingUtility.DataApply.CopyToClassSameType(from, to2);

        KeyValueMatchingUtility.DataApply.CopyTo<SHJSharedData, SHJVariableData>(from, to2);

        KeyValueMatchingUtility.DataApply.CopyToClassDynamic(from2, to1);
    }
    [Serializable]
    public class SHJ : IWithSharedAndVariableData<SHJSharedData, SHJVariableData>
    {
        [field:SerializeField]public SHJSharedData SharedData { get; set; }
        [field: SerializeField] public SHJVariableData VariableData { get; set; }
    }
    [Serializable]
    public class SuperSHJ : IWithSharedAndVariableData<SuperSHJSharedData, SHJVariableData>
    {
        [field: SerializeField] public SuperSHJSharedData SharedData { get; set; }
        [field: SerializeField] public SHJVariableData VariableData { get; set; }
    }

    [Serializable]
    public class SHJSharedData : ISharedData
    {
        public List<string> allDrinks = new List<string>();
        public Vector3 size;
        public string brand = "品牌";
    }
    [Serializable]
    public class SuperSHJSharedData : SHJSharedData
    {
        public float ffff;
        public float bbb;
    }

    [Serializable]
    public class SHJVariableData : IVariableData,IDeepClone<SHJVariableData>
    {
        public Vector3 pos;
        public float hasEarn = 0;
        public float hasStay = 0;

        public void DeepCloneFrom(SHJVariableData t)
        {
            pos = t.pos;
            /*hasEarn = t.hasEarn;
            hasStay = t.hasStay;*/
        }

        public void Init(params object[] ps)
        {
            hasEarn = 0;
            hasStay = 0;
        }
    }

   
}
