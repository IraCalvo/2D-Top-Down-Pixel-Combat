using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private int damageAmount;

    private void Start()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        EnemyHealth enemyHealth = otherCollider.gameObject.GetComponent<EnemyHealth>();
        //the " ? " checks, null refrence check with it
        enemyHealth?.TakeDamage(damageAmount);
    }
}
