using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStartScreen : MonoBehaviour
{
    public void StartBtnPressed()
    {
        GameManager.Instance.StartGame();
    }


}
