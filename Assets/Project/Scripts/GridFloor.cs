using UnityEngine;

public class GridFloor : MonoBehaviour
{
    private Material originMaterial;
    private Renderer renderer;
    private bool isTowerPlaced = false;
    public bool IsSelected { get; private set; }

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        originMaterial = renderer.material;
    }

    private void OnMouseDown()
    {
        Select();
    }

    public void Select()
    {
        
        GridFloor previousSelected = UIManager.Instance.GetSelectedGridFloor();
        SoundManager.Instance.PlaySFX("SelectGrid");
        if (previousSelected != null && previousSelected != this)
        {
            previousSelected.Deselect();
        }

        IsSelected = true;
        UpdateVisual();
        UIManager.Instance.ShowTowerOptions(this, isTowerPlaced);
    }

    public void Deselect()
    {
        IsSelected = false;
        UpdateVisual();
    }
    
    

    public void PlaceTower()
    {
        if (!isTowerPlaced)
        {
            TowerManager.Instance.PlaceTower(this);
            isTowerPlaced = true;
            Deselect();
            UpdateVisual();
        }
    }

    public void RemoveTower()
    {
        if (isTowerPlaced)
        {
            TowerManager.Instance.RemoveTower(this);
            isTowerPlaced = false;
            UpdateVisual();
        }
    }

    public void UpgradeTower()
    {
        if (isTowerPlaced)
        {
            TowerManager.Instance.UpgradeTower(this);
        }
    }

    private void UpdateVisual()
    {
        if (IsSelected)
        {
            renderer.material = MaterialManager.Instance.outlineMaterial;
        }
        else
        {
            renderer.material = originMaterial;
        }
    }

    private void OnMouseEnter()
    {
        if (!IsSelected)
        {
            renderer.material = MaterialManager.Instance.outlineMaterial;
        }
    }
    
    private void OnMouseExit()
    {
        if (!IsSelected)
        {
            UpdateVisual();
        }
    }
}