using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButton : MonoBehaviour
{

    public GameObject playerObject;
    private Player player;

    void Start()
    {
        player = playerObject.GetComponent<Player>();
    }

    public void OnClick()
    {
        if (name == "Combine")
        {
            print("clicked combine button");
            player.gameRules.OnCombineEvent.Invoke();
        }
    }
}
