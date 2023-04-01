using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region Variables (REGION)
    public int x, y;
    public Board board;
    #endregion

    #region Functions (REGION)
    //Coordinate System for board
    public void Setup(int x_, int y_, Board board_)
    {
        //Setting the coordinates of each Tile
        x = x_;
        y = y_;
        board = board_;
    }

    #region Input Detection (REGION)
    /*Detect the input and the piece selected*/
    public void OnMouseDown()
    {
        board.TileDown(this);
    }

    public void OnMouseEnter()
    {
        board.TileOver(this);
    }

    public void OnMouseUp()
    {
        board.TileUp(this);
    }
    #endregion

    #endregion
}
