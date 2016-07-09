using UnityEngine;
using System.Collections;

public class GuiManager : MonoBehaviour {

    public static GuiManager Instance;
    public GameObject MenuPanel;
    public GameObject GameGuiPanel;
    public bool IsMenuOn = true;
    private Vector3 playerMenuPosition;
    private Quaternion playerMenuRotation;

    void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        GameManager.Instance.GameEnded += Instance_GameOver;
        playerMenuPosition = GameManager.Instance.Player.transform.position;
        playerMenuRotation = GameManager.Instance.Player.transform.localRotation;
        GameGuiPanel.SetActive(false);
        ShowMenu();
    }

    private void Instance_GameOver(bool canceledByUser)
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        GameManager.Instance.Player.transform.position = playerMenuPosition;
        GameManager.Instance.Player.transform.localRotation = playerMenuRotation;
        MenuPanel.SetActive(true);
        //GameGuiPanel.SetActive(false);
        IsMenuOn = true;
    }

    public void HideMenu()
    {
        MenuPanel.SetActive(false);
        //GameGuiPanel.SetActive(true);
        IsMenuOn = false;
    }
    
}
