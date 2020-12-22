using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modifier : MonoBehaviour
{
    public Vector4 color;
    [HideInInspector] public delegate void EffectDelegate();
    private EffectDelegate effectDelegate;

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

    void OnEnabled()
    {
        if (name == "Burn")
        {
            effectDelegate = Burn;
        }

        else if (name == "Drown")
        {
            effectDelegate = Drown;
        }

        else if (name == "Fear")
        {
            effectDelegate = Fear;
        }

        else if (name == "Army")
        {
            effectDelegate = Army;
        }

        else { effectDelegate = Nullity; }
    }

    public void GetModifierValues()
    {
        burnDamage = casterPiece.level * burnDamageIncrement + baseBurnDamage;
        drownRate = casterPiece.level * drownRateIncrement + baseDrownRate;
        paralyzeDuration = casterPiece.level * paralyzeDurationIncrement + baseParalyzeDuration;
        armyHealth = casterPiece.level * armyHealthIncrement + baseArmyHealth;
    }

    public void Apply()
    {
        if (targetPiece)
        {
            effectDelegate();
        }
    }

    void Burn()
    {
        print("burning");
        targetPiece.health = targetPiece.health - burnDamage;
    }

    void Drown()
    {
        print("drowning");
        targetPiece.drownDuration = targetPiece.drownDuration + drownRate;
        if (targetPiece.drownDuration >= targetPiece.drownThreshold)
        {
            targetPiece.health = 0f;
        }
    }

    void Fear()
    {
        if (!targetPiece.isParalyzed)
        {
            print("fear");

            targetPiece.paralyzeDuration = paralyzeDuration;

            Status status = targetPiece.statusObject.GetComponent<Status>();
            GameObject fearBar = status.fearBar;
            Slider fearSlider = fearBar.GetComponent<Slider>();
            fearSlider.maxValue = paralyzeDuration;
            fearSlider.value = paralyzeDuration;

            targetPiece.isParalyzed = true;
        }
    }

    void Army()
    {
        print("army");

        Status status = targetPiece.statusObject.GetComponent<Status>();
        GameObject healthBar = status.healthBar;
        Slider healthSlider = healthBar.GetComponent<Slider>();

        if (targetPiece.health < healthSlider.maxValue)
        {
            targetPiece.health = targetPiece.health + armyHealth;
            if (targetPiece.health > healthSlider.maxValue)
            {
                targetPiece.health = healthSlider.maxValue;
            }
        }
    }

    void Nullity()
    {
        //
        print("nullity");
    }
}
