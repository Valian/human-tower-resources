using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

    public PlayerBase Player;
    public NodeManager NodeManagerPrefab;
    public ScoreManager ScoreManagerPrefab;

    private NodeManager nodeManager;
    private ScoreManager scoreManager;

    public static GameManager Instance;
    public NodeManager NodeManagerInstance { get { return nodeManager; } }

    private int ballCount;

    void Awake()
    {
        ScoreBall.BallSpawned += OnBallSpawned;
        ScoreBall.BallCollected += OnBallCollected;        
        Instance = this;
    }

    void Start ()
    {
        try
        {
            InjectGearVrStuff();
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
        StartLevel();
    }

    private void StartLevel()
    {
        nodeManager = Instantiate<NodeManager>(NodeManagerPrefab);

        scoreManager = Instantiate<ScoreManager>(ScoreManagerPrefab);

        nodeManager.Generate();
        SpawnPlayer();
    }

    private void FinishLevel()
    {

    }

    private void SpawnPlayer()
    {
        // TODO - jakos sprytniej
        var node = nodeManager.GetComponentInChildren<Node>();
        Player.Movement.PlayerTargetReached += OnPlayerTargetReached;
        Player.Movement.PlayerTargetChanged += OnPlayerTargetChanged;
        Player.Movement.PlayerNextTargetChanged += OnPlayerNextTargetChanged;
        Player.Movement.SetPosition(node);
    }    

    private void OnBallSpawned(ScoreBall ball)
    {
        ballCount += 1;
    }

    private void OnBallCollected(ScoreBall ball)
    {
        ballCount -= 1;
        if(ballCount == 0)
        {
            FinishLevel();
        }
    }

    private void OnPlayerTargetReached(PlayerLinearMovement player)
    {
        ChangeNeightboursColors(player.CurrentNode, Color.green);
    }

    private void OnPlayerNextTargetChanged(PlayerLinearMovement player)
    {
        if (player.NextTargetNode)
        {
            ChangeNodeColor(player.NextTargetNode, Color.yellow);
        }
    }

    private void OnPlayerTargetChanged(PlayerLinearMovement player)
    {
        if(player.PreviousNode)
        {
            ChangeNeightboursColors(player.PreviousNode, Color.white);
        }
        if (player.TargetNode)
        {
            ChangeNeightboursColors(player.TargetNode, Color.green);
        }
    }

    private void ChangeNeightboursColors(Node node, Color color)
    {
        var neightbours = nodeManager.GetNeightbours(node);
        foreach (var n in neightbours)
        {
            ChangeNodeColor(n, color);
        }
    }    

    private void ChangeNodeColor(Node node, Color color)
    {
        node.GetComponent<Renderer>().material.color = color;
    }

    private void InjectGearVrStuff()
    {
        UnityEngine.VR.VRSettings.renderScale = 1.8f;

        var player = GameObject.FindGameObjectWithTag("Player");
        var oldCamera = player.transform.FindChild("Camera/Head/Main Camera");
        var gearCamera = (GameObject)Instantiate(Resources.Load("GearVrCamera"));
        gearCamera.transform.position = oldCamera.position;
        gearCamera.transform.rotation = oldCamera.rotation;
        gearCamera.transform.parent = oldCamera.transform.parent;
        gearCamera.SetActive(true);
        oldCamera.gameObject.SetActive(false);
    }
}
