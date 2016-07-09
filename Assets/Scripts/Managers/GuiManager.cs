using UnityEngine;
using System.Collections;

public class GuiManager : MonoBehaviour {

    public static GuiManager Instance;
    public GameObject MenuPanel;
    public GameObject CreditsPanel;
    public bool IsMenuOn = true;
    private Vector3 playerMenuPosition;
    void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        GameManager.Instance.GameEnded += Instance_GameOver;
        playerMenuPosition = GameManager.Instance.Player.transform.position;
        ShowMenu();
    }

    private void Instance_GameOver(bool canceledByUser)
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        GameManager.Instance.Player.transform.position = playerMenuPosition;
        MenuPanel.SetActive(true);
        CreditsPanel.SetActive(true);
        IsMenuOn = true;
    }

    public void HideMenu()
    {
        MenuPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        //GameGuiPanel.SetActive(true);
        IsMenuOn = false;
    }
    
}
