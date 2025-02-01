using UnityEngine;
using TMPro;

public class UpgradeSlot : MonoBehaviour
{
    // Sound played when selecting this UpgradeSlot after leveling up
    [SerializeField] public AudioClip selectSound;
    // Reference to the ScriptableObject for the Weapon this UpgradeSlot represents
    [SerializeField] public WeaponScriptableObject thisWeapon;
    // The text used to desrcibe this UpgradeSlot
    [SerializeField] TextMeshProUGUI text;

    // Updates the text that informs the Player of their potential level up upgrade (that is held within this UpgradeSlot)
    public void UpdateVisuals()
    {
        if (thisWeapon != null)
        {
            // Update the visuals
            text.text = $"{thisWeapon.name} {GameManager.instance.GetWeaponLevel(thisWeapon)}"; // temp, later we should add icons as well
        }
        else
        {
            // Set the visual to items that should be offered if every weapon is upgraded
        }
    }

    // Plays the selectSound, hides the level up menu, and gives the Player the Weapon stored in this UpgradSlot
    public void OnClickSelect()
    {
        // Guard against selectSound not existing
        if (selectSound) 
            SFXManager.instance.PlayClip(selectSound);

        // Hides level up menu
        UIManager.instance.HideLevelUpOptions();

        // Give upgraded weapon to Player
        foreach (Weapon weapon in UIManager.instance.PC.ownedWeapons)
        {
            //Debug.Log($"checked {weapon.weaponSO} vs {thisWeapon} = {weapon.weaponSO == thisWeapon}");
            if (weapon.weaponSO == thisWeapon)
            {
                weapon.LevelWeapon();
                return;
            }
        }

        // If weapon wasnt found (like when Player gets it for the first time), add it
        UIManager.instance.PC.AddWeapon(thisWeapon);
    }
}
