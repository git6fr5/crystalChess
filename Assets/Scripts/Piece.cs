using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    /* --- The Player ---*/
    public Player player;
    public PieceAnimator animationScript;

<<<<<<< HEAD
    // the pieces properties
    public string faction;
    [HideInInspector] public int level = 1;
    public Sprite[] sprites;
    private int[] radiuses = { 1, 1, 1, 2, 2, 2, 3, 3, 4 };
    
    public float baseHealth = 1;
    [HideInInspector] public float health = 0;
    [HideInInspector] public float damageTaken = 0;
    [HideInInspector] public int radius = 0;

    // possible debuffs/buffs
    [HideInInspector] public float drownTicker;
    [HideInInspector] public float paralyzeTicker;
    [HideInInspector] public float armyCount;
    [HideInInspector] public float burnDamage;
    [HideInInspector] public bool paralyzed = false;
    [HideInInspector] public float drownThreshold = 0;

    void Awake()
=======
    /*--- Piece Properties ---*/
    public Modifier modifier; // the modifier that this piece casts
    [HideInInspector] public List<Modifier> modifiers = new List<Modifier>(); // the modifier that this piece is affected by

    [HideInInspector] public string faction;
    public Sprite[] sprites;

    [HideInInspector] public int level = 1;

    /*--- Modifiers ---*/
    [HideInInspector] public float baseHealth = 1;
    [HideInInspector] public float maxHealth = 0;
    [HideInInspector] public float health = 0;

    [HideInInspector] public float curseDamage = 0;

    /*--- UI ---*/
    [HideInInspector] public bool isAttached = false;
    [HideInInspector] public Vector3 initPos;

    public GameObject statusObject;
    private Status status;

    void Update()
>>>>>>> max_level_5(v2)
    {
        if (isAttached)
        {
            Vector3 cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(cameraPos.x, cameraPos.y, initPos.z + 1);
        }
    }

    public void StartPiece()
    {
        gameObject.SetActive(true);
        faction = tag;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[level - 1];

        print(level);
        if (animationScript) { animationScript.SetAnimation(level); }

        maxHealth = baseHealth * level;
        health = maxHealth;

        status = statusObject.GetComponent<Status>();
        status.healthBar.SetActive(true);

    }

    public void UpdatePiece()
    {
        GameObject healthBar = status.healthBar;
        Slider healthSlider = healthBar.GetComponent<Slider>();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        if (health <= 0)
        {
            player.gameRules.Play(player.gameRules.controlPanelAnimator.deathAnim, transform.position);
            player.gameRules.PlayAudio(player.gameRules.controlPanelAnimator.deathAudio);
            player.pauseAction = true;
            StartCoroutine(IEDeath(8f/6f));
        }

        initPos = transform.position;
    }

    private IEnumerator IEDeath(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
        player.gameRules.Play(player.gameRules.controlPanelAnimator.idleAnim, Vector3.zero);

        player.pauseAction = false;

        yield return null;
    }

        public void DisplayStatus()
    {
        Status status = statusObject.GetComponent<Status>();

        GameObject healthBar = status.healthBar;
        Slider healthSlider = healthBar.GetComponent<Slider>();
        healthSlider.value = health;
    }

    public void Attach(bool attach)
    {
        if (attach) { initPos = transform.position; }
        else { transform.position = initPos; }
        isAttached = attach;
    }

    public void Afflict()
    {
<<<<<<< HEAD
        //string s01 = "Name: " + name;
        string s02 = "\nFaction: " + faction;
        string s03 = "\nLevel: " + level.ToString();
        string s04 = "\nHealth: " + health.ToString();
        string s05; string s06; string s07; string s08;
        if (burnDamage != 0) { s05 = "\nBurning for: " + burnDamage.ToString(); } else { s05 = "\nNot burning"; }
        if (armyCount != 0) { s06 = "\nArmy bonus in vicinity: " + armyCount.ToString(); } else { s06 = "\nNo army bonus in vicinity"; }
        if (drownTicker != 0) { s07 = "\nDrowning for: " + drownTicker.ToString() + " / Threshold: " + drownThreshold.ToString(); } else { s07 = "\nNot drowning"; }
        if (paralyzed) { s08 = "\nParalyze time left: " + paralyzeTicker.ToString(); } else { s08 = "\nNot paralyzed"; }
        return s02 + s03 + s04 + s05 + s06 + s07 + s08;
=======
        health = health - curseDamage;
        UpdatePiece();
>>>>>>> max_level_5(v2)
    }
}
