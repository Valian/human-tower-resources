using UnityEngine;
using System;
using System.Linq;

using Object = UnityEngine.Object;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class GameManager : MonoBehaviour
{
    public float DeathFreezeTime = 3f;
    public PlayerBase       Player;
    public EnemySpawner     EnemiesSpawner;

    public GraphManager     GraphManagerPrefab;

    public bool     GameRunning { get; private set; }
    public int      DotsCount { get; private set; }
    public int      CurrentLevel { get; private set; }

    public GraphManager GraphManagerInstance { get { return graphManager; } }

    public event Action<bool>   GameEnded = delegate { };
    public event Action         GameStarted = delegate { };
    public event Action         LevelStarted = delegate { };
    public event Action         LevelFinished = delegate { };
    public event Action         LifeLost = delegate { };
    public event Action         PowerDotCollected = delegate { };

    private GraphManager graphManager;

    public static GameManager Instance;

    void Awake()
    {
        ScoreBall.BallSpawned += OnBallSpawned;
        ScoreBall.BallCollected += OnBallCollected;
        ScoreBall.BallCollected += OnPowerDotCollected;
        
        graphManager = Instantiate<GraphManager>(GraphManagerPrefab);
        Instance = this;
    }

    void Start()
    {
        Player.Movement.PlayerTargetReached += OnPlayerTargetReached;
        Player.Movement.PlayerTargetChanged += OnPlayerTargetChanged;
        Player.Movement.PlayerNextTargetChanged += OnPlayerNextTargetChanged;
        LevelManager.InitiateLevels(graphManager.transform.position);
        StartCoroutine(ApplyBackgroundHealthIndicator());
    }

    void Update()
    {
        // DEBUG
        if (Input.GetKeyDown(KeyCode.K))
        {
            PowerDotCollected();
        }
        // DEBUG
    }

    public void StartGame()
    {
        Player.Stats.ResetStats();
        CurrentLevel = 1;
        GameStarted();
        StartLevel(CurrentLevel);
    }

    public void StartLevel(int levelNo)
    {
        var levelDef = LevelManager.GetLevelDefinition(levelNo);
        graphManager.Generate(levelDef.GraphType, levelDef.GraphSettings);
        EnemiesSpawner.SpawnEnemies(levelDef.EnemiesCount);
        SpawnPlayer();
        GameRunning = true;
        LevelStarted();
    }

    public void EndGame(bool success)
    {   

        graphManager.DestroyGraph();
        EnemiesSpawner.ClearEnemies();
        DotsCount = 0;
        GameRunning = false;
        GameEnded(success);
    }

    public void LooseLife()
    {               
        LifeLost();
        StartCoroutine(LoseLifeCoroutine());
    }

    private IEnumerator LoseLifeCoroutine()
    {
        SetDeathEffects(true);
        yield return WaitRealTime(DeathFreezeTime);
        ClearNodesModifications();
        SpawnPlayer();
        SetDeathEffects(false);
    }

    private IEnumerator WaitRealTime(float time)
    {
        var t = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < t + time)
        {
            yield return null;
        }
    }

    private void SetDeathEffects(bool dead)
    {
        var effects = FindObjectsOfType<Grayscale>();
        foreach (var e in effects)
        {
            e.enabled = dead;
        }
        Time.timeScale = dead ? 0f : 1f;
    }

    private void ClearNodesModifications()
    {
        foreach (Node n in graphManager.Nodes)
        {
            n.Show();
            ChangeNodeColor(n, Color.white);
        }
    }

    private IEnumerator ApplyBackgroundHealthIndicator()
    {
        var cameras = Camera.main.GetComponentsInChildren<Camera>();
        var t = Time.time;
        while(true)
        {
            if(GameRunning)
            {
                var color = (1 - Player.Stats.Lifes * 1f / PlayerStats.MaxLifes) / 3 * Mathf.Sin((Time.time - t) * 2);
                foreach (var c in cameras)
                {
                    c.backgroundColor = new Color(color, 0, 0);
                }
            }            
            yield return null;
        }        
    }

    private void FinishLevel()
    {
        LevelFinished();
        graphManager.DestroyGraph();
        CurrentLevel++;
        if (CurrentLevel > LevelManager.MaxLevelNo)
        {
            EndGame(true);
        }
        else
        {
            StartLevel(CurrentLevel);
        }
    }

    private void SpawnPlayer()
    {
        var node = graphManager.GetRandomNode();
        Player.Movement.Spawn(node);
    }

    private void OnBallSpawned(ScoreBall ball)
    {
        DotsCount += 1;
    }

    private void OnBallCollected(ScoreBall ball)
    {
        DotsCount -= 1;
        if (DotsCount == 0)
        {
            FinishLevel();
        }
    }

    private void OnPowerDotCollected(ScoreBall ball)
    {
        if (!ball.isPowerDot)
            return;
        PowerDotCollected();
    }

    private void OnPlayerTargetReached(PlayerLinearMovement player)
    {
        ChangeNeightboursColors(player.CurrentNode, Color.green);
        player.CurrentNode.Hide(true);
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
        foreach(var n in graphManager.Nodes)
        {
            if (n != player.CurrentNode)
            {
                n.Show();
            }
        }   

        if (player.PreviousNode)
        {
            ChangeNeightboursColors(player.PreviousNode, Color.white);
        }
        if (player.TargetNode)
        {
            ChangeNeightboursColors(player.TargetNode, Color.green);
            player.TargetNode.Hide();
        }
    }

    private void ChangeNeightboursColors(Node node, Color color)
    {
        var neightbours = graphManager.GetNeightbours(node);
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
