using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Reference to the Scriptable Object for this Weapon
    [SerializeField] public WeaponScriptableObject weaponSO;
    // An array of the different upgrade tiers for this Weapon, having the Weapon is at index 0,
    // and the other tiers are this Weapon's numeral upgrades in ascending order
    [SerializeField] GameObject[] weaponTiers;

    // The current upgrade tier of this Weapon
    private GameObject activeTier;
    // The current level/tier this weapon is
    [HideInInspector] public int currentLevel = -1;
    // The largest level/tier this weapon can be
    [HideInInspector] public int maxLevel = -1;

    private void Start()
    {
        // Initialize currentLevel, maxLevel, and activeTier
        currentLevel = 0;
        maxLevel = weaponTiers.Length - 1;
        UpdateActiveTier();
    }

    // Increment currentLevel and activeTier, ensuring they do not surpass maxLevel
    public void LevelWeapon()
    {
        Debug.Log("weapon leveled up!");
        currentLevel += 1;
        Mathf.Clamp(currentLevel, -1, maxLevel);
        UpdateActiveTier();
    }

    // Deactivate current weaponTier (if it exists), then set activeTier to the weaponTier at this Weapon's currentLevel
    private void UpdateActiveTier()
    {
        if (activeTier != null)
            activeTier.SetActive(false);

        activeTier = weaponTiers[currentLevel];
        activeTier.SetActive(true);
    }
}
