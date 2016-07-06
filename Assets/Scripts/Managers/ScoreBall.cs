using UnityEngine;
using System.Collections;

public class ScoreBall : MonoBehaviour {

    public int Score;

    public delegate void BallCallback(ScoreBall ball);
    public static event BallCallback BallCollected;
    public static event BallCallback BallSpawned;

    public void Collect()
    {        
        Destroy(this);
        CallBallCollected(this);
    }

    public void Start()
    {
        CallBallSpawned(this);
    }
	
    private void CallBallCollected(ScoreBall ball)
    {
        if(BallCollected != null)
        {
            BallCollected(ball);
        }
    }

    private void CallBallSpawned(ScoreBall ball)
    {
        if (BallSpawned != null)
        {
            BallSpawned(ball);
        }
    }

}
