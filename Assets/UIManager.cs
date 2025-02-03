using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject optionsPanel;
    public static UIManager Instance;

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
    public void ToggleOptions()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    public GameObject calendarPanel;

    public void ToggleCalendar()
    {
        calendarPanel.SetActive(!calendarPanel.activeSelf);
    }

    public GameObject RecruitPanel;

    public void ToggleRecruit()
    {
        RecruitPanel.SetActive(!RecruitPanel.activeSelf);
    }
}