using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public PlayerBase Player;
    public NodeManager NodeManagerPrefab;
    public ScoreManager ScoreManagerPrefab;

    private NodeManager nodeManager;
    private ScoreManager scoreManager;

    public static GameManager Instance;

    private int ballCount;

    void Awake()
    {
        ScoreBall.BallSpawned += OnBallSpawned;
        ScoreBall.BallCollected += OnBallCollected;
        Instance = this;
    }

    void Start ()
    {
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

}
