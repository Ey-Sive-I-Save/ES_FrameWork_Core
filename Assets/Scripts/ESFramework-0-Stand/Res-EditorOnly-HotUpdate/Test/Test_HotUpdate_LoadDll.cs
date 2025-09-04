using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace ES
{
    public class LoadDll : MonoBehaviour
    {
        Assembly hotUpdateAssDesign;
        Assembly hotUpdateAssCore;
        Assembly hotUpdateAssHot;
        private void Awake()
        {
                // 获取当前应用程序域中所有已加载的程序集
                Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

                // 遍历并输出每个程序集的全名
                foreach (Assembly assembly in loadedAssemblies)
                {
                    Debug.Log(assembly.FullName);
                }
            
        }

    
        public void a()
        {

            //Debug.Log("HO111T<<");
            if (hotUpdateAssDesign == null)
            {
                var str= File.ReadAllBytes($"{Application.streamingAssetsPath}/ESDesign.dll.bytes");
               // Debug.Log("STR"+str.ToString()+str.Length);
                hotUpdateAssDesign= Assembly.Load(str);
            }

            /*if (hotUpdateAssCore == null)
            {
                var str = File.ReadAllBytes($"{Application.streamingAssetsPath}/ESCore.dll.bytes");
                // Debug.Log("STR"+str.ToString()+str.Length);
                hotUpdateAssCore = Assembly.Load(str);
            }*/
            /*  // Editor下无需加载，直接查找获得HotUpdate程序集
              var all = System.AppDomain.CurrentDomain.GetAssemblies();
              Assembly hotUpdateAss = all.First(a => a.GetName().Name == "ESCore");*/
            foreach (var i in hotUpdateAssDesign.GetTypes())
            {
                if (i.Name == "Hot_helloworld")
                {
                    Debug.Log("ASSEM<<" + i.Name + ">>");
                    i.GetMethod("Run").Invoke(null, null);
                }
            }

          /*  foreach (var i in hotUpdateAssCore.GetTypes())
            {
                if (i.Name == "Hot_helloworld2")
                {
                    Debug.Log("ASSEM<<" + i.Name + ">>");
                    i.GetMethod("Run").Invoke(null, null);
                }
            }*/
            /*  Type type = hotUpdateAss.GetType("Hot_helloworld");
              Debug.Log("TYPE<<" + type);
              type.GetMethod("Run").Invoke(null, null);*/
        }

        public void b()
        {

            Debug.Log("Logic 准备");
            if (hotUpdateAssCore == null)
            {
                Debug.Log("Logic 寻找");
                var str = File.ReadAllBytes($"{Application.streamingAssetsPath}/ESLogic.dll.bytes");
                 Debug.Log("STR"+str.ToString()+str.Length);
                 hotUpdateAssCore = Assembly.Load(str);
            }
            else
            {
                Debug.Log("Logic AT" + hotUpdateAssCore);
            }


            
             foreach (var i in hotUpdateAssCore.GetTypes())
              {
                  if (i.Name == "Hot_helloworld2")
                  {
                      Debug.Log("ASSEM<<" + i.Name + ">>");
                      i.GetMethod("Run").Invoke(null, null);
                  }
              }
            /*  Type type = hotUpdateAss.GetType("Hot_helloworld");
              Debug.Log("TYPE<<" + type);
              type.GetMethod("Run").Invoke(null, null);*/
        }
        public void c()
        {

            Debug.Log("HOT 准备");
            if (hotUpdateAssHot == null)
            {
                Debug.Log("HOT 寻找");
                var str = File.ReadAllBytes($"{Application.streamingAssetsPath}/HotUpdateTest.dll.bytes");
                Debug.Log("STR" + str.ToString() + str.Length);
                hotUpdateAssHot = Assembly.Load(str);
            }
            
             foreach (var i in hotUpdateAssHot.GetTypes())
              {
                  if (i.Name == "Hot_helloworld4")
                  {
                      Debug.Log("ASSEM<<" + i.Name + ">>");
                      i.GetMethod("Run").Invoke(null, null);
                  }
              }
            /*  Type type = hotUpdateAss.GetType("Hot_helloworld");
              Debug.Log("TYPE<<" + type);
              type.GetMethod("Run").Invoke(null, null);*/
        }
    }
}
