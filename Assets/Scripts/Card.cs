using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    /* --- Associated Piece ---*/
    public Piece piece;

    /* --- The Player ---*/
    public Player player;

    public GameObject highlight;

    /*--- Card Properties ---*/
    [HideInInspector] public string faction;
    public Sprite[] sprites;
    public int level = 1;

    [HideInInspector] public bool isFirstSelected = false;
    [HideInInspector] public Vector3 initPos;

    void Start()
    {
        faction = tag;
        UpdateCard();
    }

    void Update()
    {
        if (isFirstSelected)
        {
            Vector3 cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(cameraPos.x, cameraPos.y, initPos.z + 5);
        }
    }

    void OnMouseDown()
    {
        SelectFlag();
    }

    void OnMouseOver()
    {
        Highlight();
    }

    void OnMouseExit()
    {
        UnHighlight();
    }

    public string ReadProperties()
    {
        string s01 = "Name: " + name;
        string s02 = "\nFaction: " + faction;
        string s03 = "\nLevel: " + level.ToString();
        return s01 + s02 + s03;
    }

    private void GetFaction()
    {
        faction = tag;
    }

    public void UpdateCard()
    {
        gameObject.SetActive(true);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[level - 1];
    }

    void SelectFlag()
    {
        if (player.isTurn)
        {
            // if selection list is empty
            if (player.selectionList.Count == 0)
            {
                // add to selection list, make it follow mouse
                Select(true, 0);
            }
            // if there is something in selection list
            else if (player.selectionList.Count == 1)
            {
                // if it is this thing, then deselect
                if (player.selectionList[0] == gameObject)
                {
                    Select(false, 0);
                }
                // if it is this thing, then deselect
                if (player.selectionList[0].GetComponent<Cell>())
                {
                    player.selectionList[0].GetComponent<Cell>().Select(false, 0);
                    Select(true, 0);
                }
                // check if that thing is a card
                else if (player.selectionList[0].GetComponent<Card>())
                {
                    Select(true, 1);
                    player.gameRules.OnCombineEvent.Invoke();
                }
            }
        }
    }

    public void Select(bool selecting, int index)
    {

        if (index == 0)
        {
            if (selecting) 
            {
                //GetComponent<BoxCollider2D>().enabled = false;
                player.selectionList.Add(gameObject);
                initPos = transform.position; 
            }
            else 
            {
                //GetComponent<BoxCollider2D>().enabled = true;
                player.selectionList.RemoveAt(index);
                transform.position = initPos; 

            }
            isFirstSelected = selecting;
        }
        else if (index == 1)
        {
            if (selecting)
            {
                player.selectionList.Add(gameObject);
                player.gameRules.OnCombineEvent.Invoke();
            }
            isFirstSelected = false;
        }
        else if (index == -1)
        {
            if (!selecting)
            {
                //GetComponent<BoxCollider2D>().enabled = true;
                transform.position = initPos;
            }
            isFirstSelected = false;
        }
    }

    void Highlight()
    {
        highlight.SetActive(true);
    }

    void UnHighlight()
    {
        highlight.SetActive(false);
    }
}
