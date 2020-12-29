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
            if (player.selectionList.Count == 1 && player.selectionList[0].GetComponent<Cell>())
            {
                Deselect(0);
                player.Highlight();
            }
            else if (player.selectionList.Count == 2 && (player.selectionList[0].GetComponent<Cell>() || player.selectionList[1].GetComponent<Cell>()))
            {
                Deselect(1); Deselect(0);
                player.Highlight();
            }
            if (player.selectionList.Count == 1 && player.selectionList[0].GetComponent<Card>() && player.selectionList[0].GetComponent<Card>().faction != faction)
            {
                Deselect(0);
                player.Highlight();
            }

            player.selectionList.Add(gameObject);
            player.Highlight();
        }

        player.InspectCard(this);
    }

    public void Deselect(int index)
    {
        if (player.selectionList[index].GetComponent<Cell>())
        {
            player.selectionList[index].GetComponent<Cell>().highlight.SetActive(false);
        }
        else if (player.selectionList[index].GetComponent<Card>())
        {
            player.selectionList[index].GetComponent<Card>().highlight.SetActive(false);
        }
        player.selectionList.RemoveAt(index);
    }
}
