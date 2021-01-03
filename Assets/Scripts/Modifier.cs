using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modifier : MonoBehaviour
{
    public Color color;
    public GameObject projectile;

    public Piece casterPiece;

    [HideInInspector] public Piece targetPiece;

    [HideInInspector] private float burnDamage = 0f;
    [HideInInspector] private float burnDamageIncrement = 0.5f;
    [HideInInspector] private float baseBurnDamage = 0f;

    [HideInInspector] private float drownRate = 0f;
    [HideInInspector] private float drownRateIncrement = 0.5f;
    [HideInInspector] private float baseDrownRate = 0.5f;

    [HideInInspector] private float curseDamage = 0f;
    [HideInInspector] private float curseDamageIncrement = 0.2f;
    [HideInInspector] private float baseCurseDamage = 0f;

    [HideInInspector] private float armyHealth = 0f;
    [HideInInspector] private float armyHealthIncrement = 0.3f;
    [HideInInspector] private float baseArmyHealth = 0f;

    public bool isBuff;

    //private float travelDuration = 1f;

    public void GetModifierValues()
    {
        burnDamage = casterPiece.level * burnDamageIncrement + baseBurnDamage;
        drownRate = casterPiece.level * drownRateIncrement + baseDrownRate;
        curseDamage = casterPiece.level * curseDamageIncrement + baseCurseDamage;
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
        //casterPiece.UpdatePiece();
    }

    void Burn(Piece targetPiece)
    {
        print("burning");

        /*projectile.SetActive(true);
        transform.position = casterPiece.transform.position;
        projectile.transform.SetParent(null);
        Vector3 direction = new Vector3(-casterPiece.transform.position.x + targetPiece.transform.position.x, -casterPiece.transform.position.y + targetPiece.transform.position.y, 0);
        projectile.GetComponent<Rigidbody2D>().velocity = direction;

        StartCoroutine(IEBurn(targetPiece));
        casterPiece.player.pauseAction = true;*/

        targetPiece.health = targetPiece.health - burnDamage;

    }

    /*IEnumerator IEBurn(Piece targetPiece)
    {
        yield return new WaitForSeconds(travelDuration);

        targetPiece.health = targetPiece.health - burnDamage;
        casterPiece.player.pauseAction = false;
        targetPiece.UpdatePiece();

        yield return null;
    }*/

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
        // note that this functionality means that there can only be one applied curse at a time
        targetPiece.curseDamage = curseDamage;
        casterPiece.health = casterPiece.health - curseDamage;
    }

    void Army(Piece targetPiece)
    {
        print("army");
        targetPiece.health = targetPiece.health + armyHealth;

        if (targetPiece.health >= targetPiece.maxHealth)
        {
            targetPiece.health = targetPiece.maxHealth;
        }
    }

    void Nullity(Piece targetPiece)
    {
        print("nullity");
    }
}
