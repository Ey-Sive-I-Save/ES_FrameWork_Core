using ES;
using ES.Pointer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ES {


    public abstract class BaseRunTimeStrategyCollection<Select,Self> : SingletonAsNormalClass<Self>
        where Self:BaseRunTimeStrategyCollection<Select, Self>, new()
    {
        public static Self NULL = new Self();
        public abstract Dictionary<Select, Self> AllStrategies { get; }
        public Self GetStrategy(Select enum_)
        {
            
            if (AllStrategies == null) return null;
            if (AllStrategies.TryGetValue(enum_,out var str)) return str;
            return NULL;
        }
    }
    
    
}
