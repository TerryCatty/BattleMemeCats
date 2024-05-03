using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UnitBuff
{
    public float damageBuff;
    public float speedBuff;
    public float healthBuff;
    public float attackRateBuff;
}
public class UnitTower : Unit
{
    [SerializeField] private Transform unit;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float reloadSpawnRate;

    [SerializeField] private int unitsPerOnetime;

    [SerializeField] private float timeSpawn;

    [SerializeField] private bool autoSpawn;

    private float timer;
    private bool canSpawn;

    [SerializeField] private bool undead = false;

    [SerializeField] private bool destroyUnitOnCollision = false;

    [SerializeField] private int maxCountUnits = 25;
    private int countUnits = 0;

    [SerializeField] private List<Transform> units;

    public UnitBuff buffs;

    [SerializeField] private UnitsSwitch[] unitsSwitch;
    [SerializeField] private int kills;

    [SerializeField] private Image healthBar;

    private UnitTower towerOpponent;
    private int indexChoosingUnit;

    public void Init()
    {
        countUnits = 0;
        kills = 0;
        health = baseHealth;

        isDead = false;

        DeleteAllUnits();
        HealthBar();

        UnitTower[] towers = transform.parent.GetComponentsInChildren<UnitTower>();

        foreach (UnitTower tower in towers)
        {
            if(tower.colorTeam != colorTeam)
            {
                towerOpponent = tower;
                break;
            }
        }
    }

    private void Update()
    {
        TimeToSpawn();

        CheckToSpawn();
    }

    private void CheckToSpawn()
    { 

        if (canSpawn == false) return;


        if (autoSpawn) {
            RandomChooseUnit();
        };
    }

    private void RandomChooseUnit()
    {
        int index = Random.Range(0, unitsSwitch.Length);


        if (unitsSwitch[index].killsRequire > kills)
        {
            RandomChooseUnit();
        }
        else
        {
            unit = unitsSwitch[index].unit;

            SpawnProcess();
        }
    }


    private void TimeToSpawn()
    {
        float fill = 0f;

        if (timer >= 0)
        {
            timer -= Time.deltaTime;
            canSpawn = false;

            float delta = 1f / reloadSpawnRate;


            fill = delta * timer;

        }
        if (timer <= 0 && canSpawn == false)
        {
            canSpawn = true;
        }


        if(colorTeam == TeamColor.Blue)
            GameManager.Instance.ChangeFillBar(fill);
    }
    public void SpawnProcess()
    {
        if (canSpawn == false) return;

        canSpawn = false;
        timer = reloadSpawnRate;

        StartCoroutine(SpawnUnitsPerRate());
    }

    
    IEnumerator SpawnUnitsPerRate()
    {
        for(int i = 0; i < unitsPerOnetime; i++)
        {
            SpawnUnit();
            yield return new WaitForSeconds(timeSpawn / unitsPerOnetime);
        }
       
    }
    private void SpawnUnit()
    {
        if (countUnits >= maxCountUnits) return;

        Transform spawnedUnit = Instantiate(unit, spawnPoint.position, Quaternion.identity);

        PlusUnit(spawnedUnit);

        spawnedUnit.GetComponentInChildren<UnitWarrior>().Init(this);

    }

    public void DeleteAllUnits()
    {
        foreach(Transform unit in units)
        {
            Destroy(unit.gameObject);
        }

        units = new List<Transform>();
    }

    public void MinusUnit(Transform unit)
    {
        units.Remove(unit);
        countUnits--;
    }

    public void IncreaseKillsCount()
    {
        kills++;
        if (colorTeam == TeamColor.Blue) GameManager.Instance.CheckColorUnits(indexChoosingUnit);
    }

    public void IncreaseKillsOpponent()
    {
        towerOpponent?.IncreaseKillsCount();
    }

    private void PlusUnit(Transform unit)
    {
        units.Add(unit);
        countUnits++;
    }

    
    public override void Heal(int value)
    {
        base.Heal(value);
        HealthBar();
    }
    public override void GetDamage(int damage)
    {
        if (undead) return;
        base.GetDamage(damage);
        HealthBar();
    }

    private void HealthBar()
    {

        float delta = 100.0f / baseHealth;

        if (healthBar == null) return;

        float fill = delta * health / 100.0f;
        healthBar.fillAmount = fill;
    }

    protected override void CheckDeath()
    {
        if (health <= 0)
        {
            isDead = true;

            GameManager.Instance.DestroyTower(colorTeam);
            
            gameObject.SetActive(false);
        }
    }

    public void ChangeUnit(int index)
    {
        if(index < unitsSwitch.Length && unitsSwitch[index].killsRequire <= kills)
        {
            unit = unitsSwitch[index].unit;
            indexChoosingUnit = index;
        }
        if (colorTeam == TeamColor.Blue) GameManager.Instance.CheckColorUnits(indexChoosingUnit);
    }

    public UnitsSwitch[] GetUnitsCount() { return unitsSwitch; }
    public int GetCountKills() { return kills; }
}


[System.Serializable]

public struct UnitsSwitch
{
    public Transform unit;
    public int killsRequire;
}