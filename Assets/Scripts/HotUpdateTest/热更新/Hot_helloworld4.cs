using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ES
{
    public class Hot_helloworld4 : FishNet.Object.NetworkBehaviour
    {
        public static void Run()
        {
           
            Debug.Log("Hello, Hot BASE");
            Hot_helloworld.Run();
        }
    }
    public class Hot_helloworld5 : FishNet.Object.NetworkBehaviour
    {
        public static void Run()
        {

            Debug.Log("Hello, Hot BASE");
            Hot_helloworld.Run();
        }
    }
}
