using UnityEngine;
using System.Collections.Generic;
using System;

public class Node : MonoBehaviour {

    public int NodeId { get; private set; }

    public delegate void NodeCallback(Node node);
    public static event NodeCallback NodeTriggered;

    public void InitNode(int nodeId, Vector3 position)
    {
        this.transform.position = position;
        this.NodeId = nodeId;
    }

    public void OnGazeTrigger()
    {
        CallNodeTriggered(this);
    }

    private void CallNodeTriggered(Node node)
    {
        if (NodeTriggered != null)
        {
            NodeTriggered(node);
        }
    }
}
