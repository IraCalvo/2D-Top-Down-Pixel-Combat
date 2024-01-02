using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool IsDead { get; private set; }

    [SerializeField] private int maxHealth;
    [SerializeField] private float damageRecoveryTime = 1f;
    private int currentHealth;
    private bool canTakeDamage = true;
    [SerializeField] private float knockBackThrustAmount = 10f;
    private Knockback knockback;
    private Flash flash;
    private Slider healthSlider;
    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string HUB_TEXT = "Hub World";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake()
    {
        base.Awake();
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
    }

    private void Start()
    {
        IsDead = false;
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D otherCollider)
    {
        EnemyAI enemy = otherCollider.gameObject.GetComponent<EnemyAI>();
        if (enemy)
        {
            TakeDamage(1, otherCollider.transform);
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (canTakeDamage == false)
        {
            return;
        }

        canTakeDamage = false;
        currentHealth -= damageAmount;

        ScreenShakeManager.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());

        UpdateHealthSlider();
        StartCoroutine(DamageRecoveryRoutine());
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    { 
        if(currentHealth <= 0 && PlayerController.Instance.CanMove == true)
        {
            PlayerController.Instance.CanMove = false;
            IsDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        Stamina.Instance.RefreshStaminaUponDeath();
        SceneManager.LoadScene(HUB_TEXT);
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
