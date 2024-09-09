using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MyPlayerController : SceneSingleton<MyPlayerController>
{

    public List<GameObject> Monsters = new();
    public Dictionary<int, GameObject> MonsterInstances = new();
    
    private int currentPhase = 1;

    public int Gold = 0;
    
    
    [SerializeField] private int maxMonsters = 20;
    public List<GameObject> GetAliveMonsterList()
    {
        return MonsterInstances.Values.Where(m => m != null && m.GetComponent<Monster1>().IsAlive()).ToList();
    }

    private GameObject GetMonsterPrefabForPhase(int phase)
    {
        // 스테이지에 따라 다른 몬스터 사용
        int index = Mathf.Min(phase - 1, Monsters.Count - 1);
        return Monsters[index];
    }

    public void SetCurrentPhase(int phase)
    {
        currentPhase = phase;
    }
    
    void Update()
    {
        CleanupDestroyedMonsters();
        int aliveMonsters = GetAliveMonsterList().Count;
        UIManager.Instance.RemainMonsterText.text = ": " + GetAliveMonsterList().Count;
        UIManager.Instance.GoldText.text = ": " + Gold;
        
        
        if (aliveMonsters > maxMonsters)
        {
            GameManager.Instance.GameOver();
        }
    }
    public List<GameObject> GetMonsterList()
    {
        return MonsterInstances.Values.ToList();
    }
    
    public void RemoveMonster(GameObject monster)
    {
        if (MonsterInstances.ContainsKey(monster.GetInstanceID()))
        {
            MonsterInstances.Remove(monster.GetInstanceID());
        }
    }

    public void CleanupDestroyedMonsters()
    {
        List<int> keysToRemove = new List<int>();
        foreach (var kvp in MonsterInstances)
        {
            if (kvp.Value == null)
            {
                keysToRemove.Add(kvp.Key);
            }
        }
        foreach (int key in keysToRemove)
        {
            MonsterInstances.Remove(key);
        }
    }
    
    public GameObject GetNewMonster(Vector3 position, Quaternion rotation)
    {
        GameObject prefab = GetMonsterPrefabForPhase(currentPhase);
        GameObject instance = Instantiate(prefab, position, rotation);
        MonsterInstances.Add(instance.GetInstanceID(), instance);
        return instance;
    }
}