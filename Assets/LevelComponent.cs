using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour
{
    public UnitTower[] towers;

    public void Init()
    {
        foreach (var tower in towers)
        {
            tower.gameObject.SetActive(true);
            tower.Init();
        }
    }
}
