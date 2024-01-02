using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private enum PickupType
    { 
        GoldCoin,
        StaminaGlobe,
        HealthGlobe
    }

    [SerializeField] private PickupType pickupType;
    [SerializeField] private float pickupDistance = 5f;
    [SerializeField] private float accelerationRate = 0.2f;
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;

    private Vector3 moveDir;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickupDistance)
        {
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelerationRate;
        }
        else
        {
            moveDir = Vector3.zero;
            moveSpeed = 0;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.GetComponent<PlayerController>())
        {
            DetectPickupType();
            Destroy(gameObject);
        }
    }

    private IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 startPoint = transform.position;
        float randomX = transform.position.x + Random.Range(-2f, 2f);
        float randomY = transform.position.y + Random.Range(-1f, 1f);

        Vector2 endPoint = new Vector2(randomX, randomY);
        float timePassed = 0f;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearTime = timePassed / popDuration;
            float heightTime = animCurve.Evaluate(linearTime);
            float height = Mathf.Lerp(0f, heightY, heightTime);

            transform.position = Vector2.Lerp(startPoint, endPoint, linearTime) + new Vector2(0f, height);
            yield return null;
        }
    }

    private void DetectPickupType()
    {
        switch (pickupType)
        {
            case PickupType.GoldCoin:
                EconomyManager.Instance.UpdateCurrentGold();
                break;

            case PickupType.StaminaGlobe:
                Stamina.Instance.RefreshStamina();
                break;

            case PickupType.HealthGlobe:
                PlayerHealth.Instance.HealPlayer();
                break;

            default:
                break;
        }
    }
}
