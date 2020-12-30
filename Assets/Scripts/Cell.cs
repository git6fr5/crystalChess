using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public Piece piece;
    public Board board;
    [HideInInspector] public Vector2 location;
    [HideInInspector] public List<Cell> adjacentCells;

    public GameObject highlight;
    public GameObject selectHighlight;


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
        Highlight();
    }

    void OnMouseExit()
    {
        UnHighlight();
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
        if (piece)
        {
            // if selection list is empty
            if (board.gameRules.player.selectionList.Count == 1)
            {
                // add to selection list, make it follow mouse
                if (board.gameRules.player.selectionList[0] == gameObject)
                {
                    Select(false, 0);
                }
            }
            else if (board.gameRules.player.selectionList.Count == 0 && piece.player.isTurn == board.gameRules.player.isTurn)
            {
                Select(true, 0);
            }
        }
        else
        {
            // if selection list is empty
            if (board.gameRules.player.selectionList.Count == 1)
            {
                Select(true, 1);
            }
        }
    }

    public void Select(bool selecting, int index)
    {
        Player player = board.gameRules.player;
        if (index == 0)
        {
            selectHighlight.SetActive(selecting);
            if (selecting)
            {
                player.selectionList.Add(gameObject);
            }
            else
            {
                player.selectionList.RemoveAt(index);
            }
        }
        else if (index == 1)
        {
            selectHighlight.SetActive(selecting);
            if (selecting)
            {
                player.selectionList.Add(gameObject);
                if (player.selectionList[0].GetComponent<Card>())
                {
                    player.gameRules.OnPlaceEvent.Invoke();
                }
                else if (player.selectionList[0].GetComponent<Cell>())
                {
                    player.gameRules.OnMoveEvent.Invoke();
                }
            }
        }
        else if (index == -1)
        {
            selectHighlight.SetActive(selecting);
        }
    }

    void Highlight()
    {
        highlight.SetActive(true);
    }

    void UnHighlight()
    {
        highlight.SetActive(false);
    }
}
