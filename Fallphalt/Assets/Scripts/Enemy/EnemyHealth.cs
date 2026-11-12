using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float enemyHealth = 100;

    private float currentHealth;

    void Start()
    {
        currentHealth = enemyHealth;
    }
}
