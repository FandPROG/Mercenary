using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 싱글톤 인스턴스

    public TextMeshProUGUI dayGoldText; // UI 텍스트 연결

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UpdateDayGoldText();
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬이 로드될 때 UI 다시 연결
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 이동 후 새롭게 UI를 찾아서 연결
        dayGoldText = GameObject.Find("DayGoldText")?.GetComponent<TextMeshProUGUI>();
        UpdateDayGoldText();
    }

    public void NextDay()
    {
        FadeManager.Instance.StartFade(() => 
        {
            LoadManager.Day++;
            LoadManager.Gold += 50;
            UpdateDayGoldText();
        });
    }

    public void UpdateDayGoldText()
    {
        if (dayGoldText != null)
        {
            dayGoldText.text = $"  Day {LoadManager.Day}\n 골드: {LoadManager.Gold}";
        }
    }

    public void MercenaryManagement()
    {
        SceneManager.LoadScene("MercenaryManagement");
    }
}