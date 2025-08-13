/*using UnityEditor;
using UnityEngine;

public class WizardCreateLight : ScriptableWizard
{
    public float range = 500;
    public Color color = Color.red;

    [MenuItem("GameObject/Create Light Wizard")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create", "Apply");
        //If you don't want to use the secondary button simply leave it out:
        //ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
    }

    void OnWizardCreate()
    {
        GameObject go = new GameObject("New Light");
        Light lt = go.AddComponent<Light>();
        lt.range = range;
        lt.color = color;
    }

    void OnWizardUpdate()
    {
        helpString = "Please set the color of the light!";
    }

    // When the user presses the "Apply" button OnWizardOtherButton is called.
    void OnWizardOtherButton()
    {
        if (Selection.activeTransform != null)
        {
            Light lt = Selection.activeTransform.GetComponent<Light>();

            if (lt != null)
            {
                lt.color = Color.red;
            }
        }
    }
}*/

using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine.UIElements;

// Specifying `OverlayAttribute.editorWindowType` tells the OverlayCanvas to always show this Overlay in the menu.
[Overlay(typeof(SceneView), "Selection Count")]
class SelectionCount : Overlay
{
    Label m_Label;

    public override VisualElement CreatePanelContent()
    {
        Selection.selectionChanged += () =>
        {
            if (m_Label != null)
                m_Label.text = $"Selection Count {Selection.count}";
        };

        return m_Label = new Label($"Selection Count {Selection.count}");
    }
}