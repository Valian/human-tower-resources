using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{    
    public int Score { get; private set; }
    
    void Start()
    {
        ScoreBall.BallCollected += OnBallCollected;
    }

    void OnBallCollected(ScoreBall ball)
    {
        Score += ball.Score;
    }
}
