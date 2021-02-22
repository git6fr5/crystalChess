using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    public GameRules gameRules;
    public Inspector inspector;

    public Piece centerPiece;

    public GameObject cardsObject;
    public GameObject piecesObject;
    public int deckSize;

    [HideInInspector] public List<GameObject> deckList = new List<GameObject>();
    [HideInInspector] public List<GameObject> handList = new List<GameObject>();
    [HideInInspector] public List<GameObject> selectionList = new List<GameObject>();

    private List<GameObject> cardsList = new List<GameObject>();

    [HideInInspector] public bool isTurn = false;
    [HideInInspector] public bool pauseAction = false;

<<<<<<< HEAD
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
=======
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isTurn && !pauseAction) { ResetSelections(); }
        }
>>>>>>> max_level_5(v2)
    }

    public void GetCards()
    {
        foreach (Transform cardTransform in cardsObject.transform)
        {
            cardsList.Add(cardTransform.gameObject);
        }
    }

    public void CreateDeck()
    {
        for (int i = 0; i < deckSize; i++)
        {
            deckList.Add(cardsList[Random.Range(0, 4)]);
        }
    }

    public void DisplayHand()
    {
        for (int i = 0; i < handList.Count; i++)
        {
            //Instantiate(handList[i], new Vector3(i, 0, 0), Quaternion.identity, gameObject.transform);
            handList[i].transform.localPosition = new Vector3(i, 0, 0);
            handList[i].GetComponent<Card>().StartCard();
        }
<<<<<<< HEAD
        float s = 0;
        if (n == 1) { s = 0f; }
        float interX = gameObject.transform.position.x; 
        float interY = gameObject.transform.position.y; 
        float interZ = gameObject.transform.position.z;
        deckPos = new Vector3(n * 12 + s, 4, 0);
        handPos = new Vector3(n * 10 + s, 0, 0);
        //infoPos = new Vector3(n * 10 + s, -4, 0);
        phasePos = new Vector3(n * 15 + s, 4, 0);
=======
>>>>>>> max_level_5(v2)
    }

    public void Draw(int drawNum)
    {
        if (drawNum > deckList.Count) { drawNum = deckList.Count; }
        if (drawNum + handList.Count > gameRules.handLimit) { drawNum = gameRules.handLimit - handList.Count; }

        print(drawNum); print(deckList.Count);
        for (int i = 0; i < drawNum; i++)
        {
            GameObject newCardObject = Instantiate(deckList[i], Vector3.zero, Quaternion.identity, gameObject.transform);
            newCardObject.GetComponent<Card>().StartCard();
            handList.Add(newCardObject);
        }
        deckList.RemoveRange(0, drawNum);
    }

    public void Combine()
    {
        int newLevel = 0;

        foreach (GameObject cardObject in selectionList)
        {
            Card card = cardObject.GetComponent<Card>();
            newLevel = newLevel + card.level;
        }

        Card newCard = selectionList[1].GetComponent<Card>();
        newCard.level = newLevel;
        newCard.Attach(false);
        newCard.StartCard();


        Discard(selectionList[0]);
    }

    public void Place()
    {
        Card selectedCard = selectionList[0].GetComponent<Card>();
        GameObject newPieceObject = Instantiate(selectedCard.piece.gameObject, Vector3.zero, Quaternion.identity, gameObject.transform);
        Piece newPiece = newPieceObject.GetComponent<Piece>();
        newPiece.level = selectedCard.level;
        newPiece.modifier.GetModifierValues();
        newPiece.StartPiece();

        Cell selectedCell = selectionList[1].GetComponent<Cell>();

        selectedCell.piece = newPiece;
        selectedCell.UpdateCell();

        Discard(selectionList[0]);
    }

    public void Move()
    {
<<<<<<< HEAD
        //Debug.Log("Instantiating card object");

        GameObject newCardObject = Instantiate(cardObjectBase, deckPos, Quaternion.identity, gameObject.transform);
        newCardObject.GetComponent<Card>().level = newLevel;
        newCardObject.GetComponent<Card>().UpdateSprite();
        if (addToHand) { hand.Add(newCardObject); }
        return newCardObject;
=======
        print("moving");
        Cell previousCell = selectionList[0].GetComponent<Cell>();
        Cell selectedCell = selectionList[1].GetComponent<Cell>();

        selectedCell.piece = previousCell.piece;
        previousCell.piece = null;

        previousCell.UpdateCell();
        selectedCell.UpdateCell();

        previousCell.piece = null;
>>>>>>> max_level_5(v2)
    }

    public void Attack()
    {
        print("attacking");
        Cell casterCell = selectionList[0].GetComponent<Cell>();
        Cell targetCell = selectionList[1].GetComponent<Cell>();

        casterCell.piece.modifier.Apply(targetCell.piece);
    }

    public void End()
    {
        Board board = gameRules.gameObject.GetComponent<Board>();
        for (int i = 0; i < board.rows; i++)
        {
            for (int j = 0; j < board.columns; j++)
            {
                Cell cell = board.gridArray[i][j].GetComponent<Cell>();
                if (cell.piece && cell.piece.player != this)
                {
                    cell.piece.Afflict();
                }
            }
        }
    }

    public bool ResetSelections()
    {
        for (int i = 0; i < selectionList.Count; i++)
        {
            if (Card.IsCard(selectionList[i])) { selectionList[i].GetComponent<Card>().Attach(false); }
            else { selectionList[i].GetComponent<Cell>().Attach(false); }
        }
        selectionList.Clear();
        return false;
    }

    public void Discard(GameObject cardObject)
    {
        if (handList.Contains(cardObject)) { Debug.Log("Remove card from hand"); handList.Remove(cardObject); }
        Destroy(cardObject);
    }

    public void InspectCard(Card card)
    {

        inspector.image.sprite = card.gameObject.GetComponent<SpriteRenderer>().sprite;
        inspector.nameText.text = card.faction;
        inspector.levelText.text = card.level.ToString();

        inspector.locationText.text = "";
        inspector.healthText.text = "";
        inspector.drownText.text = "";
        inspector.fearText.text = "";

        inspector.currentObject = card.gameObject;

        inspector.healthBar.SetActive(false);

    }

    public void InspectCell(Cell cell)
    {

        inspector.locationText.text = cell.location.x.ToString() + ", " + cell.location.y.ToString();

        if (cell.piece)
        {
            inspector.image.sprite = cell.piece.gameObject.GetComponent<SpriteRenderer>().sprite;
            inspector.nameText.text = cell.piece.faction;
            inspector.levelText.text = cell.piece.level.ToString();

            Status status = cell.piece.statusObject.GetComponent<Status>();

            GameObject healthBar = status.healthBar;
            Slider healthSlider = healthBar.GetComponent<Slider>();

            inspector.healthText.text = healthSlider.value.ToString() + "/" + healthSlider.maxValue.ToString();
            inspector.healthBar.SetActive(true);
            inspector.healthBar.GetComponent<Slider>().value = healthSlider.value;
            inspector.healthBar.GetComponent<Slider>().maxValue = healthSlider.maxValue;

            GameObject drownBar = status.drownBar;

            inspector.drownText.text = "";
            if (drownBar.transform.parent.gameObject.activeSelf)
            {
                Slider drownSlider = drownBar.GetComponent<Slider>();
                inspector.drownText.text = drownSlider.value.ToString() + "/" + drownSlider.maxValue.ToString();
            }

            GameObject fearBar = status.fearBar;

            inspector.fearText.text = "";
            if (fearBar.transform.parent.gameObject.activeSelf)
            {
                Slider fearSlider = fearBar.GetComponent<Slider>();
                inspector.fearText.text = fearSlider.value.ToString() + "/" + fearSlider.maxValue.ToString();
            }
        }
        else
        {
            inspector.image.sprite = inspector.defaultSprite;
            inspector.nameText.text = "";
            inspector.levelText.text = "";
        }

        inspector.currentObject = cell.gameObject;

    }
}
