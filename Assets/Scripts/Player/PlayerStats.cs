using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public const int MaxLifes = 20;
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
            GameManager.Instance.EndGame(false);
        } else
        {
            GameManager.Instance.LooseLife();
        }
    }
}
