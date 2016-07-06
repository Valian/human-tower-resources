using UnityEngine;
using System.Collections;

public class ScoreBall : MonoBehaviour {

    public int Score;

    public delegate void BallCallback(ScoreBall ball);
    public static event BallCallback BallCollected;

    public void Collect()
    {        
        Destroy(this);
        CallBallCollected(this);
    }
	
    private void CallBallCollected(ScoreBall ball)
    {
        if(BallCollected != null)
        {
            BallCollected(ball);
        }
    }

}
