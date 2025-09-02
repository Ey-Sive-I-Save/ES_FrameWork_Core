using ES;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test501 : MonoBehaviour
{
    public float ff;
    [ESBoolOption("放弃转化标准格式","转化为标准格式")]
    public bool YeO = false;

    [ESBoolOption("使用旧版功能", "使用新版功能")]
    public bool YeO1 = false;

    [ESBoolOption("@hello()", "@hello2")]
    public bool YeO2 = false;

    public string hello()
    {
        return "只能作用于自己";
    }
    public string hello2 = "灵活获取作用源";

    public Stack<Attackable> aaa = new Stack<Attackable>();

    public Stack<Attackable> aaa2 = new Stack<Attackable>();

    /* public List<int> source;
     public string[] source1;
     public Dictionary<int,Link_AttackHappen> source2;
     public HashSet<Link_EntityAttackEntityTruely> source3;
     public Queue<GameObject> source4;
     public LinkedList<_deepcloneT> source5;
     public Stack<float> source6;
     [Space(25)]
     public List<int> target;
     public string[] target1;
     public Dictionary<int,Link_AttackHappen> target2;
     public HashSet<Link_EntityAttackEntityTruely> target3;
     public Queue<GameObject> target4;
     public Stack<float> target6;
     public LinkedList<_deepcloneT> target5;*/
    [Button("TEST深拷贝")]
    public void Test()
    {
        aaa2 = ESStaticDesignUtility.Creator.DeepCloneGenericStack(aaa) as Stack<Attackable>;
       /* target = KeyValueMatchingUtility.Creator.DeepClone(source);
        target1 = KeyValueMatchingUtility.Creator.DeepClone(source1);
        target2 = KeyValueMatchingUtility.Creator.DeepClone(source2);
        target3 = KeyValueMatchingUtility.Creator.DeepClone(source3);
     
        target4 = KeyValueMatchingUtility.Creator.DeepClone(source4);
        target6 = KeyValueMatchingUtility.Creator.DeepClone(source6);
        target5 = KeyValueMatchingUtility.Creator.DeepClone(source5);*/
    }
    float a, b;
    private void Update()
    {
       /* (int left, int right) s = (6,8);
        switch (s)
        {
            case (_, _) w  > s.right: return;
        }*/
       //for (int i = 0; i < 100_0000; i++)
        {
         
           // aa._SafeDivide(b);
            //隐式调用
          /*  _deepcloneT _DeepcloneT_ = KeyValueMatchingUtility.Creator.DeepClone(source5);
*/
            //显示调用IDeepClone
            //_deepcloneT _DeepcloneT = new _deepcloneT();
        }
    }
    [Serializable]
    public class _deepcloneT : IDeepClone<_deepcloneT>
    {
        public float ff;
        public float bb;
        public float not;
        public GameObject prefab;
        public void DeepCloneFrom(_deepcloneT t)
        {
            ff = t.ff;
            bb = t.bb;
            prefab = t.prefab;
        }
    }
}
