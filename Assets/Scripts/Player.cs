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
        gameRules.OnTurnEvent.Invoke();
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
            handList[i].transform.position = new Vector3(i, 0, 0);
        }
    }

    public void OnTurn()
    {
        isTurn = true;
        gameRules.OnDrawEvent.Invoke(drawRegular);
    }

    public void OnDraw(int drawNum)
    {
        for (int i = 0; i < drawNum; i++)
        {
            GameObject newCardObject = Instantiate(deckList[i], Vector3.zero, Quaternion.identity, gameObject.transform);
            handList.Add(newCardObject);
        }
        deckList.RemoveRange(0, drawNum);
        DisplayHand();
    }

    public void OnCombine()
    {
        if (gameRules.CombineRules(selectionList))
        {
            Combine();
            DisplayHand();
        }
    }

    private void Combine()
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

    public void Discard(GameObject cardObject)
    {
        if (handList.Contains(cardObject)) { Debug.Log("Remove card from hand"); handList.Remove(cardObject); }
        Destroy(cardObject);
    }
}
