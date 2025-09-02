using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ES
{
    public interface IKey<out KeyType> : IKey
    {
        KeyType KeySelf();
    }
    public interface IKey
    {

    }
    public interface IWithStringKey : IWithKey<KeyString>
    {

    }
    public interface IWithKey<out With> : IWithKey where With : IKey
    {
        With key { get; }
    }
    public interface IWithKey
    {
        void SetKey(object o);
    }
    public abstract class KeyString : BaseKey<string>
    {
        public override string KeySelf()
        {
            
            return "";
        }
    }
    public abstract class BaseKey<KeyType> : IKey<KeyType>
    {
        public virtual KeyType KeySelf()
        {
            return default(KeyType);
        }
        public override int GetHashCode()
        {
            return KeySelf()?.GetHashCode() ?? base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            // Debug.Log("Equals"+ typeof(GameKeyType));
            if (obj is IKey<KeyType> Ikey)
            {
                // Debug.Log("EqualsKey" + typeof(GameKeyType)+"handler"+TypeSelect_()+"  and  "+ IValue.TypeSelect_()+"end"+ TypeSelect_()?.Equals(IValue.TypeSelect_()));
                return (KeySelf()?.Equals(Ikey.KeySelf())) ?? false;
            }
            else if (obj is string s && typeof(string) == typeof(KeyType))
            {
                //  Debug.Log("str" + (TypeSelect_())+"/ "+s);
                return (KeySelf())?.Equals(s) ?? false;
            }
            else if (obj is int i && typeof(int) == typeof(KeyType))
            {
                // Debug.Log("int" + typeof(GameKeyType));
                return (KeySelf()?.Equals(i)) ?? false;
            }
            else if (obj is Enum e && obj.GetType() == typeof(KeyType))
            {
                return (KeySelf()?.Equals(e)) ?? false;
            }
            else if (obj != null && typeof(KeyType) == obj.GetType())
            {
                Debug.Log("other" + typeof(KeyType));
                return KeySelf()?.Equals((KeyType)obj) ?? false;
            }
            return base.Equals(obj);
        }
    }
    [Serializable]
    public class KeyString_Direct : KeyString
    {
        [LabelText("直接输入字符串键")] public string str_direc = "键";
        public override string KeySelf()
        {
            if (str_direc != null) return str_direc;
            return base.KeySelf();
        }
    }
}
