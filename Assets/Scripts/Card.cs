using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    /* --- Associated Piece ---*/
    public Piece piece;

    /* --- The Player ---*/
    public Player player;

    public GameObject highlightObject;

    /*--- Card Properties ---*/
    [HideInInspector] public string faction;
    public Sprite[] sprites;
    public int level = 1;

    [HideInInspector] public bool isAttached = false;
    [HideInInspector] public Vector3 initPos;

    void Start()
    {
        faction = tag;
        UpdateCard();
    }

    void Update()
    {
        if (isAttached)
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
        Highlight(true);
    }

    void OnMouseExit()
    {
        Highlight(false);
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
                Select(true);
                Attach(true);
            }
            // if there is something in selection list
            else if (player.selectionList.Count == 1)
            {
                GameObject firstSelection = player.selectionList[0];
                // if it is this thing, then deselect
                if (firstSelection == gameObject)
                {
                    Select(false);
                    Attach(false);
                    return;
                }

                Select(true);
                // if it is this thing, then deselect
                if (IsCard(firstSelection))
                {
                    player.gameRules.OnCombineEvent.Invoke();
                }
                else
                {
                    player.ResetSelections();
                    Select(true);
                    Attach(true);
                }
            }
        }
    }

    public void Select(bool select)
    {
        if (select) { player.selectionList.Add(gameObject); }
        else { player.selectionList.Remove(gameObject); }       
    }

    public void Attach(bool attach)
    {
        if (attach) { initPos = transform.position; }
        else { transform.position = initPos; }
        isAttached = attach;
    }

    void Highlight(bool highlight)
    {
        highlightObject.SetActive(highlight);
    }

    bool IsCard(GameObject _object)
    {
        if (_object.GetComponent<Card>()) { return true; }
        else { return false; }
    }
}
