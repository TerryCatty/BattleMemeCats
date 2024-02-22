using System;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class GameInfo
{
    public int levels = 1;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject gameUI;

    [SerializeField] private GameObject parentGameScene;
    private LevelComponent[] gameScene;
    [SerializeField] private GameObject backgroundUI;

    [SerializeField] private GameObject settings;

    [SerializeField] private GameObject levelsMenu;
    [SerializeField] private GameObject[] levelsOpen;

    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject winPanel;

    [SerializeField] private GameObject pausePanel;

    public GameInfo gameInfo;

    [SerializeField] private int maxLevels = 3;

    [SerializeField] private int indexUnit = 0;

    private UnitTower playerTower;

    public static GameManager Instance = null;

    private bool secondChanceIsUse = false;
    private bool healIsUse = false;
    private bool justAdvIsReady = false;

    [SerializeField] private float timeToShowAd = 120;
    [SerializeField] private float timer;

    [DllImport("__Internal")]
    private static extern void ShowAdvExtern();

    [DllImport("__Internal")]
    private static extern void SecondChanceExtern();

    [DllImport("__Internal")]
    private static extern void HealTowerExtern();
    [DllImport("__Internal")]
    private static extern void SaveExtern(string value);
    [DllImport("__Internal")]
    private static extern void LoadExtern();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            indexUnit = 0;

            gameScene = parentGameScene.GetComponentsInChildren<LevelComponent>();

            for (int i = 1; i < gameScene.Length; i++)
            {
                gameScene[i].gameObject.SetActive(false);

            }

            backgroundUI.SetActive(true);
            gameUI.SetActive(false);
            parentGameScene.SetActive(false);

            DontDestroyOnLoad(gameObject);
        }
        else if (Instance == this)
        { 
            Destroy(gameObject); 
        }

       
    }

    private void Start()
    {
        timer = timeToShowAd;
        LoadExtern();
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            justAdvIsReady = true;
        }
    }

    public void StartGame(Transform levelButton)
    {
        indexUnit = 0;

        int indexLevel = int.Parse(levelButton.gameObject.name[levelButton.gameObject.name.Length - 1].ToString());

        LoadLevel(indexLevel);
    }

    private void LoadLevel(int indexLevel)
    {
        if (gameInfo.levels < indexLevel) return;


        DeleteUnits();

        defeatPanel.SetActive(false);
        winPanel.SetActive(false);
        pausePanel.SetActive(false);

        parentGameScene.SetActive(true);


        TurnOnLevel(indexLevel);

        gameUI.SetActive(true);

        mainMenu.SetActive(false);
        backgroundUI.SetActive(false);

        playerTower.ChangeUnit(0);
    }

    private void TurnOnLevel(int indexLevel)
    {
        for (int i = 0; i < gameScene.Length; i++)
        {
            gameScene[i].gameObject.SetActive(false);

            if (i == indexLevel - 1)
            {
                gameScene[i].gameObject.SetActive(true);
                gameScene[i].Init();

                foreach (UnitTower tower in gameScene[i].towers)
                {
                    tower.Init();
                    if (tower.colorTeam == Unit.TeamColor.Blue)
                    {
                        playerTower = tower;
                    }
                }

            }
        }
    }

    private void DeleteUnits()
    {
        UnitTower[] towers = FindObjectsByType<UnitTower>(sortMode: default);

        foreach (UnitTower t in towers)
        {
            t.DeleteAllUnits();
        }

    }

    public void BackToMainMenu()
    {
        CheckShowAd();
        DeleteUnits();

        Unpause();

        parentGameScene.SetActive(false);
        gameUI.SetActive(false);

        mainMenu.SetActive(true);
        menuUI.SetActive(true);
        levelsMenu.SetActive(false);


        backgroundUI.SetActive(true);
    }

    public void OpenMenuLevels()
    {
        ResetAdvBoolean();
        levelsMenu.SetActive(true);
        int i = 1;
        foreach (GameObject levelOpen in levelsOpen)
        {
            if (i <= gameInfo.levels) levelOpen.SetActive(false);
            else levelOpen.SetActive(true);
            i++;
        }
        menuUI.SetActive(false);
    }

    public void OpenSettings()
    {
        settings.SetActive(true);
        menuUI.SetActive(false);
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
        menuUI.SetActive(true);
    }

    public void OpenPausePanel()
    {
        pausePanel.SetActive(true);
        Pause();
    }

    private void ClosePausePanel()
    {
        pausePanel.SetActive(false);
        Unpause();
    }

    public void Continue()
    {
        ClosePausePanel();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
    }

    public void IncreaseSpeedTime()
    {
        Time.timeScale = 2f;
    }

    public void ChangeUnit(GameObject button)
    {
        int index = int.Parse(button.gameObject.name[button.gameObject.name.Length - 1].ToString()) - 1;

        playerTower.ChangeUnit(index);
    }

    public void Spawn()
    {
        playerTower.SpawnProcess();
    }

    public void DestroyTower(Unit.TeamColor colorTeam)
    {
        if(colorTeam == Unit.TeamColor.Blue)
        {
            Defeat();
        }
        else
        {
            Win();
        }
    }

    private void Defeat()
    {
        Pause();
        defeatPanel.SetActive(true);
        CheckShowAd();
        ResetAdvBoolean();
    }

    private void Win()
    {
        Pause();
        winPanel.SetActive(true);
        CheckShowAd();
        ResetAdvBoolean();
        IncreaseOpenLevelsCount();
    }


    private void IncreaseOpenLevelsCount()
    {
        if (gameInfo.levels < maxLevels)
        {
            gameInfo.levels++;
        }

        SaveData();
    }
    public void NextLevel()
    {
        ResetAdvBoolean();
        ClosePausePanel();
        LoadLevel(gameInfo.levels);
    }

    public void SecondChance()
    {
        if (secondChanceIsUse) return;

        SecondChanceExtern();
    }

    public void AdvIsShowSecondChance()
    {
        ReviveTower();

        secondChanceIsUse = true;
    }

    private void ReviveTower()
    {
        playerTower.gameObject.SetActive(true);
        playerTower.Init();
        DeleteUnits();

        Unpause();
        defeatPanel.SetActive(false);
    }
    public void HealTower()
    {
        if(healIsUse) return;

        HealTowerExtern();
    }

    public void AdvIsShowHeal()
    {
        healIsUse = true;
        playerTower.Heal(Convert.ToInt32(playerTower.baseHealth / 2));
    }

    private void ResetAdvBoolean()
    {
        healIsUse = false;
        secondChanceIsUse = false;
    }

    private void CheckShowAd()
    {
        if (justAdvIsReady == false) return;

        ShowAdvExtern();

        justAdvIsReady = false;

        timer = timeToShowAd;
    }

    private void SaveData()
    {
        /*PlayerPrefs.SetInt("OpenLevels", levels);
        PlayerPrefs.Save();*/

        string jsonString = JsonUtility.ToJson(gameInfo);
        SaveExtern(jsonString);
    }

    private void LoadData(string value)
    {
        //gameInfo.levels = PlayerPrefs.HasKey("OpenLevels") ? PlayerPrefs.GetInt("OpenLevels") : 1;

        gameInfo = JsonUtility.FromJson<GameInfo>(value);

    }

    public void ResetData()
    {
        gameInfo.levels = 1;
        SaveData();
    }
}


