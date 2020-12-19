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
    public UnityEvent OnMoveEvent;
    public UnityEvent OnAuraEvent;

    public LayerMask cardLayer;
    public LayerMask pieceLayer;
    public LayerMask cellLayer;

    public Player player0;
    public Player player1;
    [HideInInspector] public Player player;

    public Text moveCounter;
    public int moveLimit = 3;
    private int moves;

    public int drawStart = 5;

    void Start()
    {
        player = player1;
        OnDrawEvent.Invoke(drawStart);
        player = player0;
        OnDrawEvent.Invoke(drawStart);
        OnTurnEvent.Invoke();
    }

    public void OnTurn()
    {
        player.isTurn = true;
        moves = 0;
        MoveCounter();
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

    public void OnMove()
    {
        if (MoveRules(player.selectionList))
        {
            moves++;
            MoveCounter();
            player.Move();
            player.ResetSelections();
        }
    }

    public void MoveCounter()
    {
        moveCounter.text = moves.ToString();
    }

    public void OnAura()
    {
        player.Aura();
        player.ResetSelections();
        if (player == player0) { player = player1; }
        else if (player == player1) { player = player0; }
        OnTurnEvent.Invoke();
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
        if (!EmptyCheck(selectionList[1])) { return false; }

        print("can combine");
        return true;
    }

    public bool MoveRules(List<GameObject> selectionList)
    {
        if (!GeneralCheck(selectionList)) { return false; }
        print("passed general check");
        if (!TypeCheck(selectionList, cellLayer, cellLayer)) { return false; }
        print("passed type check");
        if (!PieceCheck(selectionList[0])) { return false; }
        if (!EmptyCheck(selectionList[1])) { return false; }
        if (!MoveLimitCheck()) { return false; }

        print("can move");
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

    private bool EmptyCheck(GameObject cellObject)
    {
        if (cellObject.GetComponent<Cell>().piece)
        {
            Debug.Log("This cell is not empty");
            return player.ResetSelections();
        }
        return true;
    }

    private bool PieceCheck(GameObject cellObject)
    {
        if (!cellObject.GetComponent<Cell>().piece)
        {
            Debug.Log("Did not select a piece to move");
            return player.ResetSelections();
        }

        if (!cellObject.GetComponent<Cell>().piece.GetComponent<Piece>().player == player)
        {
            Debug.Log("Did not select your piece");
            return player.ResetSelections();
        }
        return true;
    }

    private bool MoveLimitCheck()
    {
        if (moves >= moveLimit)
        {
            Debug.Log("Used up all moves");
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
