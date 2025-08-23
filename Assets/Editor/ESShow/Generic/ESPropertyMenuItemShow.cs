using ES;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace ES {
    [InitializeOnLoad]
    public class ESPropertyMenuItemShow
    {
        static ESPropertyMenuItemShow()
        {
            EditorApplication.contextualPropertyMenu += OnPropertyMenu;
        }

        private static void OnPropertyMenu(GenericMenu menu, SerializedProperty property)
        {
            Debug.Log("ss");
            Debug.Log(property+"666"+property.propertyType);
            // 仅对float类型属性生效
            if (property.propertyType == SerializedPropertyType.Float)
            {
                menu.AddItem(new GUIContent("重置为0"), false, () =>
                {
                    property.floatValue = 0;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
        }
    }
}
