using UnityEngine;
using System.Collections.Generic;
using System;

public class Node : MonoBehaviour {

    public int NodeId { get; private set; }
    
    public void InitNode(int nodeId, Vector3 position)
    {
        this.transform.position = position;
        this.NodeId = nodeId;
    }

    public void OnGazeTrigger()
    {
        Signals.CallNodeTriggered(this);
    }
}
