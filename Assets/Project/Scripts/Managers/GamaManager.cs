using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SceneSingleton<GameManager>
{
    private const string TITLE_SCENE_NAME = "Title"; 

    public void GameOver()
    {
        UIManager.Instance.ShowGameOverPanel();
        SoundManager.Instance.PlaySFX("GameOver"); 
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game");
        UIManager.Instance.HideGameOverPanel();
        Time.timeScale = 1f;
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartCoroutine(InitializeAfterSceneLoad());
    }
    
    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return null; // 한 프레임 대기
        Debug.Log("Initializing after scene load");
        FindObjectOfType<PhaseStateMachine>()?.InitializeStateMachine();
    }

    public void GoToTitle()
    {
        UIManager.Instance.HideGameOverPanel();
        SceneManager.LoadScene(TITLE_SCENE_NAME);
    }
}