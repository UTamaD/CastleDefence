using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonMouseOver : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private string soundName = "ButtonMouseOver";  
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }
    
    private void PlayHoverSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(soundName);
        }
        else
        {
            Debug.LogWarning("SoundManager.Instance is not assigned or initialized!");
        }
    }
}