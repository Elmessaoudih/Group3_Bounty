using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
    // Damage done by Acid per damageInterval
    [SerializeField] int dmg;
    // Time Acid lingers in the Scene in seconds
    [SerializeField] float lifetime;
    // Time between damage applications, in seconds
    [SerializeField] float damageInterval = 0.5f; 

    // A dictionary of GameObjects and the time (since the start of the game) when the GameObject last took damage from this Acid
    private Dictionary<GameObject, float> damageTimers = new Dictionary<GameObject, float>();

    private void Start()
    {
        // Destroys this Acid object when its lifetime expires
        Destroy(this.gameObject, lifetime);
    }

    // If Player stays within Acid and CanDealDamage is true for the Player, then deal damage and update their damageTimer
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (CanDealDamage(collision.gameObject))
            {
                collision.GetComponent<PlayerController>().TakeDamage(dmg);
                UpdateDamageTimer(collision.gameObject);
            }
        }
    }

    // If the target just entered the Acid or the damageInterval has elapsed, then return true,
    // if not, return false
    private bool CanDealDamage(GameObject target)
    {
        // No timer exists, so one will be made when damageTimer is updated, so return true
        if (!damageTimers.ContainsKey(target))
        {
            return true; // If no timer exists for this object, we can deal damage
        }

        // Check if damageInterval or more time has passed since the last application of damage (damageTimer[target]) to the target,
        // if so return true, false otherwise
        return Time.time - damageTimers[target] >= damageInterval;
    }

    // If the target has an entry in damageTimer, then set the time of their latest application of damage to right now,
    // if not, then create and initialize a damageTimer for this target
    private void UpdateDamageTimer(GameObject target)
    {
        if (damageTimers.ContainsKey(target))
        {
            damageTimers[target] = Time.time;
        }
        else
        {
            damageTimers.Add(target, Time.time);
        }
    }

    // When Player leaves the Acid, remove their damageTimer (ensures they are always damaged at least once upon entering/re-entring Acid)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Remove the player from the timer tracking when they leave the acid area
            if (damageTimers.ContainsKey(collision.gameObject))
            {
                damageTimers.Remove(collision.gameObject);
            }
        }
    }
}
