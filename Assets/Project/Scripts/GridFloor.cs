using UnityEngine;

public class GridFloor : MonoBehaviour
{
    private Material originMaterial;
    private Renderer renderer;
    private bool isTowerPlaced = false;
    public bool IsSelected { get; private set; }

    void Awake()
    {
        // 렌더러와 원본 머티리얼 초기화
        renderer = GetComponent<Renderer>();
        originMaterial = renderer.material;
    }

    private void OnMouseDown()
    {
        Select();
    }

    /// <summary>
    /// 그리드를 선택하는 함수
    /// </summary>
    public void Select()
    {
        // 이전에 선택된 그리드가 있다면 선택 해제
        GridFloor previousSelected = UIManager.Instance.GetSelectedGridFloor();
        SoundManager.Instance.PlaySFX("SelectGrid");
        if (previousSelected != null && previousSelected != this)
        {
            previousSelected.Deselect();
        }

        // 현재 그리드 선택 상태 업데이트
        IsSelected = true;
        UpdateVisual();
        UIManager.Instance.ShowTowerOptions(this, isTowerPlaced);
    }

    /// <summary>
    /// 그리드 선택을 해제하는 함수
    /// </summary>
    public void Deselect()
    {
        IsSelected = false;
        UpdateVisual();
    }
    
    /// <summary>
    /// 그리드에 타워를 배치하는 함수
    /// </summary>
    public void PlaceTower()
    {
        if (!isTowerPlaced)
        {
            // 타워 배치 및 상태 업데이트
            TowerManager.Instance.PlaceTower(this);
            isTowerPlaced = true;
            Deselect();
            UpdateVisual();
        }
    }

    /// <summary>
    /// 그리드에서 타워를 제거하는 함수
    /// </summary>
    public void RemoveTower()
    {
        if (isTowerPlaced)
        {
            // 타워 제거 및 상태 업데이트
            TowerManager.Instance.RemoveTower(this);
            isTowerPlaced = false;
            UpdateVisual();
        }
    }

    /// <summary>
    /// 그리드의 타워를 업그레이드하는 함수
    /// </summary>
    public void UpgradeTower()
    {
        if (isTowerPlaced)
        {
            TowerManager.Instance.UpgradeTower(this);
        }
    }

    /// <summary>
    /// 그리드의 시각적 상태를 업데이트하는 함수
    /// </summary>
    private void UpdateVisual()
    {
        // 선택 상태에 따라 머티리얼 변경
        if (IsSelected)
        {
            renderer.material = MaterialManager.Instance.outlineMaterial;
        }
        else
        {
            renderer.material = originMaterial;
        }
    }

    /// <summary>
    /// 마우스가 그리드 위에 들어올 때 호출되는 함수
    /// </summary>
    private void OnMouseEnter()
    {
        // 선택되지 않은 상태에서만 호버 효과 적용
        if (!IsSelected)
        {
            renderer.material = MaterialManager.Instance.outlineMaterial;
        }
    }
    
    /// <summary>
    /// 마우스가 그리드에서 나갈 때 호출되는 함수
    /// </summary>
    private void OnMouseExit()
    {
        // 선택되지 않은 상태에서만 시각적 상태 업데이트
        if (!IsSelected)
        {
            UpdateVisual();
        }
    }
}