using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SceneSingleton<UIManager>
{
    public Button[] towerSelectionButtons;
    public GameObject towerOptionsPanel;
    public Button placeTowerButton;
    public Button removeTowerButton;
    public Button upgradeTowerButton;
    public TextMeshProUGUI RemainMonsterText;
    public TextMeshProUGUI GoldText;

    public GameObject SuccessText;
    public GameObject FailText;
    
    private GridFloor selectedGridFloor;

    public GameObject skillInfoPanel;
    public TextMeshProUGUI skillDescriptionText;

    public GameObject upgradePanel;
    public Button[] upgradeButtons;

    public Camera mainCamera;
    private Character1 currentTower;

    public GameObject gameOverPanel;
    public Button restartButton;
    public Button goToTitleButton;
    
    
    public Button toggleSpeedButton;
    
    public Image defaultSpeedImage;
    public Image doubleSpeedImage;

    private bool isSpeedNormal = true;
    
    [SerializeField] private GameObject StageText;
    
    void Start()
    {
        for (int i = 0; i < towerSelectionButtons.Length; i++)
        {
            int index = i;
            towerSelectionButtons[i].onClick.AddListener(() => SelectTower(index));
        }

        placeTowerButton.onClick.AddListener(PlaceSelectedTower);
        removeTowerButton.onClick.AddListener(RemoveSelectedTower);
        upgradeTowerButton.onClick.AddListener(UpgradeSelectedTower);

        HideTowerOptions();
        
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found");
        }
        
        if (skillInfoPanel == null  || skillDescriptionText == null)
        {
            Debug.LogError("SkillInfoPanel are not assigned");
        }
        
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int index = i;
            upgradeButtons[i].onClick.AddListener(() => OnUpgradeSelected(index));
        }
        toggleSpeedButton.onClick.AddListener(ToggleGameSpeed);
        UpdateButtonImage();
        restartButton.onClick.AddListener(GameManager.Instance.RestartGame);
        goToTitleButton.onClick.AddListener(GameManager.Instance.GoToTitle);
    }
    public void ShowUpgradeOptions()
    {
        SoundManager.Instance.PlaySFX("UpgradePannel");
        upgradePanel.SetActive(true);
        Time.timeScale = 0; // 게임 일시 정지
    }

    private void OnUpgradeSelected(int choice)
    {
        UpgradeManager.Instance.ApplyUpgrade(choice);
        SoundManager.Instance.PlaySFX("UpgradeSelect");
        upgradePanel.SetActive(false);
        Time.timeScale = 1; // 게임 재개
    }
    
    void SelectTower(int index)
    {
        
        TowerManager.Instance.SelectTower(index);
        if (TowerManager.Instance.selectedTowerPrefab != null)
        {
            Character1 tower = TowerManager.Instance.selectedTowerPrefab.GetComponent<Character1>();
            if (tower != null && tower.skillInstances.Count > 0)
            {
                ShowSkillInfoPanel(tower);
            }
        }
        else
        {
            HideSkillInfoPanel();
        }
    
    }
    
    public void UpdateSkillInfoPanelPosition()
    {
        if (skillInfoPanel.activeSelf && currentTower != null)
        {
            Vector3 towerPosition = currentTower.transform.position;
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(towerPosition);
            skillInfoPanel.transform.position = screenPosition + new Vector3(0, 100, 0); // 패널을 타워 위 100픽셀에 위치시킵니다.
        }
    }
    public void HideSkillInfoPanel()
    {
        skillInfoPanel.SetActive(false);
        currentTower = null;
    }
    public void ShowSkillInfoPanel(Character1 tower)
    {
        if (tower != null && tower.skillInstances.Count > 0)
        {
            UpdateInfo(tower);
        }
        else
        {
            HideSkillInfoPanel();
        }
    }


    public void UpdateInfo(Character1 tower)
    {
        currentTower = tower;
        skillInfoPanel.SetActive(true);
        SkillInfo skillInfo = tower.skillInstances[0].info;
        skillDescriptionText.text = $"Damage: {skillInfo.Damage}\n" +
                                    $"Attack Distance: {skillInfo.AttackDistance}\n" +
                                    $"Cooldown: {skillInfo.Cooltime}\n" +
                                    $"AOE Damage: {skillInfo.AreaOfEffectDamage}\n" +
                                    $"AOE Interval: {skillInfo.AreaOfEffectDamageInterval}\n" +
                                    $"Debuff Duration: {skillInfo.DebuffDuration}";
        UpdateSkillInfoPanelPosition();
    }


    void PlaceSelectedTower()
    {
        if (selectedGridFloor != null)
        {
            selectedGridFloor.PlaceTower();
            ShowTowerOptions(selectedGridFloor, true);
        }
    }

    void RemoveSelectedTower()
    {
        if (selectedGridFloor != null)
        {
            selectedGridFloor.RemoveTower();
            ShowTowerOptions(selectedGridFloor, false);
        }
    }

    void UpgradeSelectedTower()
    {
        if (selectedGridFloor != null)
        {
            selectedGridFloor.UpgradeTower();
        }
    }
    
    
    public void ShowTowerOptions(GridFloor gridFloor, bool isTowerPlaced)
    {
        if (selectedGridFloor != null && selectedGridFloor != gridFloor)
        {
            selectedGridFloor.Deselect();
        }

        selectedGridFloor = gridFloor;
        towerOptionsPanel.SetActive(true);
        
        Character1 tower = null;
        if (TowerManager.Instance.placedTowers.TryGetValue(gridFloor, out tower))
        {
           
            ShowSkillInfoPanel(tower);
        }
        else
        {
            HideSkillInfoPanel();
        }

        placeTowerButton.gameObject.SetActive(!isTowerPlaced);
        removeTowerButton.gameObject.SetActive(isTowerPlaced);
        upgradeTowerButton.gameObject.SetActive(isTowerPlaced);
    }

    
    public void HideTowerOptions()
    {
        if (selectedGridFloor != null)
        {
            selectedGridFloor.Deselect();
            selectedGridFloor = null;
        }
        towerOptionsPanel.SetActive(false);
        HideSkillInfoPanel();
    }

    
    public GridFloor GetSelectedGridFloor()
    {
        return selectedGridFloor;
    }

    public void ShowMessage(string name)
    {
        switch (name)
        {
            case "Success":
                StartCoroutine(ShowSuccess());
                break;
            case "Fail":
                StartCoroutine(ShowFail());
                break;
        }
    }

    IEnumerator ShowSuccess()
    {
        SuccessText.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        SuccessText.SetActive(false);
    }
    
    IEnumerator ShowFail()
    {
        FailText.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        FailText.SetActive(false);
    }


    public void ShowStageText(string stageName)
    {
        StartCoroutine(ShowStage(stageName));
    }
    IEnumerator ShowStage(string stageName)
    {
        StageText.SetActive(true);
        StageText.GetComponent<TextMeshProUGUI>().text = stageName;
        yield return new WaitForSeconds(3.0f);
        StageText.SetActive(false);
    }
    
    
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0; 
    }

    public void HideGameOverPanel()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1; 
    }
    
    
    void ToggleGameSpeed()
    {
        if (isSpeedNormal&&Time.timeScale != 0f)
        {
            Time.timeScale = 2f;
            isSpeedNormal = !isSpeedNormal;
            SoundManager.Instance.PlaySFX("Place");
            UpdateButtonImage();
        }
        else if(Time.timeScale != 0f)
        {
            Time.timeScale = 1f;
            isSpeedNormal = !isSpeedNormal;
            SoundManager.Instance.PlaySFX("Place");
            UpdateButtonImage();
        }
        
    }

   
    void UpdateButtonImage()
    {
        if (isSpeedNormal)
        {
            defaultSpeedImage.gameObject.SetActive(true);
            doubleSpeedImage.gameObject.SetActive(false);
        }
        else
        {
            defaultSpeedImage.gameObject.SetActive(false);
            doubleSpeedImage.gameObject.SetActive(true);
        }
    }
    
    
}