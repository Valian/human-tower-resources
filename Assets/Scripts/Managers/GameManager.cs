﻿using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

    public PlayerBase Player;
    public int CurrentLevel { get { return 1; } }
    public NodeManager NodeManagerPrefab;
    public ScoreManager ScoreManagerPrefab;
    public bool GameRunning { get; private set; }
    public int DotsCount { get; private set; }
    public event Action<bool> GameOver = delegate { };
     
    private NodeManager nodeManager;
    private ScoreManager scoreManager;

    public static GameManager Instance;
    public NodeManager NodeManagerInstance { get { return nodeManager; } }

    void Awake()
    {
        ScoreBall.BallSpawned += OnBallSpawned;
        ScoreBall.BallCollected += OnBallCollected;        
        Instance = this;
    }

    void Start ()
    {
        //StartLevel();
    }

    public void StartLevel()
    {        
        nodeManager = Instantiate<NodeManager>(NodeManagerPrefab);

        if (scoreManager == null)
        {
            scoreManager = Instantiate<ScoreManager>(ScoreManagerPrefab);
        }

        nodeManager.Generate();
        SpawnPlayer();
        GameRunning = true;
    }

    public void EndGame(bool canceledByUser = false)
    {
        Destroy(nodeManager);
        GameRunning = false;
        GameOver(canceledByUser);
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
        Player.Stats.ResetStats();
        Player.Movement.SetPosition(node);
    }    

    private void OnBallSpawned(ScoreBall ball)
    {
        DotsCount += 1;
    }

    private void OnBallCollected(ScoreBall ball)
    {
        DotsCount -= 1;
        if(DotsCount == 0)
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
}
