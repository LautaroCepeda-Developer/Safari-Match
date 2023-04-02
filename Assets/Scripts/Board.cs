using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private bool swappingPieces = false; //Currently Unused


    #endregion

    #region Functions (REGION)

    private void SetupBoard()
    {//Set the grid
        tiles = new Tile[width, height]; //Avoiding errors
        
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

    private Piece CreatePieceAt(int x, int y)
    {
        var selectedPiece = availablePieces[UnityEngine.Random.Range(0, availablePieces.Length)]; //Selecting a random piece
        var o = Instantiate(selectedPiece, new Vector3(x, y, -5), Quaternion.identity); //Creating the element
        o.transform.parent = transform; //Establishing the board as parent of the element
        pieces[x, y] = o.GetComponent<Piece>();
        pieces[x, y]?.Setup(x, y, this); //Setting the coordinates of the piece
        return pieces[x, y];
    }

    private void SetupPieces()
    {
        pieces = new Piece[width, height]; //Avoiding Errors
        int maxIterations = 50;
        int currentIteration = 0;


        //Loops to position elements
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                currentIteration = 0;
                var newPiece = CreatePieceAt(x, y);
                while (HasPreviousMatches(x, y))
                {
                    ClearPieceAt(x, y);
                    newPiece = CreatePieceAt(x, y);
                    currentIteration++;
                    if (currentIteration > maxIterations)
                    {
                        break;
                    }
                }
            }
        }
    }

    private void ClearPieceAt(int x, int y)
    {
        var pieceToClear = pieces[x, y];
        Destroy(pieceToClear.gameObject);
        pieces[x, y] = null;
    }

    #region Movement of pieces (REGION)
    private IEnumerator SwapTiles()
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

        //Waiting 0.6 seconds
        yield return new WaitForSeconds(0.6f);

        bool foundMatch = false;
        var startMatches = GetMatchByPiece(startTile.x, startTile.y, 3);
        var endMatches = GetMatchByPiece(endTile.x, endTile.y, 3);

        //if match >= 3 destroy pieces
        startMatches.ForEach(piece =>
        {
            foundMatch = true;
            ClearPieceAt(piece.x, piece.y);
        });

        //if match >= 3 destroy pieces
        endMatches.ForEach(piece =>
        {
            foundMatch = true;
            ClearPieceAt(piece.x, piece.y);
        });

        //Reseting the position in case match wasn't found
        if (!foundMatch)
        { 
            startPiece.Move(startTile.x, startTile.y);
            endPiece.Move(endTile.x, endTile.y);
            pieces[startTile.x, startTile.y] = startPiece;
            pieces[endTile.x, endTile.y] = endPiece;
        }

        //Reseting the vars
        startTile = null;
        endTile = null;
        swappingPieces = false; //Currently Unused

        yield return null;
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
        if (startTile != null && endTile != null && IsCloseTo(startTile, endTile))
        {
            StartCoroutine(SwapTiles());
        }
    }

    public bool IsCloseTo(Tile start, Tile end)
    { //Limiting the movement of the pieces
        if (Math.Abs(start.x - end.x) == 1 && start.y == end.y)
        {
            return true;
        }

        if(Math.Abs(start.y - end.y) == 1 && start.x == end.x)
        {
            return true;
        }

        return false;
    }
    #endregion

    public List<Piece>GetMatchByDirection(int xpos, int ypos, Vector2 direction, int minPieces = 3)
    {
        List<Piece> matches = new List<Piece>(); //Creating a list
        Piece startPiece = pieces[xpos, ypos]; //Setting the initial piece
        matches.Add(startPiece); //Adding the initial piece to matches

        //Vars for positions
        int nextX, nextY;
        int maxVal = width > height ? width : height;
        
        for (int i = 1; i < maxVal; i++)
        {
            //Searching a piece in the next position of 2D axes
            nextX = xpos + ((int)direction.x * i);
            nextY = ypos + ((int)direction.y * i);

            //Limiting the search of pieces within the grid
            if (nextX >= 0 && nextX < width && nextY >= 0 && nextY < height)
            {
                var nextPiece = pieces[nextX, nextY];
                if (nextPiece != null && nextPiece.pieceType == startPiece.pieceType)
                { //If the pieces match, then add to the array
                    matches.Add(nextPiece);
                } else { break; } //If the pieces doesn't match, break the loop
            }
        }//Loop end

        if (matches.Count >= minPieces)
        {
            return matches;
        }

        return null; //Returns null if nothing matched.
    }

    public bool HasPreviousMatches(int posX, int posY)
    {
        /*Searching a match in left direction, and down direction, because the creation of pieces
        starts in the left-down corner*/
        var downMatches = GetMatchByDirection(posX, posY, new Vector2(0, -1), 2);
        var leftMatches = GetMatchByDirection(posX, posY, new Vector2(-1, 0), 2);

        downMatches ??= new List<Piece>();
        leftMatches ??= new List<Piece>();

        return downMatches.Count > 0 || leftMatches.Count > 0;
    }


    public List<Piece>GetMatchByPiece(int xpos, int ypos, int minPieces = 3)
    {
        //Vars for search a match in each direction
        var upMatchs = GetMatchByDirection(xpos, ypos, new Vector2(0, 1), 2);
        var downMatchs = GetMatchByDirection(xpos, ypos, new Vector2(0, -1), 2);
        var rightMatchs = GetMatchByDirection(xpos, ypos, new Vector2(1, 0), 2);
        var leftMatchs = GetMatchByDirection(xpos, ypos, new Vector2(-1, 0), 2);

        //Initializing variables with empty lists
        if (upMatchs == null) upMatchs = new List<Piece>();
        if (downMatchs == null) downMatchs = new List<Piece>();
        if (rightMatchs == null) rightMatchs = new List<Piece>();
        if (leftMatchs == null) leftMatchs = new List<Piece>();

        //Creating the list of matches in the 2D axes
        var verticalMatches = upMatchs.Union(downMatchs).ToList();
        var horizontalMatches = leftMatchs.Union(rightMatchs).ToList();

        //Creating the list of foundedMatches
        var foundMatches = new List<Piece>();

        //Adding the matched pieces to the list "foundMatches"
        if (verticalMatches.Count >= minPieces)
        {
            foundMatches = foundMatches.Union(verticalMatches).ToList();
        }
        if (horizontalMatches.Count >= minPieces)
        {
            foundMatches = foundMatches.Union(horizontalMatches).ToList();
        }

        //Returning the matches regardless of whether they meet or not
        return foundMatches;
    }

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
