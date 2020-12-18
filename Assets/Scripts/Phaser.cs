using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phaser : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject playerObject;

    void Awake()
    {
        playerObject = gameObject.transform.parent.gameObject;
    }

    void Update()
    {
        Player player = playerObject.GetComponent<Player>();
        Sign(player.phase);
    }

    public void OnMouseDown()
    {
        Player player = playerObject.GetComponent<Player>();
        if (player.isTurn)
        {
            player.SkipPhase();
        }
    }

    public void Sign(int phase)
    {

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Player player = playerObject.GetComponent<Player>();
        if (!player.isTurn)
        {
            spriteRenderer.sprite = null;
        }
        else
        {
            spriteRenderer.sprite = sprites[player.phase];
        }
    }
}
