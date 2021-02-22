using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inspector : MonoBehaviour
{
    public Image image;
    public Text nameText;
    public Text levelText;
    public Text locationText;
    public Text healthText;
    public Text drownText;
    public Text fearText;

    public Sprite defaultSprite;
    public GameObject healthBar;

    [HideInInspector] public GameObject currentObject;
}
