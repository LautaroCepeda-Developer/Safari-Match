using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    #region Variables (REGION)

    [SerializeField] int width, height; //Size of the board (Each unit, represents one object)
    [SerializeField] GameObject tileObject; //Object to fill the board
    [SerializeField] float cameraSizeOffset, cameraVerticalOffset; // [0]OrthographicSize, [1]cameraVerticalPosition

    #endregion

    #region Functions (REGION)

    private void SetupBoard() //Set the grid
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var o = Instantiate(tileObject, new Vector3(x, y, -5), Quaternion.identity);
                o.transform.parent = transform;
            }
        }
    }

    private void PositionCamera()
    {
        float newPosX = (float)width / 2f;
        float newPosY = (float)height / 2f;

        //Moving the camera to correctly position the elements
        Camera.main.transform.position = new Vector3(newPosX - 0.5f, newPosY - 0.5f + cameraVerticalOffset, -10);

        float horizontal = width + 1;
        float vertical = (height / 2) + 1;

        //Adapting the camera based on the grid
        Camera.main.orthographicSize = horizontal > vertical ? horizontal + cameraSizeOffset: vertical + cameraVerticalOffset; 
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SetupBoard();
        PositionCamera();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
