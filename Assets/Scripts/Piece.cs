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

    [HideInInspector] public int level = 1;

    /*--- Modifiers ---*/
    [HideInInspector] public float baseHealth = 1;
    [HideInInspector] public float maxHealth = 0;
    [HideInInspector] public float health = 0;

    /*--- UI ---*/
    [HideInInspector] public bool isAttached = false;
    [HideInInspector] public Vector3 initPos;

    public GameObject statusObject;
    private Status status;

    void Update()
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
}
