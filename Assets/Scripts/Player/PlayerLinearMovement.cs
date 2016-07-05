using UnityEngine;
using System.Collections;


public class PlayerLinearMovement : MonoBehaviour {

    [Range(0, 30f)]
    public float Speed;
    public bool IsMoving { get { return targetNode != null; } }
    public bool CanMove { get { return !IsMoving; } }
    public Node CurrentNode { get { return currentNode; } }

    private Node currentNode;
    private Node targetNode;
    

    public void SetPosition(Node node)
    {
        currentNode = node;
        targetNode = null;
        transform.position = node.transform.position;
    }

    public void MoveTo(Node target)
    {
        currentNode = null;
        targetNode = target;
    }
	
	void Update ()
    {
        if (IsMoving)
        {
            var diff = targetNode.transform.position - transform.position;
            if (diff.magnitude <= this.Speed * Time.deltaTime)
            {
                this.SetPosition(targetNode);
            }
            else
            {
                this.transform.position += diff.normalized * Speed * Time.deltaTime;
            }
        }
	}
}
