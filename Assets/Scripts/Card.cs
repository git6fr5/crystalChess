﻿using UnityEngine;
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

    void Update()
    {
        if (isAttached)
        {
            Vector3 cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(cameraPos.x, cameraPos.y, initPos.z + 1);
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

    public void StartCard()
    {
        gameObject.SetActive(true);
        faction = tag;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[level - 1];
        initPos = transform.position;
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
        player.InspectCard(this);
    }

    public void Attach(bool attach)
    {
<<<<<<< HEAD
        string s01 = "\nFaction: " + faction;
        string s02 = "\nLevel: " + level.ToString();
        return s01 + s02;
=======
        if (attach) { initPos = transform.position; }
        else { transform.position = initPos; }
        isAttached = attach;
>>>>>>> max_level_5(v2)
    }

    void Highlight(bool highlight)
    {
        if (highlight) { player.InspectCard(this); }
        else { player.gameRules.ClearInspector(); }
        highlightObject.SetActive(highlight);
    }

    public static bool IsCard(GameObject _object)
    {
        if (_object.GetComponent<Card>()) { return true; }
        else { return false; }
    }
}
