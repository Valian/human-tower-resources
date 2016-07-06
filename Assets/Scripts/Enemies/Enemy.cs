using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class Enemy : MonoBehaviour
{
    [Range(0f, 50f)]
    public float Speed;

    [Range(0, 40)]
    public int ChaseTimer;

    [Range(0, 1000)]
    public int SearchRadius;

    //[Range(0, 20)]
    //public int FrightenedTimer;

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

    private bool IsMoving;
    private bool IsInitialized;
    private Node currentNode;
    private Node targetNode;
    private Node previousNode;

    private float timer;
    private GameObject player;
    private Vector3 targetPosition;

    // Use this for initialization
    void Start()
    {
        
        IsInitialized = false;

        //target = GameObject.FindObjectOfType<PlayerLinearMovement>;
        //player = GameManager.Instance.Player.gameObject;
        //InvokeRepeating("ChangeMovingPattern", 5, ChaseTimer);
    }
    public void SetPosition(Node node)
    {
        currentNode = node;
        targetNode = null;
        previousNode = null;
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
        movingPattern = MovingPattern.Chase;
        player = GameManager.Instance.Player.gameObject;

        IsInitialized = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsInitialized) Init();
        if (IsMoving)
        {
            //var diff = player.transform.position - transform.position;
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
            SetNewTarget();
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
    void ChaseMove()
    {
        switch (ghostType)
        {
            case GhostType.Blinky:
                ChaseBlinky();
                break;
            case GhostType.Clide:
                ChaseClide();
                break;
            case GhostType.Inky:
                ChaseInky();
                break;
            case GhostType.Pinky:
                ChasePinky();
                break;
        }
    }
    void ScatterMove()
    {

    }
    void FrightendMove()
    {
        
    }
    // ============== Chase Methods ==============
    void ChaseBlinky()
    {
        //wyznacz sciezke od siebie do gracza
        //targetNode = GameManager.Instance.Player.Movement.CurrentNode;
        Collider[] hitColliders = Physics.OverlapSphere(Vector3.MoveTowards(transform.position, player.transform.position, SearchRadius), SearchRadius);
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
        //Node PlayerNode = GameManager.Instance.Player.Movement.CurrentNode ? GameManager.Instance.Player.Movement.CurrentNode : GameManager.Instance.Player.Movement.TargetNode;
        //Node myNode = currentNode;
        ////wybierz targetNode jako pierwszy wezel
    }
    void ChaseClide()
    {

    }
    void ChaseInky()
    {

    }
    void ChasePinky()
    {

    }
    public void ChangeMovingPattern()
    {
        if (movingPattern == MovingPattern.Chase) movingPattern = MovingPattern.Scatter;
        else movingPattern = MovingPattern.Chase;
    }
    //void RandomStep()
    //{
    //    Vector3 movestep = new Vector3(Random.Range(0, Direction.x * Speed) * Time.deltaTime, Random.Range(0, Direction.y * Speed) * Time.deltaTime, Random.Range(0, Direction.z * Speed) * Time.deltaTime);
    //    gameObject.transform.position += movestep;
    //}
    //void SpiralStep()
    //{
    //    Vector3 result = MoveOneStepTowards();
    //    double a = Radius * System.Math.Cos(Time.deltaTime * transform.position[(mainDirection + 1) % 3]);
    //    double b = Radius * System.Math.Sin(Time.deltaTime * transform.position[(mainDirection + 2) % 3]);
    //    result[(mainDirection + 1) % 3] += (float)a;
    //    result[(mainDirection + 2) % 3] += (float)b;
    //    gameObject.transform.position = result;
    //}
    //void ZigzagStep()
    //{
    //    //Vector3 result = MoveOneStepTowards();
    //    Vector3 result = new Vector3();
    //    double a = Speed * Direction[(mainDirection + 1) % 3] * Time.deltaTime;
    //    double b = Speed * Direction[(mainDirection + 2) % 3] * Time.deltaTime;
    //    if (a > transform.position[(mainDirection + 1) % 3] % Radius) a = -a;
    //    if (b > transform.position[(mainDirection + 1) % 3] % Radius) b = -b;
    //    result[(mainDirection + 1) % 3] += (float)a;
    //    result[(mainDirection + 2) % 3] += (float)b;
    //    gameObject.transform.position = result;
    //}
    //void LineStep()
    //{
    //    gameObject.transform.position = MoveOneStepTowards();
    //}
    //Vector3 MoveOneStepTowards()
    //{
    //    float step = Speed * Time.deltaTime;
    //    return Vector3.MoveTowards(transform.position, targetPosition, step);
    //}
}
