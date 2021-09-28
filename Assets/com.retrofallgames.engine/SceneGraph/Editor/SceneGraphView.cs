using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;

namespace RFG.SceneGraph
{
  public class SceneGraphView : GraphView
  {
    public new class UxmlFactory : UxmlFactory<SceneGraphView, GraphView.UxmlTraits> { }

    public Action<NodeView> OnNodeSelected;
    private SceneGraph graph;

    public SceneGraphView()
    {
      Insert(0, new GridBackground());

      this.AddManipulator(new ContentZoomer());
      this.AddManipulator(new ContentDragger());
      this.AddManipulator(new SelectionDragger());
      this.AddManipulator(new RectangleSelector());

      var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/AssetPacks/RFGScene/UIBuilder/SceneGraphEditor.uss");
      styleSheets.Add(styleSheet);
    }

    internal void PopulateView(SceneGraph graph)
    {
      this.graph = graph;

      graphViewChanged -= OnGraphViewChanged;
      DeleteElements(graphElements);
      graphViewChanged += OnGraphViewChanged;

      // Create Node Views
      graph.sceneNodes.ForEach(n => CreateNodeView(n));

      // Create Edges
      // tree.nodes.ForEach(n =>
      // {
      //   var children = BehaviourTree.GetChildren(n);
      //   children.ForEach(c =>
      //   {
      //     NodeView parentView = FindNodeView(n);
      //     NodeView childView = FindNodeView(c);

      //     Edge edge = parentView.output.ConnectTo(childView.input);
      //     AddElement(edge);
      //   });
      // });
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
      return ports.ToList(); //All ports are compatible with all other ports.
    }

    private void CreateNode(System.Type type, Vector2 position)
    {
      SceneNode node = graph.CreateNode(type);
      node.position = position;
      CreateNodeView(node);
    }

    private void CreateNodeView(SceneNode node)
    {
      NodeView nodeView = new NodeView(node);
      nodeView.title = node.name;

      Label sceneName = nodeView.Q<Label>("scene-name-label");
      sceneName.text = node.SceneName;

      VisualElement doors = nodeView.Q<VisualElement>("doors");

      doors.Add(new Label("Hello"));

      nodeView.OnNodeSelected = OnNodeSelected;
      AddElement(nodeView);
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
      if (graphViewChange.elementsToRemove != null)
      {
        graphViewChange.elementsToRemove.ForEach(elem =>
        {
          NodeView nodeView = elem as NodeView;
          if (nodeView != null)
          {
            graph.DeleteNode(nodeView.sceneNode);
          }
            // Edge edge = elem as Edge;
            // if (edge != null)
            // {
            //   NodeView parentView = edge.output.node as NodeView;
            //   NodeView childView = edge.input.node as NodeView;
            //   graph.RemoveConnection(parentView.node, childView.node);
            // }
          });
      }
      // if (graphViewChange.edgesToCreate != null)
      // {
      //   graphViewChange.edgesToCreate.ForEach(edge =>
      //   {
      //     NodeView parentView = edge.output.node as NodeView;
      //     NodeView childView = edge.input.node as NodeView;
      //     graph.AddChild(parentView.node, childView.node);
      //   });
      // }

      // nodes.ForEach((n) =>
      // {
      //   NodeView nodeView = n as NodeView;
      //   nodeView.SortChildren();
      // });

      return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
      // base.BuildContextualMenu(evt);
      Vector2 nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
      evt.menu.AppendAction("Create New Node", (a) => CreateNode(typeof(SceneNode), nodePosition));
    }

    public void UpdateNodeStates()
    {
      graph.sceneNodes.ForEach((n) =>
      {
        NodeView nodeView = GetNodeByGuid(n.guid) as NodeView;
        if (nodeView != null)
        {
          nodeView.UpdateState();
        }
      });
    }
  }
}