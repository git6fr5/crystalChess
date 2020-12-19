using System.Collections;
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
    }

    void CreateBoard()
    {
        gridArray = new GameObject[rows][];
        for (int i = 0; i < gridArray.Length; i++)
        {
            gridArray[i] = new GameObject[columns];
            for (int j = 0; j<gridArray[i].Length; j++)
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
        //
    }

}
