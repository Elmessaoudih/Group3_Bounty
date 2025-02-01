using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    // The amount of HP the Health Potion gives
    [SerializeField] private int hpGained = 10;

    // If the Player touches the Health Potion, give them hpGained amount of health and remove the Health Potion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().GainHp(hpGained);

            Destroy(gameObject);
        }
    }
}
