using UnityEngine;

public class NearestEnemyAttackWeapon : MonoBehaviour
{
    // This class holds the method that finds the nearest Enemy, derive a class from this one when said class spawns weapons that need to
    // target the closest Enemy

    // Returns the GameObject of the nearest object with the "Enemy" tag

    protected GameObject FindNearestEnemy()
    {
        // Initialize array of all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        // Sequentially search all enemies for the closest
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        // Return nearestEnemy
        return nearestEnemy;
    }
}
