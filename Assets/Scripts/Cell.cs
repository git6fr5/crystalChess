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

    public void UpdateCell()
    {
        if (piece)
        {
            piece.transform.parent = gameObject.transform;
            piece.transform.localPosition = new Vector3(0, 0, -5);
        }
    }

    void OnMouseDown()
    {
        Select();
    }

    void Select()
    {
        print("selected a cell");

        if (piece && board.gameRules.player.selectionList.Count == 0 && piece.player == board.gameRules.player)
        {
            board.gameRules.player.selectionList.Add(gameObject);
            board.gameRules.player.Highlight();
        }
        else if (!piece && board.gameRules.player.selectionList.Count == 1)
        {
            board.gameRules.player.selectionList.Add(gameObject);
            board.gameRules.player.Highlight();
        }
        board.gameRules.player.InspectCell(this);
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
}
