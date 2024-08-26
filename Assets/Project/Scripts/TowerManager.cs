using System.Collections.Generic;
using UnityEngine;

public class TowerManager : SceneSingleton<TowerManager>
{
    public List<GameObject> towerPrefabs;
    public GameObject selectedTowerPrefab;
    public Dictionary<GridFloor, Character1> placedTowers = new Dictionary<GridFloor, Character1>();
    public int TowerValue = 5;
    public int upgradeValue = 5;

    public void SelectTower(int index)
    {
        if (index >= 0 && index < towerPrefabs.Count)
        {
            selectedTowerPrefab = towerPrefabs[index];
            
        }
        else
        {
            selectedTowerPrefab = null;
        }
    }

    public void PlaceTower(GridFloor gridFloor)
    {
        SoundManager.Instance.PlaySFX("Place");   
        if (MyPlayerController.Instance.Gold >= TowerValue && selectedTowerPrefab != null && !placedTowers.ContainsKey(gridFloor))
        {
            MyPlayerController.Instance.Gold -= TowerValue;
            Vector3 position = gridFloor.transform.position + Vector3.up * 0.5f;
            GameObject towerInstance = Instantiate(selectedTowerPrefab, position, Quaternion.identity);
            Character1 tower = towerInstance.GetComponent<Character1>();
            placedTowers[gridFloor] = tower;
        }
        else
        {
            SoundManager.Instance.PlaySFX("NoMoney");
        }
    }

    public void RemoveTower(GridFloor gridFloor)
    {
        if (placedTowers.TryGetValue(gridFloor, out Character1 tower))
        {
            Destroy(tower.gameObject);
            placedTowers.Remove(gridFloor);
        }
    }

    public void UpgradeTower(GridFloor gridFloor)
    {
   
        if (placedTowers.TryGetValue(gridFloor, out Character1 tower))
        {
            
            if (MyPlayerController.Instance.Gold >= upgradeValue + tower.level)
            {
                MyPlayerController.Instance.Gold -= upgradeValue + tower.level;
                if ((Random.Range(0, 1.0f) > 0.5f))
                {
                    UIManager.Instance.ShowMessage("Success");
                    SoundManager.Instance.PlaySFX("Success");
                    Debug.Log("upgrade Sucess");
                    tower.level++;

                    foreach (var skillInstance in tower.skillInstances)
                    {

                        switch (skillInstance.info.SkillName)
                        {
                            case "StunMagic":
                                skillInstance.info.Damage *= 1.1f;
                                skillInstance.info.AttackDistance += 0.5f;
                                skillInstance.info.Cooltime *= 0.9f;
                                skillInstance.info.DebuffDuration *= 1.1f;
                                break;
                            case "AOEMagic":
                                skillInstance.info.AreaOfEffectDamage *= 1.1f;
                                skillInstance.info.AreaOfEffectDamageInterval *= 0.9f;
                                break;
                            case "PriMagic":
                                skillInstance.info.Damage *= 1.1f;
                                skillInstance.info.AttackDistance += 0.5f;
                                skillInstance.info.Cooltime *= 0.9f;
                                break;

                        }


                    }
                    
                    
                    UIManager.Instance.UpdateInfo(tower);
                }
                else
                {
                    UIManager.Instance.ShowMessage("Fail");
                    SoundManager.Instance.PlaySFX("Fail");
                }
            }
            else
            {
                SoundManager.Instance.PlaySFX("NoMoney");
            }
            

        }
    }
}