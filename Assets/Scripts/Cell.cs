using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public Piece piece;
    private Board board;

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
        if (piece)
        {
            DisplayHealth();
            DisplayModifiers();
        }
    }

    private void DisplayHealth()
    {
        //
    }

    private void DisplayModifiers()
    {
        //
    }
}
