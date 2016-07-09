using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{           
    public AudioSource MusicSource;
    public AudioSource FastMusicSource;

    public AudioSource NextLevelSource;
    public AudioSource DieSource;
    public AudioSource GameOverSource;
    public AudioSource PowerDotSource;
    public GameObject  EatingPrefab;

    public static AudioManager Instance;

    private List<AudioSource> eatingSounds;
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        var toBeDestroyed = new List<AudioSource>();
        for(var i = 0; i < eatingSounds.Count; i++)
        {
            if (!eatingSounds[i].isPlaying)
            {
                toBeDestroyed.Add(eatingSounds[i]);
            }
        }
        foreach (var item in toBeDestroyed)
        {
            eatingSounds.Remove(item);
            Destroy(item.gameObject);
        }
    }

    void Start()
    {
        GameManager.Instance.LevelFinished += Instance_LevelFinished;
        GameManager.Instance.LifeLost += Instance_LifeLost;
        GameManager.Instance.GameEnded += Instance_GameEnded;
        ScoreBall.BallCollected += ScoreBall_BallCollected;
        eatingSounds = new List<AudioSource>();
    }

    private void ScoreBall_BallCollected(ScoreBall ball)
    {
        if(typeof(PowerDot) == ball.GetType())
        {
            PowerDotSource.Play();
            MusicSource.Stop();
            FastMusicSource.Play();
            Invoke("ChangeIsFrightened", Enemy.FrightenedTimer);
        } else
        {
            var eating = Instantiate(EatingPrefab).GetComponent<AudioSource>();
            eating.Play();
            eatingSounds.Add(eating);
        }
    }

    private void ChangeIsFrightened()
    {
        FastMusicSource.Stop();
        MusicSource.Play();
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