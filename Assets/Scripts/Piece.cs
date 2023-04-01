using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Piece : MonoBehaviour
{
    #region Variables(REGION)

    public int x, y;
    public Board board;
    public PieceType pieceType;

    #endregion

    public enum PieceType
    { //List of pieces
        elephant,
        giraffe,
        hippo,
        monkey,
        panda,
        parrot,
        penguin,
        pig,
        rabbit,
        snake
    };

    #region Functions(REGION)

    public void Setup(int x_, int y_, Board board_)
    {
        //Coordinate System for board
        x = x_;
        y = y_;
        board = board_;
    }

    public void Move(int desX, int desY)
    {
        //Move the piece
        transform.DOMove(new Vector3(desX, desY, -5), 0.25f).SetEase(Ease.InOutCubic).onComplete = () =>
        {
            x = desX;
            y = desY;
        };
    }

    //Function to test the movement of the piece
    [ContextMenu("Test Move")]
    public void MoveTest()
    {
        Move(0, 0);
    }

    #endregion
}
