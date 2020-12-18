using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class TurnSystem : MonoBehaviour
{
    public Player player;

    [HideInInspector] public bool isTurn = false;

    public UnityEvent OnTurnEvent;
    public UnityEvent OnDrawEvent;
    public UnityEvent OnCombineEvent;

    public void OnTurn()
    {
        OnDrawEvent.Invoke();
    }

    public void OnDraw()
    {
        for (int i = 0; i < player.drawRegular; i++)
        {
            player.handList.Add(player.deckList[i]);
        }
        player.deckList.RemoveRange(0, player.drawRegular);
        player.DisplayHand();
    }
}
