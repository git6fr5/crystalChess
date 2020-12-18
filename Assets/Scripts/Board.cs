using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

public class Board : MonoBehaviour
{

    /*-------------------------------------------------------------------------------------------------------*/
    // DECLARING VARIABLES
    /*-------------------------------------------------------------------------------------------------------*/

    // the board
    public int height = 9;
    public int width = 9;
    public int scale = 2;
    public GameObject[][] gridArray; // initialized within
    public GameObject squareObject; // from inspector

    // the players
    public GameObject[] playerObjects; // from inspector
    public Player[] players; // initialized within
    public int playerTurn = 0;

    // the phases
    public int phase = 0;
    public int moveTick = 0;

    // starting game variables
    public int startingCardsNum = 5;
    public int drawCardsNum = 2;
    private int cardsNum; // initialized within
    private Vector2 startPosition0 = new Vector2(0, 0);
    private Vector2 startPosition1 = new Vector2(8, 8);

    // scripts
    public Aura aura;

    // the function list
    private List<Action> phaseFunctions; // initialized within

    /*-------------------------------------------------------------------------------------------------------*/
    // INITIALIZING AND RUNNING THE GAME
    /*-------------------------------------------------------------------------------------------------------*/

    // start the game
    void Start()
    {

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
    }

    void UpdateGameState()
    {
        for (int i = 0; i < 2; i++)
        {
            players[i].UpdatePlayer();
        }
        UpdateBoard();
    }

