using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameRules : MonoBehaviour
{

    [System.Serializable] public class IntEvent : UnityEvent<int> { }

    public UnityEvent OnTurnEvent;
    public IntEvent OnDrawEvent;
    public UnityEvent OnCombineEvent;
    public UnityEvent OnPlaceEvent;

    public LayerMask cardLayer;
    public LayerMask pieceLayer;
    public LayerMask cellLayer;

    public Player player0;
    public Player player1;
    [HideInInspector] public Player player;

    void Start()
    {
        player = player0;
    }

    public void OnTurn()
    {
        player.isTurn = true;
    }

    public void OnDraw(int drawNum)
    {
        if (drawNum > player.deckList.Count) { drawNum = player.deckList.Count; }
        for (int i = 0; i < drawNum; i++)
        {
            GameObject newCardObject = Instantiate(player.deckList[i], Vector3.zero, Quaternion.identity, player.gameObject.transform);
            player.handList.Add(newCardObject);
        }
        player.deckList.RemoveRange(0, drawNum);
        player.DisplayHand();
    }

    public void OnCombine()
    {
        if (CombineRules(player.selectionList))
        {
            player.Combine();
            player.DisplayHand();
            player.ResetSelections();
        }
    }

    public void OnPlace()
    {
        if (PlaceRules(player.selectionList))
        {
            player.Place();
            player.DisplayHand();
            player.ResetSelections();
        }
    }

    public bool CombineRules(List<GameObject> selectionList)
    {
        if (!GeneralCheck(selectionList)) { return false; }
        print("passed general check");
        if (!TypeCheck(selectionList, cardLayer, cardLayer)) { return false; }
        print("passed type check");
        if (!FactionCheck(selectionList)) { return false; }
        print("passed faction check");

        print("can combine");
        return true;
    }

    public bool PlaceRules(List<GameObject> selectionList)
    {
        if (!GeneralCheck(selectionList)) { return false; }
        print("passed general check");
        if (!TypeCheck(selectionList, cardLayer, cellLayer)) { return false; }
        print("passed type check");
        if (!EmptyCheck(selectionList)) { return false; }

        print("can combine");
        return true;
    }

    private bool TypeCheck(List<GameObject> selectionList, LayerMask layer1, LayerMask layer2)
    {

        LayerMask selection1 = LayerMask.GetMask(LayerMask.LayerToName(selectionList[0].layer));
        LayerMask selection2 = LayerMask.GetMask(LayerMask.LayerToName(selectionList[1].layer));

        if (selection1 != layer1 || selection2 != layer2)
        {
            Debug.Log("Incorrect types of selections");
            return player.ResetSelections();
        }
        return true;
    }

    private bool FactionCheck(List<GameObject> selectionList)
    {
        if (selectionList[0].tag != selectionList[1].tag)
        {
            Debug.Log("These are of different factions");
            return player.ResetSelections();
        }
        return true;
    }

    private bool EmptyCheck(List<GameObject> selectionList)
    {
        if (selectionList[1].GetComponent<Cell>().piece)
        {
            Debug.Log("This cell is not empty");
            return player.ResetSelections();
        }
        return true;
    }

    private bool GeneralCheck(List<GameObject> selectionList)
    {
        //check that there atleast two items, if not wait
        if (selectionList.Count < 2)
        {
            Debug.Log("Not enough items selected");  
            return false;
        }
        //check that there are only two items, if not reset
        if (selectionList.Count > 2)
        {
            Debug.Log("Too many items selected");
            return player.ResetSelections();
        }
        //check that same item has not been selected twice, and if it has then deselect
        if (selectionList[0] == selectionList[1])
        {
            Debug.Log("Selected same item twice");
            return player.ResetSelections();
        }
        
        return true;
    }

}
