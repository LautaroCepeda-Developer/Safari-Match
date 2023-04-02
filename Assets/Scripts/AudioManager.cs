using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //singleton var
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    public AudioClip moveSFX;
    public AudioClip missSFX;
    public AudioClip matchSFX;
    public AudioClip gameOverSFX;

    public AudioSource sfxSource;

    private void PointsUpdated()
    {
        sfxSource.PlayOneShot(matchSFX);
    }

    private void GameStateUpdated(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.GameOver)
        {
            sfxSource.PlayOneShot(gameOverSFX);
        }

        if (newState == GameManager.GameState.Ingame)
        {
            sfxSource.PlayOneShot(matchSFX);
        }
    }

    private void Start()
    {
        GameManager.Instance.onPointsUpdated.AddListener(PointsUpdated);
        GameManager.Instance.onGameStateUpdated.AddListener(GameStateUpdated);


    }

    private void OnDestroy()
    {
        GameManager.Instance.onPointsUpdated.RemoveListener(PointsUpdated);
        GameManager.Instance.onGameStateUpdated.RemoveListener(GameStateUpdated);
    }


    public void Move()
    {
        sfxSource.PlayOneShot(moveSFX);
    }

    public void Miss()
    {
        sfxSource.PlayOneShot(missSFX);
    }
}
