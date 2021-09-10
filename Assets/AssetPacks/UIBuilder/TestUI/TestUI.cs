using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class TestUI : EditorWindow
{
  [MenuItem("Window/UI Toolkit/TestUI")]
  public static void ShowExample()
  {
    TestUI wnd = GetWindow<TestUI>();
    wnd.titleContent = new GUIContent("TestUI");
  }

  public void CreateGUI()
  {
    // Each editor window contains a root VisualElement object
    VisualElement root = rootVisualElement;

    // Import UXML
    var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/AssetPacks/UIBuilder/TestUI/TestUI.uxml");
    visualTree.CloneTree(root);

    // A stylesheet can be added to a VisualElement.
    // The style will be applied to the VisualElement and all of its children.
    var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/AssetPacks/UIBuilder/TestUI/TestUI.uss");
    root.styleSheets.Add(styleSheet);
  }
}