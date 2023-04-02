using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    //Singleton var
    public static GameManager Instance;
    private void Awake()
    {
        #region Singleton (REGION)
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion


    }


    public int points = 0;
    public UnityEvent onPointsUpdated;
    public UnityEvent<GameState>onGameStateUpdated;

    public float timeToMatch = 10f;
    public float currentTimeToMatch = 0;

    public enum GameState
    { 
        Idle,
        Ingame,
        GameOver
    }
    public GameState gameState;

    public void AddPoints(int newPoints)
    { 
        points += newPoints; //Updating the points
        onPointsUpdated?.Invoke();
        currentTimeToMatch = 0;
    }

    public void StartGame()
    {
        points = 0;
        gameState = GameState.Ingame;
        onGameStateUpdated?.Invoke(gameState);
        currentTimeToMatch = 0;
    }

    public void RestartGame()
    {
        points = 0;
        gameState = GameState.Ingame;
        onGameStateUpdated?.Invoke(gameState);
        currentTimeToMatch = 0f;
    }
    
    public void ExitGame()
    {
        points = 0;
        gameState = GameState.Idle;
        onGameStateUpdated?.Invoke(gameState);
    }

    private void Update()
    {
        if (gameState == GameState.Ingame)
        {
            currentTimeToMatch += Time.deltaTime;
            if (currentTimeToMatch >= timeToMatch)
            {
                gameState = GameState.GameOver;
                onGameStateUpdated?.Invoke(gameState);
            }
        }
    }
}
