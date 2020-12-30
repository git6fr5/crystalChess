using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public Piece piece;
    public Board board;
    [HideInInspector] public Vector2 location;
    [HideInInspector] public List<Cell> adjacentCells;

    public GameObject highlightObject;
    public GameObject selectHighlightObject;


    public void UpdateCell()
    {
        if (piece)
        {
            piece.transform.parent = gameObject.transform;
            piece.transform.localPosition = new Vector3(0, 0, -1);
            piece.UpdatePiece();
        }
    }

    void OnMouseDown()
    {
        SelectFlag();
    }

    void OnMouseOver()
    {
        Highlight(true);
    }

    void OnMouseExit()
    {
        Highlight(false);
    }

    public void DisplayCell()
    {
        GetAdjacentCells();

        if (piece)
        {
            piece.statusObject.SetActive(true);
            piece.modifiers = new List<Modifier>();

            foreach (Cell cell in adjacentCells)
            {
                if (cell.piece && cell.piece.player != piece.player && (!cell.piece.modifier.isBuff))
                {
                    piece.modifiers.Add(cell.piece.modifier);
                }
                else if (cell.piece && cell.piece.player == piece.player && cell.piece.modifier.isBuff)
                {
                    piece.modifiers.Add(cell.piece.modifier);
                }
            }

            piece.DisplayStatus();

        }
    }

    public void SetLocation(int i, int j)
    {
        location = new Vector2(i, j);
    }

    public void GetAdjacentCells()
    {
        adjacentCells = new List<Cell>();

        Vector2[] adjacentVectors = new Vector2[] { new Vector2(1, 0), new Vector2(-1, 0),
            new Vector2(0, 1), new Vector2(0, -1),
            new Vector2(1, 1), new Vector2(-1, -1),
            new Vector2(-1, 1), new Vector2(1, -1) };

        foreach (Vector2 v in adjacentVectors)
        {
            Vector2 newLocation = new Vector2(location.x + v.x, location.y + v.y);
            if (board.ValidLocation((int)newLocation.x, (int)newLocation.y))
            {
                adjacentCells.Add(board.gridArray[(int)newLocation.x][(int)newLocation.y].GetComponent<Cell>());
            }
        }
    }

    void SelectFlag()
    {
        Player player = board.gameRules.player;
        List<GameObject> selectionList = player.selectionList;

        if (piece && piece.player.isTurn == player.isTurn)
        {
            // if selection list is empty
            if (selectionList.Count == 0)
            {
                Select(true);
                Attach(true);
            }
        }
        else
        {
            // if selection list is empty
            if (selectionList.Count == 1)
            {
                Select(true);
                GameObject firstSelection = player.selectionList[0];
                if (firstSelection == gameObject)
                {
                    Select(false);
                    Attach(false);
                    return;
                }
                if (IsCard(firstSelection))
                {
                    player.gameRules.OnPlaceEvent.Invoke();
                }
                // check if that thing is a card
                else
                {
                    player.gameRules.OnMoveEvent.Invoke();
                }
            }
        }
    }

    public void Select(bool select)
    {
        Player player = board.gameRules.player;
        if (select) { player.selectionList.Add(gameObject); }
        else { player.selectionList.Remove(gameObject); }
    }

    public void Attach(bool attach)
    {
        selectHighlightObject.SetActive(attach);
    }

    void Highlight(bool highlight)
    {
        highlightObject.SetActive(highlight);
    }

    bool IsCard(GameObject _object)
    {
        if (_object.GetComponent<Card>()) { return true; }
        else { return false; }
    }
}
