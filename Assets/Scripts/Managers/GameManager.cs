using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public PlayerBase PlayerPrefab;
    public NodeManager NodeManagerPrefab;
    public ScoreManager ScoreManagerPrefab;

    private NodeManager nodeManager;
    private ScoreManager scoreManager;
    private PlayerBase player;

    void Start () {
        nodeManager = Instantiate<NodeManager>(NodeManagerPrefab);
        scoreManager = Instantiate<ScoreManager>(ScoreManagerPrefab);
        player = Instantiate<PlayerBase>(PlayerPrefab);

        nodeManager.Generate();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        // TODO - jakos sprytniej
        var node = nodeManager.GetComponentInChildren<Node>();
        player.Movement.SetPosition(node);
    }


    // Update is called once per frame
    void Update () {
	
	}
}
