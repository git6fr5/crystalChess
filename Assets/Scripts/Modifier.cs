using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier : MonoBehaviour
{
    public Vector4 color;
    public delegate void EffectDelegate();
    private EffectDelegate effectDelegate;

    public Piece targetPiece;

    void Start()
    {
        if (name == "Burn")
        {
            effectDelegate = Burn;
        }

        else { effectDelegate = Nullity; }
    }

    public void Apply()
    {
        if (targetPiece)
        {
            effectDelegate();
        }
    }

    void Burn()
    {
        print("burning");
        targetPiece.health = targetPiece.health - 0.5f;
    }

    void Nullity()
    {
        //
        print("nullity");
    }
}
