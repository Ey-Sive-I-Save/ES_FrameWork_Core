using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    [Serializable]
    public struct Link_TestChannel : ILink
    {
        public Channel_TestOnly channel;
    }
    public enum Channel_TestOnly
    {
        AAA,
        BBB,
        CCC,
        DDD
    }
    public class TestLinkChannel : MonoBehaviour,IReceiveLink<Link_TestChannel>
    {
        public int count1 = 100;
        public int count = 1_000;
        public int show = 0;
       
        public LinkReceivePool list 
            = new LinkReceivePool();

        private void OnEnable()
        {
            //

            


        }

        private void Update()
        {
        /*    for(int i=0;i< count1; i++)
            {
                list.AddReceive(this);
            }*/
            for (int i = 0; i < count ; i++)
            {
                list.AddReceive(this);
               
            }
           // list.SendLink(new Link_TestChannel() { });
           /* for (int i = 0; i < count; i++)
            {
               
                list.RemoveReceive(this);
            }
*/
            
            /*  for (int i = 0; i < count1; i++)
              {
                  list.RemoveReceive(this);
              }*/
        }

        public void OnLink(Link_TestChannel link)
        {
          
            show++;
        }
    }
}
