using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
     //此处仅展示如何创建显式和隐式的转换
     //主要会通过Partical 在这个文件夹下来写转换
     public class ShowHowToExplicit
     {
        // 显式转换：string →ShowHowToExplicit  (源类型是string)
        public static explicit operator ShowHowToExplicit(string e)
        {
            return new ShowHowToExplicit();
        }
        // 显式转换：ShowHowToExplicit → string (源类型是当前类)
        public static explicit operator string(ShowHowToExplicit e)
        {
            return e.ToString();
        }
        // 隐式转换：int → ShowHowToExplicit (目标类型是当前类)
        public static implicit operator ShowHowToExplicit(int e)
        {
            return new ShowHowToExplicit();
        }
        // 隐式转换： ShowHowToExplicit→ int (目标类型是int)
        public static implicit operator int(ShowHowToExplicit obj)
        {
            return obj.GetHashCode(); // 返回 int 类型
        }
    }
}

