﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int rows;
    public int columns;

    public int scale = 2;

    public GameObject[][] gridArray;
    public GameObject cellObject;

    public GameRules gameRules;

    // Start is called before the first frame update
    void Start()
    {
        CreateBoard();
        CenterObjects();
        gameRules.ClearInspector();
    }

    void CreateBoard()
    {
        gridArray = new GameObject[rows][];
        for (int i = 0; i < gridArray.Length; i++)
        {
            gridArray[i] = new GameObject[columns];
            for (int j = 0; j < gridArray[i].Length; j++)
            {
                AttachCell(i, j);
            }
        }
    }

    void AttachCell(int i, int j)
    {
        gridArray[i][j] = Instantiate(cellObject, Vector3.zero, Quaternion.identity, gameObject.transform);

        Vector3 cellPosition = new Vector3(i - (rows - 1) / 2, j - (columns - 1) / 2, 0) * scale;
        gridArray[i][j].transform.localPosition = cellPosition;
        gridArray[i][j].GetComponent<Cell>().SetLocation(i, j);
        gridArray[i][j].SetActive(true);
    }

    public bool ValidLocation(int i, int j)
    {
        if (i < rows && j < columns && i >= 0 && j >= 0)
        {
            return true;
        }
        return false;
    }

    void CenterObjects()
    {
        Cell startPos0 = gridArray[0][(int)Mathf.Floor(columns / 2)].GetComponent<Cell>();
        Cell startPos1 = gridArray[rows - 1][(int)Mathf.Floor(columns / 2)].GetComponent<Cell>();

        Piece centerPiece0 = gameRules.player0.centerPiece;
        centerPiece0.level = 1;
        centerPiece0.modifier.GetModifierValues();
        centerPiece0.StartPiece();

        startPos0.piece = gameRules.player0.centerPiece;
        print(startPos0.location);
        startPos0.UpdateCell();
        //startPos0.TintAdjacentCells(centerPiece0.modifier.color);

        Piece centerPiece1 = gameRules.player1.centerPiece;
        centerPiece1.level = 1;
        centerPiece1.modifier.GetModifierValues();
        centerPiece1.StartPiece();

        startPos1.piece = gameRules.player1.centerPiece;
        print(startPos1.location);
        startPos1.UpdateCell();
    }

}
