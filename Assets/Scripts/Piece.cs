using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    /* --- The Player ---*/
    public GameObject playerObject;

    /*--- Piece Properties ---*/
    public Sprite[] sprites;
    private int[] radiuses = { 1, 1, 1, 2, 2, 2, 3, 3, 4 };

    [HideInInspector] public int level = 1;
    [HideInInspector] public string faction;
    [HideInInspector] public float baseHealth = 1;
    [HideInInspector] public float health = 0;
    [HideInInspector] public float damageTaken = 0;
    [HideInInspector] public int radius = 0;

    /*--- Modifiers ---*/
    [HideInInspector] public float drownTicker = 0f;
    [HideInInspector] public float paralyzeTicker = 0f;
    [HideInInspector] public float armyCount = 0f;
    [HideInInspector] public float burnDamage = 0f;
    [HideInInspector] public bool paralyzed = false;
    [HideInInspector] public float drownThreshold = 0;

    void Start()
    {
        GetPlayer();
        GetFaction();
    }

    public void UpdateSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[level - 1];
        radius = radiuses[level - 1];
        health = baseHealth * radius - damageTaken;
    }

    public string ReadProperties()
    {
        string s01 = "Name: " + name;
        string s02 = "\nFaction: " + faction;
        string s03 = "\nLevel: " + level.ToString();
        string s04 = "\nHealth: " + health.ToString();
        string s05; string s06; string s07; string s08;
        if (burnDamage != 0) { s05 = "\nBurning for: " + burnDamage.ToString(); } else { s05 = "\nNot burning"; }
        if (armyCount != 0) { s06 = "\nArmy bonus in vicinity: " + armyCount.ToString(); } else { s06 = "\nNo army bonus in vicinity"; }
        if (drownTicker != 0) { s07 = "\nDrowning for: " + drownTicker.ToString() + " / Threshold: " + drownThreshold.ToString(); } else { s07 = "\nNot drowning"; }
        if (paralyzed) { s08 = "\nParalyze time left: " + paralyzeTicker.ToString(); } else { s08 = "\nNot paralyzed"; }
        return s01 + s02 + s03 + s04 + s05 + s06 + s07 + s08;
    }

    private void GetPlayer()
    {
        playerObject = gameObject.transform.parent.gameObject;
    }

    private void GetFaction()
    {
        faction = gameObject.tag;
    }
}
