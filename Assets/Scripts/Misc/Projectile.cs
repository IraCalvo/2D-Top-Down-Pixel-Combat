using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;

    [SerializeField] private float projectileRange = 10f;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void MoveProjectile()
    {
        //Vector3.right cause the sprite of the arrow is already facing towards teh right
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }


    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        EnemyHealth enemyHealth = otherCollider.gameObject.GetComponent<EnemyHealth>();
        Indestructable indestructable = otherCollider.gameObject.GetComponent<Indestructable>();
        PlayerHealth player = otherCollider.gameObject.GetComponent<PlayerHealth>();

        if (otherCollider.isTrigger == false && (enemyHealth || indestructable || player))
        {
            if ((player && isEnemyProjectile) || (enemyHealth && isEnemyProjectile == false))
            {
                player?.TakeDamage(1, transform);
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else if (otherCollider.isTrigger == false && indestructable)
            {
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

}
