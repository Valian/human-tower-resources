using UnityEngine;
using UnityEngine.UI;

public class GameGUI : MonoBehaviour
{
    public Text LifesText;
    public Text DotsRemainig;
    public Text Level;

    void Update()
    {
        LifesText.text = string.Format("{0}/{1}", GameManager.Instance.Player.Stats.Lifes, PlayerStats.MaxLifes);
        DotsRemainig.text = GameManager.Instance.DotsCount.ToString();
        Level.text = GameManager.Instance.CurrentLevel.ToString();
    }

    public void ExitToMenuClick()
    {
        GameManager.Instance.EndGame(false);
        GuiManager.Instance.ShowMenu();
    }
}
