using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int rows;
    public int columns;

<<<<<<< HEAD
    // the board
    public int height = 9;
    public int width = 9;
    public int scale = 2;
    public GameObject[][] gridArray; // initialized within
    public GameObject squareObject; // from inspector
=======
    public int scale = 2;
>>>>>>> max_level_5(v2)

    public GameObject[][] gridArray;
    public GameObject cellObject;

    public GameRules gameRules;

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD

        // initialize the aura system
        Debug.Log("Initializing the aura system");
        aura = GetComponent<Aura>();

        // initialize the player array
        Debug.Log("Initializing player array");
        players = new Player[2];
        players[0] = playerObjects[0].GetComponent<Player>();
        players[1] = playerObjects[1].GetComponent<Player>();

        // initialize the grid array
        Debug.Log("Initializing the grid");
        float interZ = gameObject.transform.position.z;
        float interY = gameObject.transform.position.y;
        float interX = gameObject.transform.position.x;
        float offSet = -1;
        gridArray = new GameObject[height][];
        for (int i = 0; i < height; i++)
        {
            gridArray[i] = new GameObject[width];
            // put the squares in the grid
            for (int j = 0; j < width; j++)
            {
                Vector3 squarePos = new Vector3(interX + i * scale - (height - 1) * scale / 2, interY + j * scale - (height - 1) * scale / 2, interZ + offSet);
                gridArray[i][j] = Instantiate(squareObject, squarePos, Quaternion.identity, gameObject.transform);
            }
        }

        // initialize the turn structure
        Debug.Log("Initializing the turn structure");
        phaseFunctions = new List<Action>();
        phaseFunctions.Add(DrawPhase); // check that this works
        phaseFunctions.Add(CombinePhase);
        phaseFunctions.Add(PlacePhase);
        phaseFunctions.Add(MovePhase);
        phaseFunctions.Add(AuraPhase);

        // initialize the centre pieces
        Debug.Log("Initializing the centrepieces");
        Square square0 = gridArray[(int)startPosition0.x][(int)startPosition0.y].GetComponent<Square>();
        Square square1 = gridArray[(int)startPosition1.x][(int)startPosition1.y].GetComponent<Square>();
        square0.AddPiece(players[0].centrePieceObject);
        square1.AddPiece(players[1].centrePieceObject);

        // initialize the starting cards
        Debug.Log("Drawing the players first hand");
        cardsNum = startingCardsNum; // check that this works
        playerTurn = 1; phaseFunctions[0]();
        playerTurn = 0; phaseFunctions[0]();
        cardsNum = drawCardsNum;
        phase = 0;
        UpdateGameState();
    }

    void Update()
    {
        //Debug.Log(phase);
        // get the appropriate data
        Player player = players[playerTurn];
        player.isTurn = true;

        // check if skip button has been pressed (make sure the skip button can only be pressed during combine to attack phase)
        if (player.skipButtonPressed)
        {
            Debug.Log("Skip button was pressed, therefore skipping phase");
            player.skipButtonPressed = false;
            phase = player.phase;
        }
        player.phase = phase;

        // call the appropriate function
        phaseFunctions[phase]();
        if (phase > 4)
        {
            Debug.Log("Ending player turn");
            phase = 0;
            playerTurn = (playerTurn + 1) % 2;
            player.isTurn = false;
        }
=======
        CreateBoard();
        CenterObjects();
        gameRules.ClearInspector();
>>>>>>> max_level_5(v2)
    }

    void CreateBoard()
    {
        gridArray = new GameObject[rows][];
        for (int i = 0; i < gridArray.Length; i++)
        {
            gridArray[i] = new GameObject[columns];
            for (int j = 0; j < gridArray[i].Length; j++)
            {
                AttachCell(i, j);
            }
        }
    }

    void AttachCell(int i, int j)
    {
        gridArray[i][j] = Instantiate(cellObject, Vector3.zero, Quaternion.identity, gameObject.transform);

        Vector3 cellPosition = new Vector3(i - (rows - 1) / 2, j - (columns - 1) / 2, 0) * scale;
        gridArray[i][j].transform.localPosition = cellPosition;
        gridArray[i][j].GetComponent<Cell>().SetLocation(i, j);
        gridArray[i][j].SetActive(true);
    }

    public bool ValidLocation(int i, int j)
    {
        if (i < rows && j < columns && i >= 0 && j >= 0)
        {
            return true;
        }
        return false;
    }

    void CenterObjects()
    {
        Cell startPos0 = gridArray[0][(int)Mathf.Floor(columns / 2)].GetComponent<Cell>();
        Cell startPos1 = gridArray[rows - 1][(int)Mathf.Floor(columns / 2)].GetComponent<Cell>();

        Piece centerPiece0 = gameRules.player0.centerPiece;
        centerPiece0.level = 1;
        centerPiece0.modifier.GetModifierValues();
        centerPiece0.StartPiece();

        startPos0.piece = gameRules.player0.centerPiece;
        print(startPos0.location);
        startPos0.UpdateCell();
        //startPos0.TintAdjacentCells(centerPiece0.modifier.color);

        Piece centerPiece1 = gameRules.player1.centerPiece;
        centerPiece1.level = 1;
        centerPiece1.modifier.GetModifierValues();
        centerPiece1.StartPiece();

        startPos1.piece = gameRules.player1.centerPiece;
        print(startPos1.location);
        startPos1.UpdateCell();
    }

}
