using UnityEngine;
using System.Collections;

public static class Signals {

    public delegate void NodeEvent(Node node);
    public static event NodeEvent NodeTriggered;
    public static event NodeEvent NodeLeft;
    public static event NodeEvent NodeTargeted;
    public static event NodeEvent NodeReached;    

    public static void CallNodeTriggered(Node node)
    {
        if(NodeTriggered != null)
        {
            NodeTriggered(node);
        }
    }

    public static void CallNodeTargeted(Node node)
    {
        if (NodeTargeted != null)
        {
            NodeTargeted(node);
        }
    }

    public static void CallNodeReached(Node node)
    {
        if (NodeReached != null)
        {
            NodeReached(node);
        }
    }


    public static void CallNodeLeft(Node node)
    {
        if (NodeLeft != null)
        {
            Signals.NodeLeft(node);
        }
    }    
}
