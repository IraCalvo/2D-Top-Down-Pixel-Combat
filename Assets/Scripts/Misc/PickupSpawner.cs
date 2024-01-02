using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoinPrefab;
    [SerializeField] private GameObject healthPickupPrefab;
    [SerializeField] private GameObject staminaPickupPrefab;

    public void DropItems()
    {
        int randomNum = Random.Range(1, 5);

        if (randomNum == 1)
        {
            Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
        }
        if (randomNum == 2)
        {
            Instantiate(staminaPickupPrefab, transform.position, Quaternion.identity);
        }
        if (randomNum == 3)
        {
            int randomAmountOfGoldCoins = Random.Range(1, 4);

            for (int i = 0; i < randomAmountOfGoldCoins; i++)
            {
                Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
