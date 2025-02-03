using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    public static int selectedDifficulty = 1; // 기본값: 1성

    public void SetDifficulty(int difficulty)
    {
        selectedDifficulty = difficulty;
        Debug.Log("선택한 난이도: " + selectedDifficulty);
        SceneManager.LoadScene("GameScene"); // 게임 씬으로 이동
    }
}
