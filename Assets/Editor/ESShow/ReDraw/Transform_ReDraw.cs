using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine.UIElements;

namespace ES
{

    public enum TransformSpace
    {
        [InspectorName("本地")] Local,
        [InspectorName("世界")] World,
        [InspectorName("相对")] Rele
    }
    [CustomEditor(typeof(Transform)), CanEditMultipleObjects]
    public class CustomTransformEditor : Editor
    {
        private float lastHeight;
        private TransformSpace transformSpace;
        private Transform transform;
        public override void OnInspectorGUI()
        {
            #region START
            EditorGUILayout.Space(0);
            var space = GUILayoutUtility.GetLastRect();
            var startY1 = space.yMax;
            #endregion


            SirenixEditorGUI.DrawBorders(space, (int)space.width+10, 0, (int)lastHeight + 2, 0, Color.black);
            

            Rect rect = space.SetYMax(space.yMin + lastHeight);

            transformSpace = Enum.Parse<TransformSpace>(EditorGUILayout.EnumPopup(transformSpace).ToString());

            if (transformSpace == TransformSpace.Local)
            {
                // 显示默认属性（位置、旋转、缩放）
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LocalPosition"));
                if (GUILayout.Button("重置", GUILayout.MaxWidth(35)))
                {
                    (target as Transform).localPosition = Vector3.zero;
                }
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LocalRotation"));
                if (GUILayout.Button("重置", GUILayout.MaxWidth(35)))
                {
                    (target as Transform).localRotation = Quaternion.identity;
                }
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LocalScale"));
                if (GUILayout.Button("重置", GUILayout.MaxWidth(35)))
                {
                    (target as Transform).localScale = Vector3.one;
                }
                EditorGUILayout.EndHorizontal();

            }
            else if (transformSpace == TransformSpace.World)
            {
                Transform tt = (target as Transform);
                // 显示默认属性（位置、旋转、缩放）
                EditorGUILayout.BeginHorizontal();
                tt.transform.position = EditorGUILayout.Vector3Field("世界坐标", tt.transform.position);
                if (GUILayout.Button("重置", GUILayout.MaxWidth(35)))
                {
                    tt.position = Vector3.zero;
                }
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                tt.transform.rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("世界旋转", tt.transform.rotation.eulerAngles));
                if (GUILayout.Button("重置", GUILayout.MaxWidth(35)))
                {
                    tt.rotation = Quaternion.identity;
                }
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                GUI.enabled = false;
                // 恢复编辑功能
                EditorGUILayout.Vector3Field("世界缩放", tt.transform.lossyScale);
                GUI.enabled = true;

                if (GUILayout.Button("重置", GUILayout.MaxWidth(35)))
                {
                    var fromONE = Vector3.one;
                    if (tt.parent != null)
                    {
                        var pa = tt.parent.transform.lossyScale;
                        fromONE = new Vector3(fromONE.x._SafeDivide(pa.x), fromONE.y._SafeDivide(pa.y), fromONE.z._SafeDivide(pa.z));
                    }
                    tt.transform.localScale = fromONE;
                }
                EditorGUILayout.EndHorizontal();
            }
            else if (transformSpace == TransformSpace.Rele)
            {
                transform = (Transform)EditorGUILayout.ObjectField(transform, typeof(UnityEngine.Transform), transform);
                Transform tt = (target as Transform);
                if (transform != null)
                {
                    // 显示默认属性（位置、旋转、缩放）
                    EditorGUILayout.BeginHorizontal();



                    tt.transform.position = transform.TransformPoint(EditorGUILayout.Vector3Field("相对坐标", transform.InverseTransformPoint(tt.transform.position)));
                    if (GUILayout.Button("重置", GUILayout.MaxWidth(35)))
                    {
                        tt.position = transform.position;
                    }
                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginHorizontal();
                    tt.transform.rotation = transform.rotation * Quaternion.Euler(EditorGUILayout.Vector3Field("相对旋转", (Quaternion.Inverse(transform.rotation) * tt.transform.rotation).eulerAngles));
                    if (GUILayout.Button("重置", GUILayout.MaxWidth(35)))
                    {
                        tt.rotation = transform.rotation;
                    }
                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginHorizontal();
                    GUI.enabled = false;
                    EditorGUILayout.Vector3Field("相对缩放", tt.transform.lossyScale._DivideVector3(transform.lossyScale));
                    GUI.enabled = true;

                    if (GUILayout.Button("重置", GUILayout.MaxWidth(35)))
                    {
                        var fromONE = transform.lossyScale;
                        if (tt.parent != null)
                        {
                            var pa = tt.parent.transform.lossyScale;
                            fromONE = fromONE._DivideVector3(pa);
                        }
                        tt.transform.localScale = fromONE;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            serializedObject.ApplyModifiedProperties();

            #region END
            float startY2 = GUILayoutUtility.GetLastRect().yMax;
            float f = startY2 - startY1;
            lastHeight = f > 0 ? f : lastHeight;

            #endregion
        }
    }
}
