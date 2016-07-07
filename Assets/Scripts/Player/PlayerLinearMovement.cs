using System;
using UnityEngine;


public class PlayerLinearMovement : MonoBehaviour {

    [Range(0, 50f)]
    public float Speed;
    public bool IsMoving { get { return TargetNode != null; } }
    public Node CurrentNode { get; private set; }
    public Node TargetNode { get; private set; }
    public Node NextTargetNode { get; private set; }
    public Node PreviousNode { get; private set; }
    public event Action<bool> FirstMoveChanged = delegate { };

    public delegate void PlayerMovement(PlayerLinearMovement player);
    public event PlayerMovement PlayerTargetChanged;
    public event PlayerMovement PlayerTargetReached;
    public event PlayerMovement PlayerNextTargetChanged;

    private bool _firstMoveDone = false;
    void Start()
    {
        Node.NodeTriggered += MoveTo;
    }
    
    public void Spawn(Node node)
    {
        SetPosition(node);
        _firstMoveDone = false;
        FirstMoveChanged(false);
    }

    public void SetPosition(Node node)
    {
        CurrentNode = node;
        TargetNode = null;
        PreviousNode = null;
        CallPlayerTargetChanged();
        CallPlayerTargetReached();
        transform.position = node.transform.position;
    }

    public void MoveTo(Node target)
    {
        if (target)
        {
            if (!_firstMoveDone)
            {
                _firstMoveDone = true;
                FirstMoveChanged(true);
            }
            var manager = GameObject.FindObjectOfType<GraphManager>();
            var wantsToGoBack = target == PreviousNode;
            var canMove = !IsMoving && manager.IsConnected(CurrentNode.NodeId, target.NodeId);
            if (wantsToGoBack || canMove)
            {
                ChangePlayerTarget(target);
            } 
            else if(TargetNode && manager.IsConnected(target.NodeId, TargetNode.NodeId))
            {
                ChangePlayerNextTarget(target);
            }
        }
    }
	
	void Update ()
    {
        if (IsMoving)
        {
            var diff = TargetNode.transform.position - transform.position;
            if (diff.magnitude <= this.Speed * Time.deltaTime)
            {
                this.SetPosition(TargetNode);
                if(NextTargetNode)
                {
                    ChangePlayerTarget(NextTargetNode);
                    NextTargetNode = null;
                }
            }
            else
            {
                this.transform.position += diff.normalized * Speed * Time.deltaTime;
            }
        }
	}

    private void ChangePlayerTarget(Node target)
    {
        PreviousNode = CurrentNode ? CurrentNode : TargetNode;
        TargetNode = target;
        CurrentNode = null;
        NextTargetNode = null;
        CallPlayerTargetChanged();
    }

    private void ChangePlayerNextTarget(Node nextTarget)
    {
        NextTargetNode = nextTarget;
        CallPlayerNextTargetChanged();
    }

    private void CallPlayerNextTargetChanged()
    {
        if (PlayerNextTargetChanged != null)
        {
            PlayerNextTargetChanged(this);
        }
    }

    private void CallPlayerTargetChanged()
    {
        if (PlayerTargetChanged != null)
        {
            PlayerTargetChanged(this);
        }
    }

    private void CallPlayerTargetReached()
    {
        if (PlayerTargetReached != null)
        {
            PlayerTargetReached(this);
        }
    }
}
