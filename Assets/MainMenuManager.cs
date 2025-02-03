using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("DifficultySelection");  // 게임 씬으로 이동 (게임 씬이 있어야 함)
    }

    public void OpenSettings()
    {
        Debug.Log("설정 버튼 클릭됨");  // 설정 창이 생기면 연결
    }

    public void ShowCredits()
    {
        Debug.Log("제작자 버튼 클릭됨");  // 제작자 정보 창 연결
    }

    public void QuitGame()
    {
        Application.Quit();  // 게임 종료
    }
}
