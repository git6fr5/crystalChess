using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public GameObject pieceObject;

    private Piece piece;
    public GameObject health;

    [Range(0f, 1f)] public float healthPercent = 1f;

    void Start()
    {
        piece = pieceObject.GetComponent<Piece>();
        SetHealth();
    }

    void SetHealth()
    {
        healthPercent = piece.health / piece.baseHealth;
        health.transform.localScale = new Vector3(healthPercent, 1, 1);
    }
}
