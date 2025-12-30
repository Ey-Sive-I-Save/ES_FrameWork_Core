using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;

namespace ES
{
    public class Viewer : GraphView
    {
        public new class UxmlFactory : UxmlFactory<Viewer, GraphView.UxmlTraits>
        {

        }
        public Viewer()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            var i = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/UITookit/USSGridBack.uss");
            Debug.Log("ASSET" + i);
            styleSheets.Add(i);
        }
    }
}
