using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : DD_Singleton<TowerManager>
{
    public GameObject[] towerPrefabs;  // 타워 프리팹 배열
    public GameObject[] towerRangePrefabs;  // 타워 범위 표시 프리팹 배열
    public GameObject[] towerUpgradePrefabs;  // 타워 업그레이드 프리팹 배열
    public GameObject[] towerUpgradeEffectPrefabs;  // 타워 업그레이드 이펙트 프리팹 배열

  
    private Dictionary<GridFloor, GameObject> placedTowers = new Dictionary<GridFloor, GameObject>();
    private Dictionary<GridFloor, GameObject> towerRanges = new Dictionary<GridFloor, GameObject>();
    private Dictionary<GridFloor, GameObject> towerUpgradeEffects = new Dictionary<GridFloor, GameObject>();

    /// <summary>
    /// 타워를 선택하는 함수
    /// </summary>
    /// <param name="towerIndex">선택할 타워의 인덱스</param>
    public void SelectTower(int towerIndex)
    {
        if (towerIndex >= 0 && towerIndex < towerPrefabs.Length)
        {
            // 선택된 타워의 범위 표시
            ShowTowerRange(towerIndex);
        }
    }

    /// <summary>
    /// 타워를 배치하는 함수
    /// </summary>
    /// <param name="gridFloor">타워를 배치할 그리드</param>
    public void PlaceTower(GridFloor gridFloor)
    {
        if (!placedTowers.ContainsKey(gridFloor))
        {
            // 타워 생성 및 배치
            GameObject tower = Instantiate(towerPrefabs[0], gridFloor.transform.position, Quaternion.identity);
            placedTowers.Add(gridFloor, tower);
            
            // 타워 범위 표시
            ShowTowerRange(0);
        }
    }

    /// <summary>
    /// 타워를 제거하는 함수
    /// </summary>
    /// <param name="gridFloor">타워를 제거할 그리드</param>
    public void RemoveTower(GridFloor gridFloor)
    {
        if (placedTowers.ContainsKey(gridFloor))
        {
            // 타워 및 관련 오브젝트 제거
            Destroy(placedTowers[gridFloor]);
            placedTowers.Remove(gridFloor);
            
            // 타워 범위 숨기기
            HideTowerRange(gridFloor);
        }
    }

    /// <summary>
    /// 타워를 업그레이드하는 함수
    /// </summary>
    /// <param name="gridFloor">업그레이드할 타워가 있는 그리드</param>
    public void UpgradeTower(GridFloor gridFloor)
    {
        if (placedTowers.ContainsKey(gridFloor))
        {
            // 기존 타워 제거
            Destroy(placedTowers[gridFloor]);
            
            // 업그레이드된 타워 생성
            GameObject upgradedTower = Instantiate(towerUpgradePrefabs[0], gridFloor.transform.position, Quaternion.identity);
            placedTowers[gridFloor] = upgradedTower;
            
            // 업그레이드 이펙트 표시
            ShowUpgradeEffect(gridFloor);
        }
    }

    /// <summary>
    /// 타워의 범위를 표시하는 함수
    /// </summary>
    /// <param name="towerIndex">타워의 인덱스</param>
    private void ShowTowerRange(int towerIndex)
    {
        if (towerIndex >= 0 && towerIndex < towerRangePrefabs.Length)
        {
            // 범위 표시 오브젝트 생성
            GameObject rangeIndicator = Instantiate(towerRangePrefabs[towerIndex]);
            // 범위 표시 위치 설정
            rangeIndicator.transform.position = new Vector3(0, 0.1f, 0);
        }
    }

    /// <summary>
    /// 타워의 범위를 숨기는 함수
    /// </summary>
    /// <param name="gridFloor">타워가 있는 그리드</param>
    private void HideTowerRange(GridFloor gridFloor)
    {
        if (towerRanges.ContainsKey(gridFloor))
        {
            Destroy(towerRanges[gridFloor]);
            towerRanges.Remove(gridFloor);
        }
    }

    /// <summary>
    /// 업그레이드 이펙트를 표시하는 함수
    /// </summary>
    /// <param name="gridFloor">업그레이드된 타워가 있는 그리드</param>
    private void ShowUpgradeEffect(GridFloor gridFloor)
    {
        if (!towerUpgradeEffects.ContainsKey(gridFloor))
        {
            // 이펙트 생성 및 표시
            GameObject effect = Instantiate(towerUpgradeEffectPrefabs[0], gridFloor.transform.position, Quaternion.identity);
            towerUpgradeEffects.Add(gridFloor, effect);
            
            // 이펙트 자동 제거
            Destroy(effect, 2f);
        }
    }
}