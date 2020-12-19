using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier : MonoBehaviour
{
    public Vector4 color;
    public delegate void EffectDelegate();
    private EffectDelegate effectDelegate;

    void Start()
    {
        if (name == "Burn")
        {
            effectDelegate = Burn;
        }
    }

    public void Apply()
    {
        //
    }

    void Burn()
    {
        //
    }
}
