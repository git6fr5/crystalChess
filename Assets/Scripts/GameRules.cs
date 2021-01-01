using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
    public UnityEvent OnAttackEvent;
    public UnityEvent OnEndEvent;

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
    public int maxCardLevel = 9;


    public ControlPanelAnimation controlPanelAnimator;

    public GameObject controlPanel;

    void Start()
    {
        print("staring game rules");
        player = player1; player.GetCards(); player.CreateDeck();
        OnTurnEvent.Invoke();
        OnDrawEvent.Invoke(drawStart);

        player = player0; player.GetCards(); player.CreateDeck();
        OnTurnEvent.Invoke();
        OnDrawEvent.Invoke(drawStart);

        player0.isTurn = false; player1.isTurn = false;

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
            Play(controlPanelAnimator.combineAnim, player.selectionList[1].transform.position);
            PlayAudio(controlPanelAnimator.combineAudio);
            player.pauseAction = true; player.selectionList[0].SetActive(false);
            StartCoroutine(IECombine(8f/6f));
        }
    }

    private IEnumerator IECombine(float delay)
    {
        yield return new WaitForSeconds(delay);

        Play(controlPanelAnimator.idleAnim, Vector3.zero);

        ActionCounter();
        player.Combine();
        player.DisplayHand();
        player.ResetSelections();

        player.pauseAction = false;

        yield return null;
    }

    public void OnPlace()
    {
        if (PlaceRules(player.selectionList))
        {
            Play(controlPanelAnimator.placeAnim, player.selectionList[1].transform.position);
            PlayAudio(controlPanelAnimator.placeAudio);
            player.pauseAction = true; player.selectionList[0].SetActive(false);
            StartCoroutine(IEPlace(8f / 6f));
        }
    }

    private IEnumerator IEPlace(float delay)
    {
        yield return new WaitForSeconds(delay);

        Play(controlPanelAnimator.idleAnim, Vector3.zero);

        ActionCounter();
        player.Place();
        player.DisplayHand();
        player.ResetSelections();

        player.pauseAction = false;

        yield return null;
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

    public void OnAttack()
    {
        print("onAttack");
        if (AttackRules(player.selectionList))
        {
            ActionCounter();
            player.Attack();
            player.ResetSelections();
        }
    }

    public void OnEnd()
    {
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
        if (!LevelCheck(selectionList)) { return false; }
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
        if (!AdjacentCheck(selectionList)) { return false; }
        if (!LimitCheck()) { return false; }
        if (!FearCheck(selectionList[0])) { return false; }

        print("can move");
        return true;
    }

    public bool AttackRules(List<GameObject> selectionList)
    {
        if (!GeneralCheck(selectionList)) { return false; }
        print("passed general check");
        if (!TypeCheck(selectionList, cellLayer, cellLayer)) { return false; }
        print("passed type check");
        if (!PieceCheck(selectionList[0])) { return false; }
        if (!NotEmptyCheck(selectionList[1])) { return false; }
        if (!AdjacentCheck(selectionList)) { return false; }
        if (!LimitCheck()) { return false; }

        print("can attack");
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

    private bool LevelCheck(List<GameObject> selectionList)
    {
        Card card0 = selectionList[0].GetComponent<Card>();
        Card card1 = selectionList[1].GetComponent<Card>();

        if (card1.level + card0.level > maxCardLevel)
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

    private bool NotEmptyCheck(GameObject cellObject)
    {
        if (!cellObject.GetComponent<Cell>().piece)
        {
            Debug.Log("This cell is  empty");
            return player.ResetSelections();
        }
        return true;
    }

    private bool AdjacentCheck(List<GameObject> selectionList)
    {
        selectionList[0].GetComponent<Cell>().GetAdjacentCells();
        foreach (Cell adjacentCell in selectionList[0].GetComponent<Cell>().adjacentCells)
        {
            if (adjacentCell.gameObject == selectionList[1])
            {
                return true;
            }
        }

        Debug.Log("This cell is too far away");
        return player.ResetSelections(); ;
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
        /*if (piece.isParalyzed)
        {
            Debug.Log("piece is paralyzed");
            return player.ResetSelections();
        }*/
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

    public void Play(AnimationClip animation, Vector3 animPosition)
    {
        controlPanelAnimator.gameObject.transform.position = new Vector3(animPosition.x, animPosition.y, animPosition.z - 1);
        controlPanelAnimator.animator.Play(animation.name);
    }

    public void PlayAudio(AudioClip audio)
    {
        controlPanelAnimator.audioSource.clip = audio;
        controlPanelAnimator.audioSource.Play();
    }
}
