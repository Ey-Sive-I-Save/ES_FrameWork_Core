using ES;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Test : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/Test")]
    public static void ShowExample()
    {
        Test wnd = GetWindow<Test>();
        wnd.titleContent = new GUIContent("Test");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        var nodeTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UITookit/Bas/Test.uxml");

        nodeTree.CloneTree(root);

        var sheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/UITookit/USSGridBack.uss");
        root.styleSheets.Add(sheet);
        if (root != null)
        {
            Button button = new Button();
            if (button != null)
            {
                root.Add(button);
                button.text = "sdasd";
                button.name = "ss";
                button.SetEnabled(true);
                button.style.width= 100;
                button.style.height = 100;
                button.style.backgroundColor = Color.blue;
                button.RegisterCallback<ClickEvent>((a) => { Debug.Log(666); });
                Debug.Log("AAAA" + root.styleSheets[0]);
            }
            else
            {
                Debug.Log("BBB");
            }

            var ruler = new TimeLineRuler();
            root.Add(ruler);
        }
        // var nodeTreeView = root.Q<Viewer>();
                                                                                                                                                                                                                                                                                                                                                                                                                                                        
        // Instantiate UXML
       /* VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);*/
    }
}
