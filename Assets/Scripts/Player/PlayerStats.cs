using UnityEngine;
using System.Collections;
using System;

public class PlayerStats : MonoBehaviour
{
    public const int MaxLifes = 3;
    public int Lifes { get; private set; }

    public void ResetStats()
    {
        Lifes = MaxLifes;
    }

    public void GetHit()
    {
        Lifes--;
        if(Lifes < 0)
        {
            GameManager.Instance.EndGame();
        }
    }
}
