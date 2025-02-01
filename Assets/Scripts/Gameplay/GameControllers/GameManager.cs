using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Public reference to this GameManager for use in other scripts
    public static GameManager instance;

    // Reference to CountdownTimer
    [SerializeField] CountdownTimer timer;
    // Reference to EnemySpawner
    [SerializeField] EnemySpawner enemySpawner;
    // Reference to the prefab used to create the Player
    [SerializeField] GameObject playerPrefab;
    // Reference to the Player to access their weapons
    [HideInInspector] public Transform player;
    // Whether the game is frozen (paused) or not, used to freeze player/enemies when we level up 
    [HideInInspector] public bool freezeGame;

    // List of all possible weapons in the game
    [SerializeField] List<WeaponScriptableObject> allWeapons;
    // List of weapons the Player can receive upon leveling up
    [HideInInspector] public List<WeaponScriptableObject> possibleUpgrades;

    private void Start()
    {
        // Create references to this GameManager and the list of possibleUpgrades
        instance = this;
        possibleUpgrades = new List<WeaponScriptableObject>();

        // Start the game paused (in main menu)
        freezeGame = true;
    }

    private void Update()
    {
        // If game isn't frozen, resume typical the passage of Time
        // Otherwise (game is frozen), stop the passage of Time (stopping all things dependant on Scaled Time, as well as the Physics Engine)
        if (!freezeGame)
        {
            // Game is running normally
            Time.timeScale = 1f; 
        }
        else
        {
            // Game is paused or frozen
            Time.timeScale = 0f; 
        }
    }

    // Start the game, initializing the Player, their UI, the Camera that locks to the Player,
    // enemy spawn waves, the timer, possibleUpgrades list, and unpauses the game
    public void StartGame()
    {
        player = Instantiate(playerPrefab).transform;
        UIManager.instance.PC = player.GetComponentInChildren<PlayerController>();
        FindObjectOfType<CameraFollow2D>().target = player;
        enemySpawner.StartWaves();
        timer.StartTimer();
        possibleUpgrades = allWeapons;
        freezeGame = false;
    }

    // Freezes game, stops timer and spawn waves, and displays the victory screen
    public void WinGame()
    { 
        freezeGame = true;
        timer.StopTimer();
        enemySpawner.StopWaves();

        UIManager.instance.ShowWinScreen();
    }

    // Freezes game, destroys Player, stops timer and spawn waves, and displays the game over screen
    public void EndGame()
    {
        freezeGame = true;
        Destroy(player.gameObject);
        timer.StopTimer();
        UIManager.instance.gameOver.SetActive(true);
        enemySpawner.StopWaves();
    }

    // Reloads the entire Scene
    public void ReloadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    // Returns a random Weapon from the list of possibleUpgrades
    public WeaponScriptableObject GetRandomUpgrade()
    {
        // If the weapon is at its max level we remove it from the possible upgrades
        foreach(Weapon weapon in player.GetComponent<PlayerController>().ownedWeapons)
        {
            if (weapon.currentLevel >= weapon.maxLevel && possibleUpgrades.Contains(weapon.weaponSO))
            {
                Debug.Log("weapon already maxed");
                possibleUpgrades.Remove(weapon.weaponSO);
            }
        }

        if (possibleUpgrades.Count > 0)
        {
            // Return a random item from possibleUpgrades
            int randomIndex = Random.Range(0, possibleUpgrades.Count);
            return possibleUpgrades[randomIndex];
        }
        else
            return null;
    }

    // Returns the Roman numeral representation of weaponToCheck's level as a String
    public string GetWeaponLevel(WeaponScriptableObject weaponToCheck)
    {
        foreach (Weapon weapon in player.GetComponent<PlayerController>().ownedWeapons)
        {
            if (weapon.weaponSO == weaponToCheck)
            {
                return ConvertToRoman(weapon.currentLevel + 1);
            }
        }

        return "";
    }

    // Helper method to convert an integer to a Roman numeral
    private string ConvertToRoman(int number)
    {
        if (number < 1) return "";
        if (number >= 1000) return "M" + ConvertToRoman(number - 1000);
        if (number >= 900) return "CM" + ConvertToRoman(number - 900);
        if (number >= 500) return "D" + ConvertToRoman(number - 500);
        if (number >= 400) return "CD" + ConvertToRoman(number - 400);
        if (number >= 100) return "C" + ConvertToRoman(number - 100);
        if (number >= 90) return "XC" + ConvertToRoman(number - 90);
        if (number >= 50) return "L" + ConvertToRoman(number - 50);
        if (number >= 40) return "XL" + ConvertToRoman(number - 40);
        if (number >= 10) return "X" + ConvertToRoman(number - 10);
        if (number >= 9) return "IX" + ConvertToRoman(number - 9);
        if (number >= 5) return "V" + ConvertToRoman(number - 5);
        if (number >= 4) return "IV" + ConvertToRoman(number - 4);
        if (number >= 1) return "I" + ConvertToRoman(number - 1);

        return "";
    }
}
