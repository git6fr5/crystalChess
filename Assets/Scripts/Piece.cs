using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    /* --- The Player ---*/
    public Player player;
    public PieceAnimator animationScript;

    /*--- Piece Properties ---*/
    public Modifier modifier; // the modifier that this piece casts
    [HideInInspector] public List<Modifier> modifiers = new List<Modifier>(); // the modifier that this piece is affected by

    [HideInInspector] public string faction;
    public Sprite[] sprites;

    private int[] radiuses = { 1, 1, 1, 2, 2, 2, 3, 3, 4 };
    [HideInInspector] public int radius = 0;

    [HideInInspector] public int level = 1;

    /*--- Modifiers ---*/
    [HideInInspector] public float baseHealth = 1;
    [HideInInspector] public float health = 0;

    [HideInInspector] public float drownDuration = 0f;
    [HideInInspector] public bool isDrowning = false;
    [HideInInspector] public float baseThreshold = 2f;
    [HideInInspector] public float drownThreshold = 0f;

    [HideInInspector] public float paralyzeDuration = 0f;
    [HideInInspector] public bool isParalyzed = false;
    [HideInInspector] public float baseRecovery = 1f;
    [HideInInspector] public float paralyzeRecovery = 0f;


    /*--- UI ---*/

    public GameObject statusObject;

    void Start()
    {
        faction = tag;
    }

    public string ReadProperties()
    {
        string s01 = "Name: " + name;
        string s02 = "\nFaction: " + faction;
        string s03 = "\nLevel: " + level.ToString();
        string s04 = "\nHealth: " + health.ToString();
        //string s05; string s06; string s07; string s08;
        /*if (burnDamage != 0) { s05 = "\nBurning for: " + burnDamage.ToString(); } else { s05 = "\nNot burning"; }
        if (armyCount != 0) { s06 = "\nArmy bonus in vicinity: " + armyCount.ToString(); } else { s06 = "\nNo army bonus in vicinity"; }
        if (drownTicker != 0) { s07 = "\nDrowning for: " + drownTicker.ToString() + " / Threshold: " + drownThreshold.ToString(); } else { s07 = "\nNot drowning"; }
        if (paralyzed) { s08 = "\nParalyze time left: " + paralyzeTicker.ToString(); } else { s08 = "\nNot paralyzed"; }*/
        return s01;
    }

    public void UpdatePiece()
    {
        gameObject.SetActive(true);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[level - 1];
        if (animationScript)
        {
            animationScript.SetAnimation(level - 1);
        }

        radius = radiuses[level - 1];

        health = baseHealth * level;
        drownThreshold = baseThreshold * level;
        paralyzeRecovery = baseRecovery * level;

        Status status = statusObject.GetComponent<Status>();

        GameObject healthBar = status.healthBar;
        Slider healthSlider = healthBar.GetComponent<Slider>();
        healthSlider.maxValue = health;

        GameObject drownBar = status.drownBar;
        Slider drownSlider = drownBar.GetComponent<Slider>();
        drownSlider.value = drownThreshold;
    }

    public void DisplayStatus()
    {

        Status status = statusObject.GetComponent<Status>();

        GameObject healthBar = status.healthBar;
        Slider healthSlider = healthBar.GetComponent<Slider>();
        healthSlider.value = health;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        status.burnObject.SetActive(false);
        status.drownObject.SetActive(false); isDrowning = false;
        status.fearObject.SetActive(false);
        status.armyObject.SetActive(false);

        foreach (Modifier effectModifier in modifiers)
        {
            if (effectModifier.name == "Burn") { status.burnObject.SetActive(true); }
            if (effectModifier.name == "Drown") { status.drownObject.SetActive(true); isDrowning = true; }
            if (effectModifier.name == "Fear") { status.fearObject.SetActive(true); }
            if (effectModifier.name == "Army") { status.armyObject.SetActive(true); }
        }

        if (isDrowning)
        {
            GameObject drownBar = status.drownBar;
            Slider drownSlider = drownBar.GetComponent<Slider>();
            drownSlider.value = drownDuration;
        }

        if (isParalyzed)
        {
            GameObject fearBar = status.fearBar;
            Slider fearSlider = fearBar.GetComponent<Slider>();
            fearSlider.value = paralyzeDuration;
        }
    }
}
