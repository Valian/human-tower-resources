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

    [Range(0, 40)]
    public int FrightenedTimer;

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
    public Material frightenedMaterial;

    protected bool IsMoving;
    //protected bool IsInitialized;
    protected bool IsFrightened;
    private bool HasRoute;
    protected Node currentNode;
    protected Node targetNode;
    protected Edge movingOnEdge;
    //private Node previousNode;

    //private float timer;
    protected GameObject player;
    private Vector3 _targetPosition;
    protected Vector3 targetPosition
    {
        get { return _targetPosition; }
        set
        {
            _targetPosition = value;
            transform.LookAt(targetPosition);
        }
    } 
    private bool playerMoved = false;
    private Material assignedMaterial;


    // Use this for initialization
    void Start()
    {
        //IsInitialized = false;
        GameManager.Instance.Player.Movement.FirstMoveChanged += Movement_FirstMoveDone;
        InvokeRepeating("ChangeMovingPattern", 5, ChaseTimer);
        GameManager.Instance.LifeLost += Instance_LifeLost;
        GameManager.Instance.PowerDotCollected += FrightenEnemy;
        assignedMaterial = material;
    }

    private Material material
    {
        get
        {
            return transform.Find("Mesh").GetComponent<MeshRenderer>().material;
        }
        set
        {
            transform.Find("Mesh").GetComponent<MeshRenderer>().material = value;
        }
    }

    private void Instance_LifeLost()
    {
        SetPosition(targetNode);
    }

    public void SetPosition(Node node)
    {
        if (node == null)
        {
            return;
        }
        currentNode = node;
        targetNode = null;
        transform.position = node.transform.position;
        IsMoving = false;
        HasRoute = false;
    }
    public void Init(Node startingNode)
    //public void Init()
    {
        //MOCK - starting conditions (set starting node and position)
        //List<Node> targets = GameObject.FindObjectsOfType<Node>().ToList();
        //currentNode = targets[Random.Range(0, targets.Count)];
        //gameObject.transform.position = currentNode.transform.position;
        //transform.position = GetGraphCenter();
        //currentNode = ChoseRandomNode();
        currentNode = startingNode;
        transform.position = currentNode.transform.position;

        IsMoving = false;
        IsFrightened = false;
        HasRoute = false;
        SearchRadius = 117; //tested and seems fine
        ClideRange = 200;
        ChaseTimer = 10;
        FrightenedTimer = 15;
        Speed = 25;
        //movingPattern = MovingPattern.Chase;
        player = GameManager.Instance.Player.gameObject;

        //GoToRandomNode();
        //IsInitialized = true;
    }
    //private Vector3 GetGraphCenter()
    //{
    //    int len = GameManager.Instance.GraphManagerInstance.Nodes.Length;
    //    Vector3 result = new Vector3();
    //    foreach (Node n in GameManager.Instance.GraphManagerInstance.Nodes)
    //    {
    //        result += n.transform.position;
    //    }
    //    return result / len;
    //    //return new Vector3(result.x / n, result.y / n, result.z / n);
    //}
    private Node ChoseRandomNode()
    {
        Node[] nodes = GameManager.Instance.GraphManagerInstance.Nodes;
        Node playerNode = GameManager.Instance.Player.Movement.CurrentNode;
        int r = Random.Range(0, nodes.Length - 1);
        
        while (nodes.ElementAt(r) == playerNode || GameManager.Instance.GraphManagerInstance.IsConnected(nodes.ElementAt(r).NodeId, playerNode.NodeId))
        {
            r = Random.Range(0, GameManager.Instance.GraphManagerInstance.Nodes.Length - 1);
        }
        return GameManager.Instance.GraphManagerInstance.Nodes[r];
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameRunning) return;
        //if (!IsInitialized) Init();
        if (IsMoving && playerMoved)
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
            if(transform.position != player.transform.position && !HasRoute) SetNewTarget();
        }
        
    }
    void SetNewTarget()
    {
        Debug.Log("Changing Target");
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
        if(targetNode != null)HasRoute = true;

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
                if (n.transform.position[axis] > value && GameManager.Instance.GraphManagerInstance.IsConnected(currentNode.NodeId, n.NodeId))
                {
                    value = n.transform.position[axis];
                    targetNode = n;
                    targetPosition = n.transform.position;
                }
            }
            else
            {
                if(n.transform.position[axis] < value && GameManager.Instance.GraphManagerInstance.IsConnected(currentNode.NodeId, n.NodeId))
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
            if (GameManager.Instance.GraphManagerInstance.IsConnected(currentNode.NodeId, n.NodeId))
            {
                neighbourNodes.Add(n);
            }
        }
        int chosenNode = Random.Range(0, neighbourNodes.Count-1);
        targetNode = neighbourNodes[chosenNode];
        targetPosition = targetNode.transform.position;
        IsMoving = true;
    }

    protected void ChaseBlinky()
    {
        ChaseWithDjikstra();
        return;
        Node playerNode = GameManager.Instance.Player.Movement.CurrentNode ?? GameManager.Instance.Player.Movement.TargetNode;
        if (GameManager.Instance.GraphManagerInstance.IsConnected(playerNode.NodeId, currentNode.NodeId))
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
                if(GameManager.Instance.GraphManagerInstance.IsConnected(currentNode.NodeId, colObject.NodeId))
                {

                    targetNode = colObject;
                    targetPosition = colObject.transform.position;
                    if (targetNode == currentNode || targetNode == null)
                    {
                        ChaseWithDjikstra();
                    }
                    IsMoving = true;
                }
            }
        }
    }

    protected void ChaseWithVector(Vector3 vec)
    {
        ChaseWithDjikstra();
        return;
        if (GameManager.Instance.GraphManagerInstance.IsConnected(GameManager.Instance.Player.Movement.CurrentNode.NodeId, currentNode.NodeId))
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
                if (GameManager.Instance.GraphManagerInstance.IsConnected(currentNode.NodeId, colObject.NodeId))
                {
                    targetNode = colObject;
                    targetPosition = colObject.transform.position;
                    if (targetNode == currentNode || targetNode == null)
                    {
                        ChaseWithDjikstra();
                    }
                    IsMoving = true;
                }
            }
        }
    }
    protected void ChaseWithDjikstra()
    {
        GraphManager graphManager = GameManager.Instance.GraphManagerInstance;
        
        Node playerNode = GameManager.Instance.Player.Movement.CurrentNode ?? GameManager.Instance.Player.Movement.TargetNode;
        if(graphManager.IsConnected(currentNode.NodeId, playerNode.NodeId))
        {
            targetNode = playerNode;
            targetPosition = targetNode.transform.position;
            return;
        }

        List<int>[] pathsList = graphManager.GetPath(currentNode.NodeId);
        int targetNodeId = pathsList[playerNode.NodeId].ElementAt(0);
        while (!graphManager.IsConnected(currentNode.NodeId, targetNodeId))
        {
            targetNodeId = pathsList[targetNodeId].ElementAt(0);
        }
        targetNode = GameObject.FindObjectsOfType<Node>().ToList().Find(x => x.NodeId == targetNodeId);
        targetPosition = targetNode.transform.position;
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
        else if (movingPattern == MovingPattern.Scatter)
        {
            movingPattern = MovingPattern.Chase;
        }
        else if (!IsFrightened)
        {
            movingPattern = MovingPattern.Chase;
        }
    }
    private void ChangeIsFrightened()
    {
        Speed = 25;
        material = assignedMaterial;
        IsFrightened = false;
    }
    public void FrightenEnemy()
    {
        IsFrightened = true;
        movingPattern = MovingPattern.Frightened;
        Speed = 15;
        material = frightenedMaterial;
        targetNode = currentNode;
        targetPosition = targetNode.transform.position;
        Invoke("ChangeIsFrightened", FrightenedTimer);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            player.GetComponent<PlayerStats>().GetHit();
        }
    }

    private void Movement_FirstMoveDone(bool val)
    {
        playerMoved = val;
    }
}
