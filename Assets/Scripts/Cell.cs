using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public Piece piece;
    public Board board;
    [HideInInspector] public Vector2 location;
    [HideInInspector] public List<Cell> adjacentCells;

    public SpriteRenderer spriteRenderer;
    public GameObject highlightObject;
    public GameObject selectHighlightObject;
    private Color baseColor = new Color(1f, 1f, 1f, 1f);

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

    public void SetLocation(int i, int j)
    {
        location = new Vector2(i, j);
    }

    public void TintAdjacentCells(Color color)
    {
        GetAdjacentCells();

        foreach (Cell adjacentCell in adjacentCells)
        {
            adjacentCell.spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
        }
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

        if (piece)
        {
            // if selection list is empty
            if (selectionList.Count == 0 && piece.player.isTurn == player.isTurn)
            {
                Select(true);
                Attach(true);
            }
            else if (selectionList.Count == 1 && IsCell(selectionList[0]))
            {
                Piece attackingPiece = selectionList[0].GetComponent<Cell>().piece;
                print("attempting to select to attack");
                if ((attackingPiece.modifier.isBuff && piece.player == attackingPiece.player) || (!attackingPiece.modifier.isBuff && piece.player != attackingPiece.player))
                {
                    print("could select to attack");
                    Select(true);
                    player.gameRules.OnAttackEvent.Invoke();
                }
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

                // check if that thing is a cell
                if (IsCell(firstSelection))
                {
                    player.gameRules.OnMoveEvent.Invoke();
                }
                else
                {
                    player.gameRules.OnPlaceEvent.Invoke();

                }
            }
        }
    }

    public void Select(bool select)
    {
        Player player = board.gameRules.player;
        if (select) { player.selectionList.Add(gameObject); }
        else { player.selectionList.Remove(gameObject); }
        player.InspectCell(this);
    }

    public void Attach(bool attach)
    {
        selectHighlightObject.SetActive(attach);
        if (piece)
        {
            piece.Attach(attach);
        }
        /*if (attach && piece)
        {
            TintAdjacentCells(piece.modifier.color);
        }
        else
        {
            TintAdjacentCells(baseColor);
        }*/
    }

    void Highlight(bool highlight)
    {
        highlightObject.SetActive(highlight);
    }

    public static bool IsCell(GameObject _object)
    {
        if (_object.GetComponent<Cell>()) { return true; }
        else { return false; }
    }
}
