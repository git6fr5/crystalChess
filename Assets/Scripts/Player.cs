using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    /*-------------------------------------------------------------------------------------------------------*/
    // DECLARING THE VARIABLES
    /*-------------------------------------------------------------------------------------------------------*/

    // the player number
    public float playerNumber;

    // the card object bases
    public List<GameObject> ALLCARDS;

    public GameObject centrePieceObject;
    public GameObject phaseObjectBase;
    public GameObject infoFieldBase;
    public GameObject highlightObjectBase;

    // private objects
    public GameObject infoField;
    public GameObject phaseObject;

    public float playerBaseHealth = 10;
    public float playerHealth;

    // the different lists
    public List<GameObject> deck;
    public List<GameObject> hand;
    public List<GameObject> selections;
    private List<GameObject> highlights;

    // the locations of objects
    private Vector3 handPos;
    private Vector3 deckPos;
    //private Vector3 infoPos;
    public Vector3 phasePos;

    // the size of the deck
    public int deckSize = 20;

    // to check if it is currently the players turn
    public bool isTurn;
    public int phase = 0;
    public int numberOfPhases = 5;

    public bool skipButtonPressed;

    /*-------------------------------------------------------------------------------------------------------*/
    // INITIALIZING AND RUNNING THE PLAYER
    /*-------------------------------------------------------------------------------------------------------*/

    void Awake()
    {
        SetLocations();

        NewDeck();
        hand = new List<GameObject>();
        selections = new List<GameObject>();
        highlights = new List<GameObject>();

        centrePieceObject.GetComponent<Piece>().playerObject = gameObject;
        centrePieceObject.GetComponent<Piece>().baseHealth = playerBaseHealth;

        //phaser
        infoField = infoFieldBase; //Instantiate(infoFieldBase, infoPos, Quaternion.identity, gameObject.transform);
        infoField.GetComponent<Text>().text = "Nothing selected";
        //infoField.transform.position = infoPos;
        phaseObject = Instantiate(phaseObjectBase, phasePos, Quaternion.identity, gameObject.transform);
    }

    public void UpdatePlayer()
    {
        playerHealth = centrePieceObject.GetComponent<Piece>().health;
        phaseObject.GetComponent<Phaser>().Sign(phase);
        UpdateHand();
    }

    public void UpdateHand()
    {
        int handLength = hand.Count;
        int n = -1;
        if (playerNumber != 0)
        {
            n = 1;
        }
        for (int i = 0; i < handLength; i++)
        {
            Vector3 pos = handPos + new Vector3(n * i, 0, 0);
            hand[i].transform.position = pos;
            hand[i].GetComponent<Card>().UpdateSprite();
        }
    }

    public void SetLocations()
    {
        int n = -1;
        if (playerNumber != 0)
        {
            n = 1;
        }
        float s = 0;
        if (n == 1) { s = 0f; }
        float interX = gameObject.transform.position.x; 
        float interY = gameObject.transform.position.y; 
        float interZ = gameObject.transform.position.z;
        deckPos = new Vector3(n * 12 + s, 4, 0);
        handPos = new Vector3(n * 10 + s, 0, 0);
        //infoPos = new Vector3(n * 10 + s, -4, 0);
        phasePos = new Vector3(n * 15 + s, 4, 0);
    }

    /*-------------------------------------------------------------------------------------------------------*/
    // THE PLAYER ACTIONS
    /*-------------------------------------------------------------------------------------------------------*/

    public void DrawCard()
    {
        if (deck.Count > 0 && hand.Count < 10)
        {
            hand.Add(deck[0]);
            deck.RemoveAt(0);
        }
    }

    public void CombineCards(GameObject cardObject0, GameObject cardObject1)
    {
        int newLevel = cardObject0.GetComponent<Card>().level + cardObject1.GetComponent<Card>().level;
        CreateCardFromBase(cardObject0, newLevel, true);
        Discard(cardObject0);
        Discard(cardObject1);
    }

    public void PlaceCard(GameObject cardObject, GameObject squareObject)
    {
        GameObject pieceObject = ConvertToPiece(cardObject);
        squareObject.GetComponent<Square>().AddPiece(pieceObject);
        if (squareObject.GetComponent<Square>().pieceObject == null)
        {
            Debug.Log("pieceObject still null");
        }
        Discard(cardObject);
    }

    public void MovePiece(GameObject squareObject0, GameObject squareObject1)
    {
        squareObject1.GetComponent<Square>().AddPiece(squareObject0.GetComponent<Square>().pieceObject);
        squareObject0.GetComponent<Square>().RemovePiece();
    }


    public void SkipPhase()
    {
        if (0 < phase && phase < 4)
        {
            skipButtonPressed = true;
            phase++;
        }
    }

    /*-------------------------------------------------------------------------------------------------------*/
    // MANIPULATION OF CARDS
    /*-------------------------------------------------------------------------------------------------------*/

    public GameObject CreateCardFromBase(GameObject cardObjectBase, int newLevel, bool addToHand)
    {
        //Debug.Log("Instantiating card object");

        GameObject newCardObject = Instantiate(cardObjectBase, deckPos, Quaternion.identity, gameObject.transform);
        newCardObject.GetComponent<Card>().level = newLevel;
        newCardObject.GetComponent<Card>().UpdateSprite();
        if (addToHand) { hand.Add(newCardObject); }
        return newCardObject;
    }

    public GameObject ConvertToPiece(GameObject cardObject)
    {
        Card card = cardObject.GetComponent<Card>();
        GameObject newPieceObject = Instantiate(card.pieceObject, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        newPieceObject.GetComponent<Piece>().level = card.level;
        return newPieceObject;
    }

    public void Discard(GameObject cardObject)
    {
        if (hand.Contains(cardObject)) { Debug.Log("Remove card from hand"); hand.Remove(cardObject); }
        Destroy(cardObject);
        //maybe have to setactive to false here, not sure
    }

    public void NewDeck()
    {
        deck = new List<GameObject>();
        for (int i = 0; i < deckSize; i++)
        {
            GameObject cardObjectBase = RandomCardObject();
            GameObject newCardObject = CreateCardFromBase(cardObjectBase, 1, false);
            deck.Add(newCardObject);
        }
    }

    public GameObject RandomCardObject()
    {
        int numberOfDiffCards = ALLCARDS.Count;
        int randNumber = Random.Range(0, numberOfDiffCards);
        return ALLCARDS[randNumber];
    }


    public bool ResetSelections()
    {
        selections.Clear();
        Highlight(highlights[0], true);
        return false;
    }

    public void Highlight(GameObject highlight, bool clearSelections)
    {
        if (!clearSelections)
        {
            print("adding item to selection");
            highlight.SetActive(true);
            highlights.Add(highlight);
        }
        else
        {
            foreach (GameObject tempHighlight in highlights)
            {
                tempHighlight.SetActive(false);
            }
            highlights.Clear();
        }
    }

}
