using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.GetComponent<DamageSource>() || otherCollider.gameObject.GetComponent<Projectile>())
        {
            PickupSpawner pickupSpawner = GetComponent<PickupSpawner>();
            pickupSpawner?.DropItems();
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
