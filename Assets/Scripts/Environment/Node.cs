using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class Node : MonoBehaviour {

    public float HideTime = 1;
    public int NodeId { get; private set; }

    public delegate void NodeCallback(Node node);
    public static event NodeCallback NodeTriggered;

    private Vector3 initialScale;

    IEnumerator coroutine = null;

    void Awake()
    {
        initialScale = transform.localScale;
    }

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

    public void Hide()
    {
        if(coroutine == null)
        {
            coroutine = HideCoroutine();
            StartCoroutine(coroutine);
        }
    }

    public void Show()
    {
        transform.localScale = initialScale;
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private IEnumerator HideCoroutine()
    {
        var time = Time.time;
        while(Time.time < time + HideTime)
        {
            var scale = 1 + ((time - Time.time) * 1f / HideTime);
            transform.localScale = initialScale * scale;
            yield return null;
        }
        transform.localScale = new Vector3(0, 0, 0);
    }
}
