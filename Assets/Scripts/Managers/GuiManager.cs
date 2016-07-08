using UnityEngine;
using System.Collections;

public class GuiManager : MonoBehaviour {

    public static GuiManager Instance;
    public GameObject MenuPanel;
    public GameObject GameGuiPanel;

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
        GameGuiPanel.SetActive(false);
    }

    public void HideMenu()
    {
        MenuPanel.SetActive(false);
        GameGuiPanel.SetActive(true);
    }
    
}
