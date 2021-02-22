using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    void Apply(Piece piece) 
    {
        piece.health = piece.health - 1;
    }
}
