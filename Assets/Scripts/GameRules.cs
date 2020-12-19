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

    public LayerMask cardLayer;
    public LayerMask pieceLayer;

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

    private bool TypeCheck(List<GameObject> selectionList, LayerMask layer1, LayerMask layer2)
    {

        LayerMask selection1 = LayerMask.GetMask(LayerMask.LayerToName(selectionList[0].layer));
        LayerMask selection2 = LayerMask.GetMask(LayerMask.LayerToName(selectionList[1].layer));

        if (selection1 != layer1 || selection2 != layer2)
        {
            Debug.Log("Incorrect types of selections");
            return ResetSelections(selectionList);
        }
        return true;
    }

    private bool FactionCheck(List<GameObject> selectionList)
    {
        if (selectionList[0].tag != selectionList[1].tag)
        {
            Debug.Log("These are of different factions");
            return ResetSelections(selectionList);
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
            return ResetSelections(selectionList);
        }
        //check that same item has not been selected twice, and if it has then deselect
        if (selectionList[0] == selectionList[1])
        {
            Debug.Log("Selected same item twice");
            return ResetSelections(selectionList);
        }
        
        return true;
    }

    public bool ResetSelections(List<GameObject> selectionList)
    {
        selectionList.Clear();
        return false;
    }

}
