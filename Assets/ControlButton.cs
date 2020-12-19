using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButton : MonoBehaviour
{
    public int drawNum = 5;
    public Board board;

    public void OnClick()
    {
        if (name == "Draw")
        {
            print("clicked draw button");
            board.gameRules.OnDrawEvent.Invoke(drawNum);
        }

        if (name == "Combine")
        {
            print("clicked combine button");
            board.gameRules.OnCombineEvent.Invoke();
        }

        if (name == "Place")
        {
            print("clicked place button");
            board.gameRules.OnPlaceEvent.Invoke();
        }

        if (name == "Move")
        {
            print("clicked move button");
            board.gameRules.OnMoveEvent.Invoke();
        }

        if (name == "Aura")
        {
            print("clicked aura button");
            board.gameRules.OnAuraEvent.Invoke();
        }
    }
}
