using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    [ESDisplayNameKeyToType("数据单元", "角色数据单元")]
    public class ActorDataInfo : SoDataInfo,IWithSharedAndVariableData<ESEntitySharedData, ESEntityVariableData>,IMessageStringProvider,IMessageSpriteProvider
    {
        [LabelText("实体共享数据")]
        public ESEntitySharedData entitySharedData;
        public string testTitle = "huj";
        public string testStory = "sto";
        public Sprite sprite;
        [LabelText("实体变量数据")]
        public ESEntityVariableData entityVariableData;

        public ESEntitySharedData SharedData { get => entitySharedData; set => entitySharedData=value; }
        public ESEntityVariableData VariableData { get => entityVariableData; set => entityVariableData=value; }
        public string GetMessage(MessageStringKey k, EnumCollect.LanguageType language = EnumCollect.LanguageType.NotClear, int hepler = 0)
        {
            switch (k)
            {
                case MessageStringKey.Name:return entitySharedData.enemyName;
                case MessageStringKey.Title: return testTitle;
                case MessageStringKey.Story:return testStory;
            }
            return "NULL";
        }

        public Sprite GetMessage(MessageSpriteKey k, EnumCollect.LanguageType language = EnumCollect.LanguageType.NotClear, int hepler = 0)
        {
            switch (k)
            {
                case MessageSpriteKey.DefaultIcon:return sprite;
            }
            return sprite;
        }
    }
}
