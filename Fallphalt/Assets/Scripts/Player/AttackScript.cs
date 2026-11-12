using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    private RaycastHit2D[] hits;
   // [SerializeField] private int playerHealth = 100;

    // Update is called once per frame
    void Update()
    {
        if (UserInput.instance.controls.Attack.Attack.WasPressedThisFrame())
        {
            //attack 
        }
    }

    private void Attack()
    { 
        hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            EnemyHealth enemyHealth = hits[i].collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {

            }
        }
    }
}
