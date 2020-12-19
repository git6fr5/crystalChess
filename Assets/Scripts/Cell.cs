using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public Piece piece;
    private Board board;
    private Vector2 location;
    private List<Cell> adjacentCells;

    void Start()
    {
        board = gameObject.transform.parent.GetComponent<Board>();
    }

    public void SetPiece()
    {
        piece.transform.parent = gameObject.transform;
        piece.transform.localPosition = new Vector3(0, 0, -5);
    }

    void OnMouseDown()
    {
        Select();
    }

    void Select()
    {
        print("selected a cell");
        board.gameRules.player.selectionList.Add(gameObject);
    }

    public void DisplayCell()
    {
        GetAdjacentCells();

        if (piece)
        {
            piece.statusObject.SetActive(true);

            foreach (Cell cell in adjacentCells)
            {
                SpriteRenderer spriteRenderer = cell.GetComponent<SpriteRenderer>();
                //spriteRenderer.color = piece.modifier.color;
            }

        }

        foreach (Cell cell in adjacentCells)
        {
            if (cell.piece && cell.piece.player != piece.player)
            {
                piece.modifiers.Add(cell.piece.modifier);
            }
        }

        piece.DisplayStatus();
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
