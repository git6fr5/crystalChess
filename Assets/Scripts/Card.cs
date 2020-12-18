using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    // the piece it can convert to
    public GameObject pieceObject;

    // the player who owns the card
    public GameObject playerObject;

    // the cards properties
    public string faction;
    public Sprite[] sprites;
    public int level = 1;

    void Awake()
    {
        playerObject = gameObject.transform.parent.gameObject;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[level - 1];
    }

    public void OnMouseDown()
    {
        Debug.Log("In Card, OnMouseDown; Selected a card");
        Player player = playerObject.GetComponent<Player>();
        if (player.isTurn)
        {
            if (player.phase == 1 && player.selections.Count < 2)
            {
                player.selections.Add(gameObject);
            }
            else if (player.phase == 2 && player.selections.Count < 1)
            {
                Debug.Log("During place phase, added card to selections");
                player.selections.Add(gameObject);
            }
        }
        player.infoField.GetComponent<Text>().text = Properties();
        // false for not clearing selections
        player.Highlight(gameObject.transform.GetChild(0).gameObject, false);
    }

    public string Properties()
    {
        string s01 = "\nFaction: " + faction;
        string s02 = "\nLevel: " + level.ToString();
        return s01 + s02;
    }

}
