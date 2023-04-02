using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    public int displayedPoints = 0;
    public TextMeshProUGUI pointsUI;

    private void Start()
    {
        GameManager.Instance.onGameStateUpdated.AddListener(GameStateUpdated);
    }

    private void OnDestroy()
    {
        GameManager.Instance.onGameStateUpdated.RemoveListener(GameStateUpdated);
    }

    private void GameStateUpdated(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.GameOver)
        {
            displayedPoints = 0;
            StartCoroutine(DisplayPointsCoroutine());
        }
    }

    IEnumerator DisplayPointsCoroutine()
    {
        while (displayedPoints < GameManager.Instance.points)
        {
            displayedPoints++;
            pointsUI.text = displayedPoints.ToString();
            yield return new WaitForFixedUpdate();
        }

        displayedPoints = GameManager.Instance.points;
        var fullText = displayedPoints.ToString() + " Points";
        pointsUI.text = fullText;

        yield return null;
    }

    public void PlayAgainBtnPressed()
    {
        GameManager.Instance.RestartGame();
    }

    public void ExitGameBtnPressed()
    {
        GameManager.Instance.ExitGame();
    }


}
