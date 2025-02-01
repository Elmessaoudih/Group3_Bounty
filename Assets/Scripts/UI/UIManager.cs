using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Reference to this UIManager for use in other scripts
    public static UIManager instance;
    // Whether the game Player has clicked to begin the game
    [HideInInspector] public bool gameStarted;

    // The UI for the menu used to control game settings like volumes
    [SerializeField] GameObject settingsMenu;

    // The UI of the game's title and insructions to start the game
    [SerializeField] GameObject startGameScreen;
    // The UI of game-relevant info like health and exp
    [SerializeField] GameObject gameplayHud;
    // The UI of the "You Died" screen and the button the replay the game
    [SerializeField] public GameObject gameOver;
    // End of game screen
    [SerializeField] public GameObject gameWin;
    // The UI of the upgrade options upon the Player leveling up
    [SerializeField] GameObject levelUpScreen;
    // Array of upgrades and their UI that Players choose upon leveling up
    [SerializeField] UpgradeSlot[] levelUpSlots;

    // Reference to the PlayerController script attached to the Player
    [HideInInspector] public PlayerController PC;

    private void Start()
    {
        // Initialize instance
        instance = this;
    }


    private void Update()
    {
        // Upon clicking the startGameScreen, game begins, startGameScreen is disabled, and gameplayHud is enabled
        if (Input.GetKeyDown(KeyCode.Mouse0) && !gameStarted)
        {
            gameStarted = true;
            GameManager.instance.StartGame();

            startGameScreen.SetActive(false);
            gameplayHud.SetActive(true);
        }

        // Upon pressing the ESC key after the gameStarted, toggle the settingMenu on/off and toggle whether the game is frozen/unfrozen 
        if (Input.GetKeyDown(KeyCode.Escape) && gameStarted)
        {
            bool state = !settingsMenu.activeInHierarchy;
            settingsMenu.SetActive(state);
            GameManager.instance.freezeGame = state;
        }
    }

    // Display the UI of each UpgradeSlot in levelUpSlots on the levelUpScreen
    public void DisplayLevelUpOptions()
    {
        foreach(UpgradeSlot slot in levelUpSlots)
        {
            slot.thisWeapon = GameManager.instance.GetRandomUpgrade();
            slot.UpdateVisuals();
        }

        levelUpScreen.SetActive(true);
    }

    // Disable the levelUpScreen, unpause the game, and unpause the game's timer
    public void HideLevelUpOptions()
    {
        levelUpScreen.SetActive(false);
        GameManager.instance.freezeGame = false;
        CountdownTimer.instance.StartTimer();
    }


    public void ShowWinScreen()
    {
        gameWin.SetActive(true);
    }
}
