using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    public void StartGameClick()
    {
        GameManager.Instance.StartLevel();
        GuiManager.Instance.HideMenu();
    }
}
