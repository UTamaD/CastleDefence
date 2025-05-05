using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleMenuController : MonoBehaviour
{
    public GameObject mapSelectionUI; 
    public Button[] mapButtons;      
    public string[] mapSceneNames;   
    public Button startButton;        
    public AudioSource audioSource;  
    public AudioClip clickSound;      
    public AudioClip hoverSound;      

    void Start()
    {
        if (mapButtons.Length != mapSceneNames.Length)
        {
            Debug.LogError("The number of buttons and map scene names must match!");
            return;
        }

        AddButtonListeners(startButton, OnStartButtonClick);


        for (int i = 0; i < mapButtons.Length; i++)
        {
            int index = i; 
            AddButtonListeners(mapButtons[i], () => OnMapSelected(index));
        }


        mapSelectionUI.SetActive(false);
    }

    /// <summary>
    /// 버튼에 클릭 및 호버 이벤트 리스너를 추가하는 함수
    /// </summary>
    /// <param name="button">이벤트를 추가할 버튼</param>
    /// <param name="action">클릭 시 실행할 액션</param>
    void AddButtonListeners(Button button, UnityEngine.Events.UnityAction action)
    {
        button.onClick.AddListener(action);
        button.onClick.AddListener(() => PlayClickSound());
        
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => PlayHoverSound());

        trigger.triggers.Add(entry);
    }

    /// <summary>
    /// 시작 버튼 클릭 시 맵 선택 UI를 표시하는 함수
    /// </summary>
    public void OnStartButtonClick()
    {
        mapSelectionUI.SetActive(true); 
    }

    /// <summary>
    /// 맵 선택 시 해당 맵 씬으로 이동하는 함수
    /// </summary>
    /// <param name="index">선택한 맵의 인덱스</param>
    public void OnMapSelected(int index)
    {
        string selectedScene = mapSceneNames[index];
        SceneManager.LoadScene(selectedScene); 
    }

    /// <summary>
    /// 버튼 클릭 사운드를 재생하는 함수
    /// </summary>
    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    /// <summary>
    /// 버튼 호버 사운드를 재생하는 함수
    /// </summary>
    public void PlayHoverSound()
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}
