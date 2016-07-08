using UnityEngine;

public class AudioManager : MonoBehaviour
{           
    public AudioSource MusicSource;
    public AudioSource NextLevelSource;
    public AudioSource DieSource;
    public AudioSource GameOverSource;
    public AudioSource EatingSource;

    public static AudioManager Instance;                                


    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        GameManager.Instance.LevelFinished += Instance_LevelFinished;
        GameManager.Instance.LifeLost += Instance_LifeLost;
        GameManager.Instance.GameEnded += Instance_GameEnded;
    }

    private void Instance_GameEnded(bool obj)
    {
        PlayGameOverSound();
    }

    private void Instance_LifeLost()
    {
        PlayDieSound();
    }


    private void Instance_LevelFinished()
    {
        PlayNextLevelSound();
    }



    public void TurnOnMusic()
    {
        MusicSource.Play();
    }

    public void TurnOffMusic()
    {
        MusicSource.Stop();
    }
    
    public void PlayDieSound()
    {
        DieSource.Play();
    }

    public void PlayNextLevelSound()
    {
        NextLevelSource.Play();
    }

    public void PlayGameOverSound()
    {
        GameOverSource.Play();
    }

}