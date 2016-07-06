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
        GameObject.Find("TapDebug").GetComponent<TextMesh>().text = string.Format("Node tapped at {0} {1} {2}", 
            transform.position.x,
            transform.position.y,
            transform.position.z);
    }

    private void CallNodeTriggered(Node node)
    {
        if (NodeTriggered != null)
        {
            NodeTriggered(node);
        }
    }
}
