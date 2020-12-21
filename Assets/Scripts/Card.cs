using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    /* --- Associated Piece ---*/
    public GameObject pieceObject;

    /* --- The Player ---*/
    public GameObject playerObject;
    [HideInInspector] public Player player;

    public GameObject highlight;

    /*--- Card Properties ---*/
    [HideInInspector] public string faction;
    public Sprite[] sprites;
    public int level = 1;

    void Start()
    {
        GetPlayer();
        GetFaction();
        UpdateCard();
    }

    public string ReadProperties()
    {
        string s01 = "Name: " + name;
        string s02 = "\nFaction: " + faction;
        string s03 = "\nLevel: " + level.ToString();
        return s01 + s02 + s03;
    }

    private void GetPlayer()
    {
        player = playerObject.GetComponent<Player>();
    }

    private void GetFaction()
    {
        faction = gameObject.tag;
    }

    public void UpdateCard()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[level - 1];
    }

    void OnMouseDown()
    {
        Select();
    }

    void Select()
    {
        print("selected a card");
        if (player.isTurn)
        {
            player.selectionList.Add(gameObject);
            player.Highlight();
        }

        player.InspectCard(this);
    }
}
