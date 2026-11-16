using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    private RaycastHit2D[] hits;

    // Player fighting stats
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private int playerHealth = 100;
    [SerializeField] private int currentHealth;

    // Update is called once per frame
    void Update()
    {
        if (UserInput.instance.controls.Player.Attack.WasPressedThisFrame())
        {
            //attack 
        }
    }

    private void Attack()
    { 
        hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            IDamagable iDamagable = hits[i].collider.gameObject.GetComponent<IDamagable>();

            if (iDamagable != null)
            {
                //Do damage
                iDamagable.Damage(damageAmount); //Damage is tbd

            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    }
}
