using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;

namespace RFG.SceneGraph
{
  public class SceneGraphEditor : EditorWindow
  {
    private SceneGraph graph;
    private SceneGraphView graphView;
    private VisualElement overlay;
    private InspectorView inspectorView;

    [MenuItem("RFG/Scene Graph Editor")]
    public static void OpenWindow()
    {
      SceneGraphEditor wnd = GetWindow<SceneGraphEditor>();
      wnd.titleContent = new GUIContent("SceneGraphEditor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
      if (Selection.activeObject is SceneGraph)
      {
        OpenWindow();
        return true;
      }
      return false;
    }

    public void CreateGUI()
    {
      VisualElement root = rootVisualElement;

      var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/AssetPacks/RFGScene/UIBuilder/SceneGraphEditor.uxml");
      visualTree.CloneTree(root);

      var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/AssetPacks/RFGScene/UIBuilder/SceneGraphEditor.uss");
      root.styleSheets.Add(styleSheet);

      graphView = root.Q<SceneGraphView>();
      graphView.OnNodeSelected = OnNodeSelectionChanged;

      // Inspector View
      inspectorView = root.Q<InspectorView>();

      overlay = root.Q<VisualElement>("Overlay");

      if (graph == null)
      {
        OnSelectionChange();
      }
      else
      {
        SelectGraph(graph);
      }
    }

    private void OnSelectionChange()
    {
      EditorApplication.delayCall += () =>
      {
        SceneGraph sceneGraph = Selection.activeObject as SceneGraph;
        if (!sceneGraph)
        {
          return;
        }
        SelectGraph(sceneGraph);
      };
    }

    private void SelectGraph(SceneGraph sceneGraph)
    {
      if (graphView == null)
      {
        return;
      }

      if (!sceneGraph)
      {
        return;
      }

      this.graph = sceneGraph;

      // overlay.style.visibility = Visibility.Hidden;

      if (Application.isPlaying)
      {
        graphView.PopulateView(graph);
      }
      else
      {
        graphView.PopulateView(graph);
      }

      EditorApplication.delayCall += () =>
      {
        graphView.FrameAll();
      };
    }

    private void OnNodeSelectionChanged(NodeView nodeView)
    {
      inspectorView.UpdateSelection(nodeView);
    }

    private void OnInspectorUpdate()
    {
      graphView?.UpdateNodeStates();
    }
  }
}