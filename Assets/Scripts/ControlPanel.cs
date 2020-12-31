using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    private int drawNum = 2;
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

        if (name == "Attack")
        {
            print("clicked attack button");
            board.gameRules.OnAttackEvent.Invoke();
        }

        if (name == "End")
        {
            print("clicked aura button");
            board.gameRules.OnEndEvent.Invoke();
        }
    }
}
