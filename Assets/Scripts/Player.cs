using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    public TurnSystem turnSystem;

    public GameObject cardsObject;
    public GameObject piecesObject;
    public int deckSize;
    public int drawStart;
    public int drawRegular;

    [HideInInspector] public List<GameObject> deckList = new List<GameObject>();
    [HideInInspector] public List<GameObject> handList = new List<GameObject>();
    private List<GameObject> cardsList = new List<GameObject>();

    void Start()
    {
        GetCards();
        CreateDeck();
        turnSystem.OnTurnEvent.Invoke();
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

    public void DisplayHand()
    {
        print(handList.Count);
        for (int i = 0; i < handList.Count; i++)
        {
            Instantiate(handList[i], new Vector3(i, 0, 0), Quaternion.identity, gameObject.transform);
        }
    }
}
