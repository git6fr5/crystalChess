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

    public Text actionCounter;
    public int actionLimit = 3;
    private int actions;

    public int drawStart = 5;

    public GameObject controlPanel;

    void Start()
    {
        player = player1;
        OnTurnEvent.Invoke();
        OnDrawEvent.Invoke(drawStart);

        player = player0;
        OnTurnEvent.Invoke();
        OnDrawEvent.Invoke(drawStart);

        OnTurnEvent.Invoke();
    }

    public void OnTurn()
    {
        player.isTurn = true;
        actions = -1;
        ActionCounter();
    }

    public void OnDraw(int drawNum)
    {
        if (DrawRules())
        {
            ActionCounter();
            player.Draw(drawNum);
            player.DisplayHand();
            player.ResetSelections();
        }
    }

    public void OnCombine()
    {
        if (CombineRules(player.selectionList))
        {
            ActionCounter();
            player.Combine();
            player.DisplayHand();
            player.ResetSelections();
        }
    }

    public void OnPlace()
    {
        if (PlaceRules(player.selectionList))
        {
            ActionCounter();
            player.Place();
            player.DisplayHand();
            player.ResetSelections();
        }
    }

    public void OnMove()
    {
        if (MoveRules(player.selectionList))
        {
            ActionCounter();
            player.Move();
            player.ResetSelections();
        }
    }

    public void OnAura()
    {
        player.Aura();
        player.ResetSelections();
        player.isTurn = false;
        if (player == player0) { player = player1; }
        else if (player == player1) { player = player0; }
        OnTurnEvent.Invoke();
    }

    public bool DrawRules()
    {
        if (!LimitCheck()) { return false; }

        print("can draw");
        return true;
    }

    public bool CombineRules(List<GameObject> selectionList)
    {
        if (!GeneralCheck(selectionList)) { return false; }
        print("passed general check");
        if (!TypeCheck(selectionList, cardLayer, cardLayer)) { return false; }
        print("passed type check");
        if (!FactionCheck(selectionList)) { return false; }
        if (!LimitCheck()) { return false; }
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
        if (!CenterCheck(selectionList[1])) { return false; }
        if (!LimitCheck()) { return false; }
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
        if (!LimitCheck()) { return false; }
        if (!FearCheck(selectionList[0])) { return false; }

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

        if (cellObject.GetComponent<Cell>().piece.GetComponent<Piece>().player != player)
        {
            Debug.Log("Did not select your piece");
            return player.ResetSelections();
        }
        return true;
    }

    private bool CenterCheck(GameObject cellObject)
    {
        Cell selectedCell = cellObject.GetComponent<Cell>();
        selectedCell.GetAdjacentCells();
        List<Cell> adjacentCells = selectedCell.adjacentCells;

        foreach (Cell adjacentCell in adjacentCells)
        {
            if(adjacentCell.piece && adjacentCell.piece == player.centerPiece)
            {
                return true;
            }
        }

        Debug.Log("No centerpiece nearby");
        return player.ResetSelections();
    }

    public void ActionCounter()
    {
        actions++;
        actionCounter.text = actions.ToString();
    }

    private bool LimitCheck()
    {
        if (actions >= actionLimit)
        {
            Debug.Log("Used up all actions");
            return player.ResetSelections();
        }
        return true;
    }

    private bool FearCheck(GameObject cellObject)
    {
        Piece piece = cellObject.GetComponent<Cell>().piece;
        if (piece.isParalyzed)
        {
            Debug.Log("piece is paralyzed");
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
            return player.ResetSelections();
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
