using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    public GameRules gameRules;

    public GameObject cardsObject;
    public GameObject piecesObject;
    public int deckSize;
    public int drawStart;
    public int drawRegular;

    [HideInInspector] public List<GameObject> deckList = new List<GameObject>();
    [HideInInspector] public List<GameObject> handList = new List<GameObject>();
    [HideInInspector] public List<GameObject> selectionList = new List<GameObject>();

    private List<GameObject> cardsList = new List<GameObject>();

    [HideInInspector] public bool isTurn = false;

    void Start()
    {
        GetCards();
        CreateDeck();
    }

    void GetCards()
    {
        foreach (Transform cardTransform in cardsObject.transform)
        {
            cardsList.Add(cardTransform.gameObject);
        }
    }

    void CreateDeck()
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
        }
    }

    public void Combine()
    {
        int newLevel = 0;

        foreach (GameObject cardObject in selectionList)
        {
            Card card = cardObject.GetComponent<Card>();
            newLevel = newLevel + card.level;
        }

        GameObject newCardObject = Instantiate(selectionList[0], Vector3.zero, Quaternion.identity, gameObject.transform);
        Card newCard = newCardObject.GetComponent<Card>();
        newCard.level = newLevel;
        newCard.SetSprite();

        handList.Add(newCardObject);

        foreach (GameObject cardObject in selectionList)
        {
            Discard(cardObject);
        }
    }

    public void Place()
    {
        Card selectedCard = selectionList[0].GetComponent<Card>();
        GameObject newPieceObject = Instantiate(selectedCard.pieceObject, Vector3.zero, Quaternion.identity, gameObject.transform);
        Piece newPiece = newPieceObject.GetComponent<Piece>();
        newPiece.level = selectedCard.level;
        newPiece.SetSprite();

        Cell selectedCell = selectionList[1].GetComponent<Cell>();
        selectedCell.piece = newPiece;
        selectedCell.SetPiece();
        selectedCell.DisplayCell();


        Discard(selectionList[0]);
    }

    public void Move()
    {
        print("moving");
        Cell previousCell = selectionList[0].GetComponent<Cell>();
        Cell selectedCell = selectionList[1].GetComponent<Cell>();

        selectedCell.piece = previousCell.piece;
        selectedCell.SetPiece();
        selectedCell.DisplayCell();

        previousCell.piece = null;
    }

    public bool ResetSelections()
    {
        selectionList.Clear();
        return false;
    }

    public void Discard(GameObject cardObject)
    {
        if (handList.Contains(cardObject)) { Debug.Log("Remove card from hand"); handList.Remove(cardObject); }
        Destroy(cardObject);
    }
}
