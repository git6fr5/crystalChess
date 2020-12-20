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

    public void Draw(int drawNum)
    {
        if (drawNum > deckList.Count) { drawNum = deckList.Count; }
        for (int i = 0; i < drawNum; i++)
        {
            GameObject newCardObject = Instantiate(deckList[i], Vector3.zero, Quaternion.identity, gameObject.transform);
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

        GameObject newCardObject = Instantiate(selectionList[0], Vector3.zero, Quaternion.identity, gameObject.transform);
        Card newCard = newCardObject.GetComponent<Card>();
        newCard.level = newLevel;
        newCard.UpdateCard();

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
        newPiece.modifier.GetModifierValues();
        newPiece.UpdatePiece();

        Cell selectedCell = selectionList[1].GetComponent<Cell>();
        selectedCell.piece = newPiece;
        selectedCell.UpdateCell();
        selectedCell.DisplayCell();

        foreach (Cell adjacentCell in selectedCell.adjacentCells)
        {
            adjacentCell.DisplayCell();
        }


        Discard(selectionList[0]);
    }

    public void Move()
    {
        print("moving");
        Cell previousCell = selectionList[0].GetComponent<Cell>();
        Cell selectedCell = selectionList[1].GetComponent<Cell>();

        selectedCell.piece = previousCell.piece;
        previousCell.piece = null;

        previousCell.UpdateCell();
        previousCell.DisplayCell();

        selectedCell.UpdateCell();
        selectedCell.DisplayCell();

        foreach (Cell adjacentCell in previousCell.adjacentCells)
        {
            adjacentCell.DisplayCell();
        }

        foreach (Cell adjacentCell in selectedCell.adjacentCells)
        {
            adjacentCell.DisplayCell();
        }

        previousCell.piece = null;
    }

    public void Aura()
    {
        Board board = gameRules.gameObject.GetComponent<Board>();
        for (int i = 0; i < board.rows; i++)
        {
            for (int j = 0; j< board.columns; j++)
            {
                Cell cell = board.gridArray[i][j].GetComponent<Cell>();
                if (cell.piece && cell.piece.playerObject != gameObject)
                {
                    cell.DisplayCell();
                    foreach(Modifier modifier in cell.piece.modifiers)
                    {
                        if (!modifier.isBuff)
                        {
                            modifier.targetPiece = cell.piece;
                            modifier.Apply();
                        }
                    }
                    cell.DisplayCell();
                    foreach (Cell adjacentCell in cell.adjacentCells)
                    {
                        adjacentCell.DisplayCell();
                    }
                }
                else if (cell.piece && cell.piece.playerObject == gameObject)
                {
                    cell.DisplayCell();
                    foreach (Modifier modifier in cell.piece.modifiers)
                    {
                        if (modifier.isBuff)
                        {
                            modifier.targetPiece = cell.piece;
                            modifier.Apply();
                        }
                    }
                    if (!cell.piece.isDrowning) { cell.piece.drownDuration = 0f; }
                    if (cell.piece.isParalyzed) { cell.piece.paralyzeDuration = cell.piece.paralyzeDuration - cell.piece.paralyzeRecovery; }
                    cell.DisplayCell();
                    foreach (Cell adjacentCell in cell.adjacentCells)
                    {
                        adjacentCell.DisplayCell();
                    }
                }
            }
        }
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

    public void InspectCard(Card card)
    {

        inspector.image.sprite = card.gameObject.GetComponent<SpriteRenderer>().sprite;
        inspector.nameText.text = card.faction;
        inspector.levelText.text = card.level.ToString();
        
        inspector.locationText.text = "";
        inspector.healthText.text = "";

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
            inspector.image.sprite = null;
            inspector.nameText.text = "";
            inspector.levelText.text = "";
        }
    }
}
