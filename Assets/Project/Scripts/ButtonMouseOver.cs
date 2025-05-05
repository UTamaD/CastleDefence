using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonMouseOver : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private string soundName = "ButtonMouseOver";  
    
    /// <summary>
    /// 마우스가 버튼 위에 들어올 때 호출되는 함수
    /// </summary>
    /// <param name="eventData">포인터 이벤트 데이터</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }
    
    /// <summary>
    /// 버튼 호버 사운드를 재생하는 함수
    /// </summary>
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