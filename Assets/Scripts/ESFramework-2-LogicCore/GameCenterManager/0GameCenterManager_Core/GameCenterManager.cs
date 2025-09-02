using ES.Pointer;
using FishNet.Broadcast;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static ES.SkillPointDataInfo.SkillPointLevelAllTransfomor;

namespace ES
{
    public struct aa : IBroadcast
    {

    }
    [DefaultExecutionOrder(-8)]
    [TypeRegistryItem("游戏核心管理器")]
    public partial class GameCenterManager : SingletonAsCore<GameCenterManager>,IReceiveLink<aa>
    {


        [NonSerialized]public bool NetSupport = false;
        [FoldoutGroup("工具管理器引用"), LabelText("音效管理器")] public ESAudioMaster AudioMaster;
        

      /*  [LabelText("基本域")]public DomainForGameCenterManager NormalDomain;*/
        


        [FoldoutGroup("事件委托")] public UnityEvent<Scene, LoadSceneMode> OnSceneLoaded;
        [FoldoutGroup("事件委托")] public UnityEvent OnLevelEndSave;
        [FoldoutGroup("事件委托")] public UnityEvent<int> OnSoulCollected;
        [FoldoutGroup("事件委托")] public UnityEvent OnSuccessParriedMeleeATK;
        [FoldoutGroup("事件委托")] public UnityEvent OnSuccessParriedRangeATK;
        [FoldoutGroup("事件委托")] public UnityEvent OnGuardBreaked;
        [FoldoutGroup("数据包")]



       

     

        [FoldoutGroup("编辑器支持"),LabelText("默认GUI Style")]
        public GUIStyle style;

        
      

        protected override void OnBeforeAwakeRegister()
        {
            GameCenterManager.LinkReceivePoolForServer.AddRecieveWithNet<aa>(this);
            base.OnBeforeAwakeRegister();

            Part_Link_OnBeforeAwakeRegister();

            SceneManager.sceneUnloaded += (a) => { this.StopAllCoroutines(); };
        }
        
        [Button("发送任意Link")]
        public void SendLink<Link>(Link link) where Link:ILink
        {
         
        }

        public void OnLink(aa link)
        {
            throw new NotImplementedException();
        }


        #region 生成相关

        #endregion
        /*[Button("发送Link")]
        public void SendLink()
        {
            
            GameCenterArchitecture.SendLink(new Link_DestrolyCollideWall());
        }
        protected override void BeforeAwakeBroadCastRegister()
        {
            base.BeforeAwakeBroadCastRegister();
            SceneManager.sceneLoaded += PassiveDelagateMethod_OnSceneLoaded;
        }
        
        private void PassiveDelagateMethod_OnSceneLoaded(Scene scene,LoadSceneMode model)
        {
            OnSceneLoaded?.Invoke(scene, model);
        }
        public void ProactiveInvoke_OnLevelEndSave()
        {
            OnLevelEndSave?.Invoke();
        }
        public void ProactiveInvoke_OnEnemyBeAttack(Enemy enemy, DamageClass Class)
        {
            
            OnEnemyBeAttack?.Invoke(enemy,Class);
            if(enemy!=null&&Class!=null&& NormalDomain.Module_PlayerState != null)
            {
                NormalDomain.Module_PlayerState.m_healthPoint += Class.Damage * NormalDomain.Module_PlayerState.mm_VampirePercent*0.05f;
                NormalDomain.Module_PlayerState.m_healthPoint = Mathf.Min(NormalDomain.Module_PlayerState.m_healthPoint, NormalDomain.Module_PlayerState.m_maxHealthPoint);
            }
        }*/
        /// <summary>
        /// 
        /// </summary>
        /// <param content="enemy"> 谁打我</param>
        /// <param content="Class"> 咋打的</param>
        /// <returns>返回的是是否有效攻击</returns>
        /* public bool ProactiveInvoke_OnPlayerBeEnemyAttack_BackApplyAttack(DamageClass Class)
         {
             OnPlayerBeEnemyAttack?.Invoke(Class.who, Class);
             if (Class != null &&Class.who!=null&& NormalDomain.Module_PlayerState != null)
             {
                 Debug.Log("玩家被攻击" + Class.Type + Class.who);
                 if (NormalDomain.Module_PlayerState.isSimpleDefending)
                 {
                     //普通防御
                     NormalDomain.Module_PlayerState.m_blockMeter -= 20;
                     if (NormalDomain.Module_PlayerState.m_blockMeter < 0)
                     {
                         OnGuardBreaked?.Invoke();
                         Debug.Log("护盾破碎");
                     }
                     Debug.Log("护盾抵挡");
                     return false;
                 }else if (NormalDomain.Module_PlayerState.isSpecialGeDang)
                 {

                      if (Class.Type == DamageType.Melle)
                     {
                         OnSuccessParriedMeleeATK?.Invoke();
                         Debug.Log("完美格挡近战");
                     }
                     else if (Class.Type == DamageType.Range)
                     {
                         OnSuccessParriedRangeATK?.Invoke();
                         Debug.Log("完美格挡远程");
                     }
                     return false;
                 }
                 else
                 {
                     return true;
                 }
             }

             return true;
         }
         public void ProactiveInvoke_OnWeaponSetup(WeaponSwitch weaponSwitch)
         {
             if (weaponSwitch != null)
             {
                 OnWeaponSetup?.Invoke(weaponSwitch);
             }
         }
         public void ProactiveInvoke_OnSoulCollected(int num)
         {
             OnSoulCollected?.Invoke(num);
         }
         private void Start()
         {
             var test1 = ESStaticUtility.FindByAKey(buffSoInfos, "暴击强化");
            // Debug.Log(test1!=null ? $"Test1找到了{test1}": "Test1失败"  );

             var test2 = ESStaticUtility.FindByIKey(buffSoInfos, new KeyString_Direct() { str_direc= "暴击强化" });
            // Debug.Log(test1 != null ? $"Test2找到了{test1}" : "Test2失败");
         }

         public override void Update()
         {
             base.Update();

         }*/

    }
}
