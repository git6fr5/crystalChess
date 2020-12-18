using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int rows;
    public int columns;

    public GameObject[][] grid;

    // Start is called before the first frame update
    void Start()
    {
        CreateBoard();
    }

    void CreateBoard()
    {
        grid = new GameObject[rows][];
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = new GameObject[columns];
        }
    }
}
