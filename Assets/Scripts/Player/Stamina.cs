using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }
    [SerializeField] private float timeBetweenStaminaRefresh = 2.5f;

    [SerializeField] private Sprite fullStaminaImage;
    [SerializeField] private Sprite emptyStaminaImage;

    private Transform staminaContainer;
    private int startingStamina = 3;
    private int maxStamina;
    const string STAMINA_CONTAINER_TEXT = "StaminaContainer";

    protected override void Awake()
    {
        base.Awake();

        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    private void Start()
    {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;
        RefreshStaminaUponDeath();
    }

    public void UseStamina()
    {
        CurrentStamina--;
        UpdateStaminaImages();
        StopAllCoroutines();
        StartCoroutine(RefreshStaminaRoutine());
    }

    public void RefreshStamina()
    {
        if(CurrentStamina < maxStamina || PlayerHealth.Instance.IsDead == false)
        {
            CurrentStamina++;
        }
        UpdateStaminaImages();
    }

    public void RefreshStaminaUponDeath()
    {
        CurrentStamina = startingStamina;
        UpdateStaminaImages();
    }

    private IEnumerator RefreshStaminaRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamina();
        }
    }

    private void UpdateStaminaImages()
    {
        for (int i = 0; i < maxStamina; i++)
        {
            Transform child = staminaContainer.GetChild(i);
            Image image = child?.GetComponent<Image>();

            if (i <= CurrentStamina - 1)
            {
                image.sprite = fullStaminaImage;
            }
            else 
            {
                image.sprite = emptyStaminaImage;
            }
        }
    }
}
