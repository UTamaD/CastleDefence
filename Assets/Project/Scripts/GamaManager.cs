using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SceneSingleton<GameManager>
{
    private const string TITLE_SCENE_NAME = "TitleScene"; // 타이틀 씬의 이름을 적절히 변경하세요

    public void GameOver()
    {
        UIManager.Instance.ShowGameOverPanel();
        SoundManager.Instance.PlaySFX("GameOver"); // 게임 오버 사운드 추가
    }

    public void RestartGame()
    {
        UIManager.Instance.HideGameOverPanel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SoundManager.Instance.PlaySFX("Restart"); // 재시작 사운드 추가
    }

    public void GoToTitle()
    {
        UIManager.Instance.HideGameOverPanel();
        SceneManager.LoadScene(TITLE_SCENE_NAME);
        SoundManager.Instance.PlaySFX("GoToTitle"); // 타이틀로 이동 사운드 추가
    }
}