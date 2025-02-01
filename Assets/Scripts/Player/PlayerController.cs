using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Sound played when taking damage
    [SerializeField] AudioClip hurtSound;

    // Health Player can have, and what they start with
    [SerializeField] public int maxHP = 100;
    // Health Player currently has
    [SerializeField] public int currentHP = 100;

    // Exp Player currently has
    [HideInInspector] public int currentExp = 0;
    // Exp Player needs to level up
    [SerializeField] public int neededExp = 10;

    // Player's current level
    [HideInInspector] public int level = 0;
    // List of Player's weapons
    [HideInInspector] public List<Weapon> ownedWeapons;

    private void Start()
    {
        // Set Player's health to its max and give them their starting weapon
        currentHP = maxHP;
        ownedWeapons = new List<Weapon>();
        foreach (Weapon startingWeapons in GetComponentsInChildren<Weapon>())
            ownedWeapons.Add(startingWeapons);
    }

    private void Update()
    {
        // TODO Secret debugging features, remove when game is finished
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.LogWarning("Dev command: add exp");
            GainExp(10);
        }
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            Debug.LogWarning("Dev command: add health");
            GainHp(10);
        }
    }


    // Decrement Player's health by dmg, and play a sound, if this kills the Player, end the game
    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        SFXManager.instance.PlayClip(hurtSound);

        if (currentHP <= 0)
            GameManager.instance.EndGame();
    }

    // Increment Player's currentExp by exp, if this gives them their neededExp, then level up the Player
    public void GainExp(int exp)
    {
        currentExp += exp;
        if (currentExp >= neededExp)
        {
            LevelUp();
        }
    }

    // Increment Player's currentHP by hp, if this gives them more than their maxHP, then reset their currentHP to maxHP
    public void GainHp(int hp)
    {
        currentHP += hp;
        if (currentHP > maxHP)
        {
            currentHP = maxHP; 
        }
    }

    // Level up Player, pause game, and show them their new weapon options 
    private void LevelUp()
    {
        // Level up, calculate next neededExp to level up
        level += 1;
        currentExp -= neededExp;
        neededExp += level * 2;

        // Pause game and display level up menu
        GameManager.instance.freezeGame = true;
        CountdownTimer.instance.StopTimer();
        UIManager.instance.DisplayLevelUpOptions();
    }

    // Give Player the given weapon
    public void AddWeapon(WeaponScriptableObject weapon)
    {
        ownedWeapons.Add(Instantiate(weapon.weaponPrefab, this.transform).GetComponent<Weapon>());
    }
}
