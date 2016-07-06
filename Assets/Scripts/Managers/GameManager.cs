using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public PlayerBase Player;
    public NodeManager NodeManagerPrefab;
    public ScoreManager ScoreManagerPrefab;

    private NodeManager nodeManager;
    private ScoreManager scoreManager;

    void Start () {
        nodeManager = Instantiate<NodeManager>(NodeManagerPrefab);
        scoreManager = Instantiate<ScoreManager>(ScoreManagerPrefab);

        nodeManager.Generate();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        // TODO - jakos sprytniej
        var node = nodeManager.GetComponentInChildren<Node>();
        Player.Movement.SetPosition(node);
    }    
}
