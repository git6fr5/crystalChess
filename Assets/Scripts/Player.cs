using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    public GameObject cardsObject;
    public GameObject piecesObject;
    public int deckSize;
    public int drawStart;
    public int drawRegular;

    [HideInInspector] public List<GameObject> deckList = new List<GameObject>();
    [HideInInspector] public List<GameObject> handList = new List<GameObject>();
    private List<GameObject> cardsList = new List<GameObject>();

    [HideInInspector] public bool isTurn = false;

    [System.Serializable] public class IntEvent : UnityEvent<int> { }

    public UnityEvent OnTurnEvent;
    public IntEvent OnDrawEvent;
    public UnityEvent OnCombineEvent;


    void Start()
    {
        GetCards();
        CreateDeck();
        OnTurnEvent.Invoke();
        //DisplayHand();
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
        for (int i =0; i< deckSize; i++)
        {
            deckList.Add(cardsList[Random.Range(0, 4)]);
        }
    }

    void DisplayHand()
    {
        print(handList.Count);
        for (int i = 0; i < handList.Count; i++)
        {
            Instantiate(handList[i], new Vector3(i, 0, 0), Quaternion.identity, gameObject.transform);

        }
    }

    public void OnTurn()
    {
        OnDrawEvent.Invoke(drawRegular);
    }

    public void OnDraw(int drawNum)
    {
        for (int i = 0; i < drawNum; i++)
        {
            handList.Add(deckList[i]);
        }
        deckList.RemoveRange(0, drawNum);
        DisplayHand();
    }
}
