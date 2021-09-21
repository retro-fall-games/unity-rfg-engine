using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace RFG
{
  namespace SceneGraph
  {
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
      public Action<NodeView> OnNodeSelected;
      public SceneNode sceneNode;
      public List<Port> connections = new List<Port>();

      public NodeView(SceneNode sceneNode) : base("Assets/AssetPacks/RFGSceneGraph/UIBuilder/NodeView.uxml")
      {
        this.sceneNode = sceneNode;
        this.sceneNode.name = sceneNode.GetType().Name;
        this.title = sceneNode.SceneName;
        this.viewDataKey = sceneNode.guid;

        style.left = sceneNode.position.x;
        style.top = sceneNode.position.y;
      }

      public override void SetPosition(Rect newPos)
      {
        base.SetPosition(newPos);
        sceneNode.position.x = newPos.xMin;
        sceneNode.position.y = newPos.yMin;
        EditorUtility.SetDirty(sceneNode);
      }

      public override void OnSelected()
      {
        base.OnSelected();
        OnNodeSelected?.Invoke(this);
      }

      private void CreateConnections()
      {
        // Port p = new Port();
        NodePort port = new NodePort(Direction.Input, Port.Capacity.Single);

        if (port != null)
        {
          port.portName = "";
          inputContainer.Add(port);
        }
      }

      public void UpdateState()
      {
        title = sceneNode.SceneName;
        sceneNode.name = sceneNode.SceneName;
      }

    }
  }
}