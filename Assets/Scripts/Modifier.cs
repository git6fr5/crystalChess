using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modifier : MonoBehaviour
{
    public Color color;
    public Piece casterPiece;

    [HideInInspector] public Piece targetPiece;

    [HideInInspector] private float burnDamage = 0f;
    [HideInInspector] private float burnDamageIncrement = 0.2f;
    [HideInInspector] private float baseBurnDamage = 0.5f;

    [HideInInspector] private float drownRate = 0f;
    [HideInInspector] private float drownRateIncrement = 0.5f;
    [HideInInspector] private float baseDrownRate = 0.5f;

    [HideInInspector] private float paralyzeDuration = 0f;
    [HideInInspector] private float paralyzeDurationIncrement = 0.5f;
    [HideInInspector] private float baseParalyzeDuration = 2f;

    [HideInInspector] private float armyHealth = 0f;
    [HideInInspector] private float armyHealthIncrement = 0.2f;
    [HideInInspector] private float baseArmyHealth = 1f;

    public bool isBuff;

    public void GetModifierValues()
    {
        burnDamage = casterPiece.level * burnDamageIncrement + baseBurnDamage;
        drownRate = casterPiece.level * drownRateIncrement + baseDrownRate;
        paralyzeDuration = casterPiece.level * paralyzeDurationIncrement + baseParalyzeDuration;
        armyHealth = casterPiece.level * armyHealthIncrement + baseArmyHealth;
    }

    public void Apply(Piece targetPiece)
    {
        if (name == "Burn")
        {
            Burn(targetPiece);
        }
        else if (name == "Drown")
        {
            Drown(targetPiece);
        }

        else if (name == "Fear")
        {
            Fear(targetPiece);
        }

        else if (name == "Army")
        {
            Army(targetPiece);
        }

        targetPiece.UpdatePiece();
    }

    void Burn(Piece targetPiece)
    {
        print("burning");
        targetPiece.health = targetPiece.health - burnDamage;
    }

    void Drown(Piece targetPiece)
    {
        print("drowning");
        /*
        targetPiece.drownDuration = targetPiece.drownDuration + drownRate;
        if (targetPiece.drownDuration >= targetPiece.drownThreshold)
        {
            targetPiece.health = 0f;
        }*/
    }

    void Fear(Piece targetPiece)
    {
        print("fear");
        /*if (!targetPiece.isParalyzed)
        { 

            targetPiece.paralyzeDuration = paralyzeDuration;

            Status status = targetPiece.statusObject.GetComponent<Status>();
            GameObject fearBar = status.fearBar;
            Slider fearSlider = fearBar.GetComponent<Slider>();
            fearSlider.maxValue = paralyzeDuration;
            fearSlider.value = paralyzeDuration;

            targetPiece.isParalyzed = true;
        }*/
    }

    void Army(Piece targetPiece)
    {
        print("army");
        targetPiece.health = targetPiece.health + armyHealth;

        if (targetPiece.health < targetPiece.maxHealth)
        {
            targetPiece.health = targetPiece.maxHealth;
        }
    }

    void Nullity(Piece targetPiece)
    {
        print("nullity");
    }
}
