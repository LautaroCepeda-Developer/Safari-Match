using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;

public class UIPoints : MonoBehaviour
{
    private int displayedPoints = 0;
    
    public TextMeshProUGUI pointsLabel;

    private void Start()
    {
        GameManager.Instance.onPointsUpdated.AddListener(UpdatePoints);
    }

    private void UpdatePoints()
    {
        StartCoroutine(UpdatePointsCoroutine());
    }

    IEnumerator UpdatePointsCoroutine()
    {
        while (displayedPoints < GameManager.Instance.points)
        {
            displayedPoints++;
            pointsLabel.text = displayedPoints.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
