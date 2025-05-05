using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : SceneSingleton<UpgradeManager>
{
    public int monstersKilled = 0;
    public int monstersPerUpgrade = 20;
    
    public float towerDamageIncrease = 0.1f;
    public int goldIncreaseAmount = 2;
    public float enemySpeedReduction = 0.05f;

    public int towerDamageUpgrades = 0;
    public int goldIncreaseUpgrades = 0;
    public int enemySpeedReductionUpgrades = 0;

    public void IncrementKillCount()
    {
        monstersKilled++;
        if (monstersKilled % monstersPerUpgrade == 0)
        {
            UIManager.Instance.ShowUpgradeOptions();
        }
    }

    public void ApplyUpgrade(int choice)
    {
        switch (choice)
        {
            case 0: // Tower Damage
                towerDamageUpgrades++;
                ApplyTowerDamageUpgrade();
                break;
            case 1: // Gold Increase
                goldIncreaseUpgrades++;
                break;
            case 2: // Enemy Speed Reduction
                enemySpeedReductionUpgrades++;
                break;
        }
    }

    private void ApplyTowerDamageUpgrade()
    {
        var towers = FindObjectsOfType<Character1>();
        foreach (var tower in towers)
        {
            foreach (var skillInstance in tower.skillInstances)
            {
                skillInstance.info.Damage *= (1 + towerDamageIncrease);
            }
        }
    }

    public float GetEnemySpeedMultiplier()
    {
        return Mathf.Pow(1 - enemySpeedReduction, enemySpeedReductionUpgrades);
    }

    public int GetExtraGold()
    {
        return goldIncreaseAmount * goldIncreaseUpgrades;
    }
}