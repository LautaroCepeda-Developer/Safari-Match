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

    public GameObject[] availablePieces;

    public Tile[,] tiles;
    public Piece[,] pieces;

    Tile startTile, endTile;

    #endregion

    #region Functions (REGION)

    private void SetupBoard()
    {//Set the grid
        //Loops to position elements
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var o = Instantiate(tileObject, new Vector3(x, y, -5), Quaternion.identity); //Creating the element
                o.transform.parent = transform; //Establishing the board as parent of the element
                tiles[x, y] = o.GetComponent<Tile>();
                tiles[x, y]?.Setup(x, y, this); //Setting the coordinates of the element
            }
        }
    }

    private void PositionCamera()
    {
        //Setting vars to position properly the camera
        float newPosX = (float)width / 2f; //width of the grid / 2
        float newPosY = (float)height / 2f; //height of the grid / 2

        //Moving the camera to correctly position the elements
        Camera.main.transform.position = new Vector3(newPosX - 0.5f, newPosY - 0.5f + cameraVerticalOffset, -10);

        //Setting vars to adapt properly the camera
        float horizontal = width + 1;
        float vertical = (height / 2) + 1;

        //Adapting the camera based on the grid
        Camera.main.orthographicSize = horizontal > vertical ? horizontal + cameraSizeOffset: vertical + cameraVerticalOffset; 
    }

    private void SetupPieces()
    {
        //Loops to position elements
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var selectedPiece = availablePieces[UnityEngine.Random.Range(0, availablePieces.Length)]; //Selecting a random piece
                var o = Instantiate(selectedPiece, new Vector3(x, y, -5), Quaternion.identity); //Creating the element
                o.transform.parent = transform; //Establishing the board as parent of the element
                pieces[x, y] = o.GetComponent<Piece>();
                pieces[x, y]?.Setup(x, y, this); //Setting the coordinates of the piece
            }
        }
    }

    #region Movement of pieces (REGION)
    private void SwapTiles()
    {
        //Selecting the pieces
        var startPiece = pieces[startTile.x, startTile.y];
        var endPiece = pieces[endTile.x, endTile.y];

        //Swap the position of the pieces
        startPiece.Move(endTile.x, endTile.y);
        endPiece.Move(startTile.x, startTile.y);

        //Updating the coordinates of the pieces
        pieces[startTile.x, startTile.y] = endPiece;
        pieces[endTile.x, endTile.y] = startPiece;
    }

    public void TileDown(Tile tile_)
    {
        startTile = tile_;
    }

    public void TileOver(Tile tile_)
    {
        endTile = tile_;
    }

    public void TileUp(Tile tile_)
    {
        if (startTile != null && endTile != null)
        {
            SwapTiles();
        }
        startTile = null;
        endTile = null;
    }
    #endregion

    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        //Instantiating the 2D_Array (CoordinateSystem)
        tiles = new Tile[width, height];
        pieces = new Piece[width, height];

        //Setup the scene
        SetupBoard();
        PositionCamera();
        SetupPieces();
    }

}
