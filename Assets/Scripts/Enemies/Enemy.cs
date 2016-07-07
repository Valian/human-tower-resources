using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public abstract class Enemy : MonoBehaviour
{
    [Range(0f, 50f)]
    public float Speed;

    [Range(0, 40)]
    public int ChaseTimer;

    [Range(0, 1000)]
    public int SearchRadius;

    [Range(0, 500)]
    public int ClideRange;

    public enum GhostType
    {
        Blinky,
        Pinky,
        Inky,
        Clide
    };
    public enum MovingPattern
    {
        Chase,
        Scatter,
        Frightened
    }
    public GhostType ghostType;
    public MovingPattern movingPattern;

    protected bool IsMoving;
    protected bool IsInitialized;
    protected Node currentNode;
    protected Node targetNode;
    protected Edge movingOnEdge;
    //private Node previousNode;

    //private float timer;
    protected GameObject player;
    protected Vector3 targetPosition;

    // Use this for initialization
    void Start()
    {
        IsInitialized = false;

        InvokeRepeating("ChangeMovingPattern", 5, ChaseTimer);
    }
    public void SetPosition(Node node)
    {
        currentNode = node;
        targetNode = null;

        transform.position = node.transform.position;
        IsMoving = false;
    }
    void Init()
    {
        //MOCK - starting conditions (set starting node and position)
        List<Node> targets = GameObject.FindObjectsOfType<Node>().ToList();
        currentNode = targets[Random.Range(0, targets.Count)];
        gameObject.transform.position = currentNode.transform.position;
        IsMoving = false;
        SearchRadius = 117; //tested and seems fine
        ClideRange = 200;
        ChaseTimer = 10;
        //movingPattern = MovingPattern.Chase;
        player = GameManager.Instance.Player.gameObject;

        IsInitialized = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameRunning) return;
        if (!IsInitialized) Init();
        if (IsMoving)
        {
            var diff = targetPosition - transform.position;
            if (diff.magnitude <= this.Speed * Time.deltaTime)
            {
                this.SetPosition(targetNode);
                IsMoving = false;
            }
            else
            {
                this.transform.position += diff.normalized * Speed * Time.deltaTime;
            }
        }
        else
        {
            if(transform.position != player.transform.position) SetNewTarget();
            //SetNewTarget();
        }
        
    }
    void SetNewTarget()
    {
        if (movingPattern == MovingPattern.Chase)
        {
            ChaseMove();
        }
        else if (movingPattern == MovingPattern.Scatter)
        {
            ScatterMove();
        }
        else if (movingPattern == MovingPattern.Frightened)
        {
            FrightendMove();
        }

    }
    protected abstract void ChaseMove();

    protected void ScatterMove()
    {
        int axis = Random.Range(0, 2);
        int direction = Random.Range(0, 1);
        float value = 0f;
        foreach(Node n in GameObject.FindObjectsOfType<Node>().ToList())
        {
            if(direction == 0)
            {
                if (n.transform.position[axis] > value && GameManager.Instance.NodeManagerInstance.IsConnected(currentNode.NodeId, n.NodeId))
                {
                    value = n.transform.position[axis];
                    targetNode = n;
                    targetPosition = n.transform.position;
                }
            }
            else
            {
                if(n.transform.position[axis] < value && GameManager.Instance.NodeManagerInstance.IsConnected(currentNode.NodeId, n.NodeId))
                {
                    value = n.transform.position[axis];
                    targetNode = n;
                    targetPosition = n.transform.position;
                }
            }
            
        }
        IsMoving = true;
    }
    void FrightendMove()
    {
        List<Node> neighbourNodes = new List<Node>();
        foreach (Node n in GameObject.FindObjectsOfType<Node>().ToList())
        {
            if (GameManager.Instance.NodeManagerInstance.IsConnected(currentNode.NodeId, n.NodeId))
            {
                neighbourNodes.Add(n);
            }
        }
        int chosenNode = Random.Range(0, neighbourNodes.Count);
        targetNode = neighbourNodes[chosenNode];
        targetPosition = targetNode.transform.position;
        IsMoving = true;
    }
    protected void ChaseBlinky()
    {
        if(GameManager.Instance.Player.Movement.CurrentNode && GameManager.Instance.NodeManagerInstance.IsConnected(GameManager.Instance.Player.Movement.CurrentNode.NodeId, currentNode.NodeId))
        {
            targetPosition = GameManager.Instance.Player.transform.position;
            targetNode = GameManager.Instance.Player.Movement.CurrentNode;
            IsMoving = true;
            return; 
        }
        Collider[] hitColliders = FindColiders(SearchRadius);
        foreach (Collider col in hitColliders)
        {
            Node colObject = col.gameObject.GetComponent<Node>();
            if (colObject != null)
            {
                if(GameManager.Instance.NodeManagerInstance.IsConnected(currentNode.NodeId, colObject.NodeId))
                {
                    targetPosition = colObject.transform.position;
                    targetNode = colObject;

                    IsMoving = true;
                }
            }
        }
    }
    protected void ChaseWithVector(Vector3 vec)
    {
        if (GameManager.Instance.NodeManagerInstance.IsConnected(GameManager.Instance.Player.Movement.CurrentNode.NodeId, currentNode.NodeId))
        {
            targetPosition = GameManager.Instance.Player.transform.position;
            targetNode = GameManager.Instance.Player.Movement.CurrentNode;
            IsMoving = true;
            return;
        }
        Collider[] hitColliders = FindColiders(vec, SearchRadius);
        foreach (Collider col in hitColliders)
        {
            Node colObject = col.gameObject.GetComponent<Node>();
            if (colObject != null)
            {
                if (GameManager.Instance.NodeManagerInstance.IsConnected(currentNode.NodeId, colObject.NodeId))
                {
                    targetPosition = colObject.transform.position;
                    targetNode = colObject;

                    IsMoving = true;
                }
            }
        }
    }
    protected Collider[] FindColiders(int radius)
    {
        Collider[] result = Physics.OverlapSphere(Vector3.MoveTowards(transform.position, player.transform.position, radius), radius);
        while(result.Length == 0)
        {
            result = FindColiders(2 * radius);
        }
        return result;
    }
    protected Collider[] FindColiders(Vector3 vec, int radius)
    {
        Collider[] result = Physics.OverlapSphere(vec, radius);
        while (result.Length == 0)
        {
            result = FindColiders(2 * radius);
        }
        return result;
    }
    public void ChangeMovingPattern()
    {
        if (movingPattern == MovingPattern.Chase)
        {
            movingPattern = MovingPattern.Scatter;
        }
        else
        {
            movingPattern = MovingPattern.Chase;
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            player.GetComponent<PlayerStats>().GetHit();
        }
    }
}
