using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject[] unitsButtons;

    [SerializeField] private GameObject parentGameScene;
    private LevelComponent[] gameScene;
    [SerializeField] private GameObject backgroundUI;

    [SerializeField] private GameObject settings;

    [SerializeField] private GameObject levelsMenu;
    [SerializeField] private GameObject[] levelsOpen;

    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject winPanel;

    [SerializeField] private GameObject pausePanel;


    [SerializeField] private Image fillBar;

    public GameInfo gameInfo;

    [SerializeField] private int maxLevels = 3;

    private int currentIndexLevel = 0;

    [SerializeField] private int indexUnit = 0;

    private UnitTower playerTower;

    public static GameManager Instance = null;

    private bool secondChanceIsUse = false;
    private bool healIsUse = false;
    private bool justAdvIsReady = false;

    [SerializeField] private float timeToShowAd = 120;
    [SerializeField] private float timer;


    private bool isSpeedx2 = true;
    private float timeSpeed;
    private bool adForSpeed = true;


    private GameObject prevButton;

    [DllImport("__Internal")]
    private static extern void IncreaseSpeedTimeExtern();

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
            Init();
        }
        else if (Instance == this)
        { 
            Destroy(gameObject); 
        }

       
    }

    private void Init()
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

    private void ResetTowers()
    {
        foreach (UnitTower tower in backgroundUI.GetComponentsInChildren<UnitTower>())
        {
            tower.Init();
        }
    }

    private void Start()
    {
        timer = timeToShowAd;
        try
        {
           LoadExtern();
        }
        catch
        {

        }
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

    public void ChangeFillBar(float value)
    {
        fillBar.fillAmount = value;
    }

    public void StartGame(Transform levelButton)
    {
        indexUnit = 0;

        int indexLevel = int.Parse(levelButton.gameObject.name[levelButton.gameObject.name.Length - 1].ToString());

        LoadLevel(indexLevel);
    }

    private void LoadLevel(int indexLevel)
    {
        if (gameInfo.levels < indexLevel)
        {
            BackToMainMenu();
            return;
        }

        ResetTowers();

        DeleteUnits();

        currentIndexLevel = indexLevel;
        adForSpeed = true;
        isSpeedx2 = false;
        timeSpeed = 1f;
        Unpause();

        defeatPanel.SetActive(false);
        winPanel.SetActive(false);
        pausePanel.SetActive(false);

        parentGameScene.SetActive(true);


        TurnOnLevel(indexLevel);

        gameUI.SetActive(true);

        mainMenu.SetActive(false);
        backgroundUI.SetActive(false);

        playerTower.ChangeUnit(0);

        CheckColorUnits(0);
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

        adForSpeed = true;
        isSpeedx2 = false;
        timeSpeed = 1f;
        Unpause();

        parentGameScene.SetActive(false);
        gameUI.SetActive(false);

        mainMenu.SetActive(true);
        menuUI.SetActive(true);
        levelsMenu.SetActive(false);


        backgroundUI.SetActive(true);


    }

    public void IncreaseSpeedTimeAdv()
    {

        try
        {
            if (adForSpeed)
            {

                Pause();

                adForSpeed = false;
                IncreaseSpeedTimeExtern();
            }
            else
            {
                ChangeSpeedTime();
            }
        }
        catch
        {
            ChangeSpeedTime();
        }
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

    public void CloseChoosingLevelPanel()
    {
        levelsMenu.SetActive(false);
        menuUI.SetActive(true);
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
        Time.timeScale = timeSpeed;
    }

    public void ChangeSpeedTime()
    {
        timeSpeed = isSpeedx2 ? 1f : 2f;
        isSpeedx2 = timeSpeed == 2f;


        Unpause();
        Time.timeScale = timeSpeed;
    }

    

    public void ChangeUnit(GameObject button)
    {
        int index = int.Parse(button.gameObject.name[button.gameObject.name.Length - 1].ToString()) - 1;


        playerTower.ChangeUnit(index);

    }

    public void CheckColorUnits(int choosingUnit)
    {
        UnitsSwitch[] unitsList = playerTower.GetUnitsCount();
        int kills = playerTower.GetCountKills();
        foreach(GameObject unit in unitsButtons)
        {
            int index = int.Parse(unit.gameObject.name[unit.gameObject.name.Length - 1].ToString()) - 1;

           
            if (index >= unitsList.Length) return;


            if (choosingUnit == index && kills >= unitsList[index].killsRequire)
                unit.GetComponent<Image>().color = Color.white;
            else
                unit.GetComponent<Image>().color = Color.gray;



            if (kills >= unitsList[index].killsRequire)
                unit.GetComponentsInChildren<Image>()[1].color = Color.white;
            else
                unit.GetComponentsInChildren<Image>()[1].color = Color.black;
        }
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
        LoadLevel(currentIndexLevel + 1);
    }

    public void SecondChance()
    {
        if (secondChanceIsUse) return;
        try
        {
           SecondChanceExtern();
        }
        catch
        {

        }
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
        try
        {
          HealTowerExtern();
        }
        catch
        {
            AdvIsShowHeal();
        }
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
        try
        {
           ShowAdvExtern();

            justAdvIsReady = false;

            timer = timeToShowAd;
        }
        catch
        {

        }

    }

    private void SaveData()
    {
        /*PlayerPrefs.SetInt("OpenLevels", levels);
        PlayerPrefs.Save();*/

        string jsonString = JsonUtility.ToJson(gameInfo); 
        try
        {
           SaveExtern(jsonString);
        }
        catch
        {

        }
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


