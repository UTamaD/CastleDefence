using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SceneSingleton<GameManager>
{
    private const string TITLE_SCENE_NAME = "Title";

    public bool isGameOver = false;


    public void Start()
    {
        isGameOver = false;
    }

    /// <summary>
    /// 게임 오버 처리를 수행하는 함수
    /// </summary>
    public void GameOver()
    {
        isGameOver = true;
        UIManager.Instance.ShowGameOverPanel();
        SoundManager.Instance.PlaySFX("GameOver");
        SoundManager.Instance.PlayMusic(SoundManager.Instance.gameOverMusic);
    }

    /// <summary>
    /// 게임을 재시작하는 함수
    /// </summary>
    public void RestartGame()
    {
        Debug.Log("Restarting game");
        UIManager.Instance.HideGameOverPanel();
        isGameOver = false;
        Time.timeScale = 1f;
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartCoroutine(InitializeAfterSceneLoad());
    }
    
    /// <summary>
    /// 씬 로드 후 초기화 작업을 수행하는 코루틴
    /// </summary>
    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return null; // 한 프레임 대기
        Debug.Log("Initializing after scene load");
        FindObjectOfType<PhaseStateMachine>()?.InitializeStateMachine();
    }

    /// <summary>
    /// 타이틀 화면으로 이동하는 함수
    /// </summary>
    public void GoToTitle()
    {
        UIManager.Instance.HideGameOverPanel();
        SceneManager.LoadScene(TITLE_SCENE_NAME);
    }
}