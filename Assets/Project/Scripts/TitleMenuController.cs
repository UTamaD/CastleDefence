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


    public void OnStartButtonClick()
    {
        mapSelectionUI.SetActive(true); 
    }


    public void OnMapSelected(int index)
    {
        string selectedScene = mapSceneNames[index];
        SceneManager.LoadScene(selectedScene); 
    }

   
    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    // Play hover sound
    public void PlayHoverSound()
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}
