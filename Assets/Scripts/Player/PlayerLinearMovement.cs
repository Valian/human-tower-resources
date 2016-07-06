using UnityEngine;


public class PlayerLinearMovement : MonoBehaviour {

    [Range(0, 50f)]
    public float Speed;
    public bool IsMoving { get { return targetNode != null; } }
    public Node CurrentNode { get { return currentNode; } }
    public Node TargetNode { get { return targetNode; } }

    private Node currentNode;
    private Node targetNode;
    private Node previousNode;
    
    void Start()
    {
        Node.NodeTriggered += MoveTo;
    }
    

    public void SetPosition(Node node)
    {
        currentNode = node;
        targetNode = null;
        previousNode = null;
        transform.position = node.transform.position;
    }

    public void MoveTo(Node target)
    {
        if (target)
        {
            var manager = GameObject.FindObjectOfType<NodeManager>();
            var wantsToGoBack = target == previousNode;
            var canMove = !IsMoving && manager.IsConnected(currentNode.NodeId, target.NodeId);
            if (wantsToGoBack || canMove)
            {
                previousNode = currentNode ? currentNode: targetNode;
                targetNode = target;
                currentNode = null;
            }
        }
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
