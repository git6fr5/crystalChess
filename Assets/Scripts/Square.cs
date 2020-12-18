using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour
{

    /*-------------------------------------------------------------------------------------------------------*/
    // DECLARING VARIABLES
    /*-------------------------------------------------------------------------------------------------------*/

    // the piece it holds
    public GameObject pieceObject;
    // the board its spawned to
    public GameObject boardObject;
    public Vector2 vectorLocation;

    /*-------------------------------------------------------------------------------------------------------*/
    // INITIALIZING AND UPDATING THE SQUARE
    /*-------------------------------------------------------------------------------------------------------*/

    void Awake()
    {
        boardObject = gameObject.transform.parent.gameObject;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        ResetColor();
    }

    public void UpdateSquare()
    {
        vectorLocation = GetVectorLocation();
        if (pieceObject != null)
        {
            pieceObject.GetComponent<Piece>().UpdateSprite();
        }
    }

    public void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
    }

    /*-------------------------------------------------------------------------------------------------------*/
    // ADDING AND REMOVING PIECES TO THE SQUARE
    /*-------------------------------------------------------------------------------------------------------*/

    public void AddPiece(GameObject addPieceObject)
    {
        Vector3 spawnPos = gameObject.transform.position + new Vector3(0, 0, -5);
        GameObject playerObject = addPieceObject.GetComponent<Piece>().playerObject;
        //Debug.Log("pieceObject: " + pieceObject.name);
        if (pieceObject == null)
        {
            Debug.Log("piece object was not null");
            pieceObject = Instantiate(addPieceObject, spawnPos, Quaternion.identity, playerObject.transform);
        }
        if (pieceObject.tag == "centrePieceTag")
        {
            playerObject.GetComponent<Player>().centrePieceObject = pieceObject;
        }
    }

    public void RemovePiece()
    {
        if (pieceObject != null)
        {
            Destroy(pieceObject);
            pieceObject = null;
        }
    }


    /*-------------------------------------------------------------------------------------------------------*/
    // SELECTING A SQUARE
    /*-------------------------------------------------------------------------------------------------------*/

    public void OnMouseDown()
    {
        Debug.Log("In Square, OnMouseDown; Selected a square");
        Board board = boardObject.GetComponent<Board>();
        Player player = board.players[board.playerTurn];
        if (pieceObject != null)
        {
            Debug.Log("selected a not null square");
            Piece piece = pieceObject.GetComponent<Piece>();
            Player piecePlayer = piece.playerObject.GetComponent<Player>();
            if (piecePlayer.isTurn)
            {
                if (piecePlayer.phase == 3 && piecePlayer.selections.Count < 1)
                {
                    Debug.Log("During move phase, added square 0 to selections");
                    piecePlayer.selections.Add(gameObject);
                    player.Highlight(gameObject.transform.GetChild(0).gameObject, false);
                }
            }
            player.infoField.GetComponent<Text>().text = pieceObject.GetComponent<Piece>().Properties();
        }
        if (pieceObject == null)
        {
            if (player.phase == 3 && player.selections.Count > 0)
            {
                Debug.Log("During move phase, added square 1 to selections");
                player.selections.Add(gameObject);
                player.Highlight(gameObject.transform.GetChild(0).gameObject, false);
            }
            if (player.phase == 2 && player.selections.Count > 0)
            {
                Debug.Log("During place phase, added square to selections");
                player.selections.Add(gameObject);
                player.Highlight(gameObject.transform.GetChild(0).gameObject, false);
            }
        }
        // false for not clearing selections
        
    }

    /*-------------------------------------------------------------------------------------------------------*/
    // DEALING WITH SQUARES THROUGH THE USE OF VECTORS
    /*-------------------------------------------------------------------------------------------------------*/

    public List<GameObject> VectorFindSquares(Vector2[] vectors, int radius)
    {
        List<GameObject> foundSquares = new List<GameObject>();
        Board board = boardObject.GetComponent<Board>();
        GameObject[][] gridArray = board.gridArray; int height = board.height; int width = board.width;

        foreach (Vector2 vector in vectors)
        {
            /*for (int i = 1; i < radius + 1; i++)
            {
                for (int j = 1; j < radius + 1; j++)
                {*/
                    Vector2 newLocation = new Vector2(vectorLocation.x + vector.x, vectorLocation.y + vector.y);
                    if (ValidLocation(newLocation))
                    {
                        //Debug.Log("adding square (" + newLocation.x.ToString() + ", " + newLocation.y.ToString() + ")");
                        GameObject newSquare = gridArray[(int)newLocation.x][(int)newLocation.y];
                        foundSquares.Add(newSquare);
                    }
               /* }   
            }*/
        }
        return foundSquares;
    }

    public List<GameObject> AdjacentSquares()
    {

        List<GameObject> adjacentSquares = new List<GameObject>();
        Vector2[] adjacentVectors = new Vector2[] { new Vector2(1, 0), new Vector2(-1, 0),
            new Vector2(0, 1), new Vector2(0, -1),
            new Vector2(1, 1), new Vector2(-1, -1),
            new Vector2(-1, 1), new Vector2(1, -1) };

        adjacentSquares = VectorFindSquares(adjacentVectors, 1);
        return adjacentSquares;
    }

    public List<GameObject> AuraSquares()
    {

        List<GameObject> auraSquares = new List<GameObject>();
        Debug.Log("getting aurascript");
        Debug.Log(boardObject.name);
        Aura aura = boardObject.GetComponent<Board>().aura;
        int radius = pieceObject.GetComponent<Piece>().radius;
        Vector2[] auraVectors = aura.GetAuraVectors(pieceObject);

        auraSquares = VectorFindSquares(auraVectors, radius);
        return auraSquares;
    }

    public List<float> SquareDistances(List<GameObject> squareObjects)
    {

        Board board = boardObject.GetComponent<Board>();
        GameObject[][] gridArray = board.gridArray; int height = board.height; int width = board.width;

        Vector2 vector0 = vectorLocation;
        List<float> distances = new List<float>();
        foreach (GameObject squareObject in squareObjects)
        {
            Square square = squareObject.GetComponent<Square>();
            Vector2 vector1 = square.GetVectorLocation();
            distances.Add((vector0 - vector1).magnitude);
        }
        return distances;
    }

    public bool ValidLocation(Vector2 location)
    {

        Board board = boardObject.GetComponent<Board>();
        int height = board.height; int width = board.width;

        if (location.x < height && location.x >= 0 && location.y < width && location.y >= 0)
        {
            //Debug.Log("square was valid (" + location.x.ToString() + ", " + location.y.ToString() + ")");
            return true;
        }
        return false;
    }

    public Vector2 GetVectorLocation()
    {

        Board board = boardObject.GetComponent<Board>();
        GameObject[][] gridArray = board.gridArray; int height = board.height; int width = board.width;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (gridArray[i][j] == gameObject)
                {
                    return new Vector2(i, j);
                }
            }
        }
        return new Vector2(0, 0);
    }
}