    public void UpdateBoard()
    {
        ResetColors();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Square square = gridArray[i][j].GetComponent<Square>();
                if (square.pieceObject != null)
                {
                    //Debug.Log("pieceObject exists");
                    square.UpdateSquare();
                    List<GameObject> auraSquares = square.AdjacentSquares();
                    List<float> distances = square.SquareDistances(auraSquares);
                    aura.AuraLayer(gridArray[i][j], auraSquares, distances);
                    square.UpdateSquare();
                }
            }
        }
    }

    public void ResetColors()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Square square = gridArray[i][j].GetComponent<Square>();
                square.ResetColor();
            }
        }
    }

    /*-------------------------------------------------------------------------------------------------------*/
    // TURN STRUCTURE
    /*-------------------------------------------------------------------------------------------------------*/

    // draws cards for the player
    private void DrawPhase()
    {
        Player player = players[playerTurn];
        for (int i = 0; i < cardsNum; i++)
        {
            Debug.Log("Drawing cards");
            player.DrawCard();
            Debug.Log("Drew cards");
            UpdateGameState();
        }
        phase++;
    }

    // combines cards that the player has selected to combine
    private void CombinePhase()
    {
        if (GeneralCheck(phase, "cardTag", "cardTag"))
        {
            Debug.Log("Combining cards");
            Player player = players[playerTurn];
            List<GameObject> selectedItems = player.selections;
            player.CombineCards(selectedItems[0], selectedItems[1]);
            player.ResetSelections();
            Debug.Log("Combined cards");
            UpdateGameState();
        }
    }

    // places cards that the player has selected to place
    private void PlacePhase()
    {
        if (GeneralCheck(phase, "cardTag", "squareTag"))
        {
            Debug.Log("Placing cards");
            Player player = players[playerTurn];
            List<GameObject> selectedItems = player.selections;
            player.PlaceCard(selectedItems[0], selectedItems[1]);
            player.ResetSelections();
            Debug.Log("Placed card");
            phase++;
            //UpdateGameState();
            UpdateGameState();
        }
    }

    // moves pieces that the player has selected to move
    private void MovePhase()
    {
        if (GeneralCheck(phase, "squareTag", "squareTag"))
        {
            Debug.Log("Moving piece");
            Player player = players[playerTurn];
            List<GameObject> selectedItems = player.selections;
            player.MovePiece(selectedItems[0], selectedItems[1]);
            player.ResetSelections();
            moveTick++;
            if (moveTick > 2)
            {
                phase++;
                moveTick = 0;
            }
            Debug.Log("Moved piece");
            UpdateGameState();
        }
    }

    // apply the aura to all pieces
    private void AuraPhase()
    {
        Debug.Log("Applying auras");
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Square square = gridArray[i][j].GetComponent<Square>();
                if (square.pieceObject != null)
                {
                    aura.ApplyAura(gridArray[i][j]);
                }
            }
        }
        Debug.Log("Applied auras");
        phase++;
        UpdateGameState();
    }

    /*-------------------------------------------------------------------------------------------------------*/
    // CHECKING WHETHER THE ACTION IS VALID
    /*-------------------------------------------------------------------------------------------------------*/

    private bool GeneralCheck(int phase, string itemTagCheck0, string itemTagCheck1)
    {

        // Get the appropriate data
        Player player = players[playerTurn];
        List<GameObject> selectedItems = player.selections;

        //check that there atleast two items, if not wait
        if (selectedItems.Count < 2)
        {
            //Debug.Log("Not enough items selected");  
            return false;
        }
        //check that there are only two items, if not reset
        if (selectedItems.Count > 2)
        {
            Debug.Log("Too many items selected");
            return player.ResetSelections();
        }
        //check that same item has not been selected twice, and if it has then deselect
        if (selectedItems[0] == selectedItems[1])
        {
            Debug.Log("Selected same item twice");
            return player.ResetSelections();
        }
        //check that the items are of the correct type
        if (selectedItems[0].tag != itemTagCheck0 || selectedItems[1].tag != itemTagCheck1)
        {
            Debug.Log("Incorrect types of items");
            return player.ResetSelections();
        }
        //check that, if a card has been selected first, it is in the players hand
        if (itemTagCheck0 == "cardTag")
        {
            if (!player.hand.Contains(selectedItems[0]))
            {
                Debug.Log("First card not in players hand");
                return player.ResetSelections();
            }
            //check that, if a card has been selected second that it is also in the players hand
            if (itemTagCheck1 == "cardTag")
            {
                if (!player.hand.Contains(selectedItems[1]))
                {
                    Debug.Log("Second card not in players hand");
                    return player.ResetSelections();
                }
                //if the selection is both cards, then check they are the same faction
                Card card0 = selectedItems[0].GetComponent<Card>();
                Card card1 = selectedItems[1].GetComponent<Card>();
                if (card0.faction != card1.faction)
                {
                    Debug.Log("Cards not of the same faction");
                    return player.ResetSelections();
                }
            }
        }
        //check that if the second item is a square
        if (itemTagCheck1 == "squareTag")
        {
            //check that theres nothing on it
            Square square1 = selectedItems[1].GetComponent<Square>();
            if (square1.pieceObject != null)
            {
                Debug.Log("Second selected square is occupied");
                return player.ResetSelections();
            }
            // if trying to place a card, check its adjacent to a centrepiece
            if (itemTagCheck0 == "cardTag")
            {
                bool inVicinity = false;
                square1.UpdateSquare();
                List<GameObject> placeableSquares = square1.AdjacentSquares();
                foreach (GameObject squareObject in placeableSquares)
                {           
                    Square adjSquare = squareObject.GetComponent<Square>();
                    adjSquare.GetVectorLocation();
                    Debug.Log("Vector Location (" + adjSquare.vectorLocation.x.ToString() + ", " + adjSquare.vectorLocation.y.ToString() + ")");
                    if (adjSquare.pieceObject != null)
                    {
                        if (adjSquare.pieceObject.tag.Equals("centrePieceTag"))
                        {
                            Debug.Log("Found the centrepiece");
                            inVicinity = true;
                        }
                    }
                }
                if (inVicinity == false)
                {
                    Debug.Log("Not next to centrepiece");
                    return player.ResetSelections();
                }
                Debug.Log(inVicinity);
            }
        }
        // check that if a piece was selected first
        if (itemTagCheck0 == "squareTag")
        {
            //check that theres something on it
            Square square0 = selectedItems[0].GetComponent<Square>();
            if (square0.pieceObject == null)
            {
                Debug.Log("First selected square is empty");
                return player.ResetSelections();
            }
            //check that the piece selected is not paralyzed
            Piece piece0 = square0.pieceObject.GetComponent<Piece>();
            if (piece0.paralyzed)
            {
                Debug.Log("First selected piece is paralyzed");
                return player.ResetSelections();
            }
            //check its owned by the player selecting it
            Player playerOwner = piece0.playerObject.GetComponent<Player>();
            if (playerOwner != players[playerTurn])
            {
                Debug.Log("First selected piece is not owned by you");
                return player.ResetSelections();
            }
            //check that the square being moved to is adjacent
            List<GameObject> adjacentSquares = square0.AdjacentSquares();
            if (!adjacentSquares.Contains(selectedItems[1]))
            {
                Debug.Log("This piece cannot move to that location");
                return player.ResetSelections();
            }
        }
        return true;
    }


}



