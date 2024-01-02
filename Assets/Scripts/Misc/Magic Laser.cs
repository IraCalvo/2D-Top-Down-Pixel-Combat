using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 2f;
    private float laserRange;
    private SpriteRenderer sr;
    private CapsuleCollider2D laserCollider;
    private bool isGrowing = true;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        laserCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }

    public void UpdateLaserRange(float laserRange)
    {
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    private void LaserFaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = transform.position - mousePosition;
        transform.right = -direction;
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;

        while (sr.size.x < laserRange && isGrowing == true)
        {
            timePassed += Time.deltaTime;
            float linearTime = timePassed / laserGrowTime;

            sr.size = new Vector2(Mathf.Lerp(1f, laserRange, linearTime), 1f);
            laserCollider.size = new Vector2(Mathf.Lerp(1f, laserRange, linearTime), laserCollider.size.y);
            laserCollider.offset = new Vector2(Mathf.Lerp(1f, laserRange, linearTime) / 2, laserCollider.offset.y);

            yield return null;
        }
        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.GetComponent<Indestructable>() && otherCollider.isTrigger == false)
        {
            isGrowing = false;
        }
    }

}
