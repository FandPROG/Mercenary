using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MercenaryUI : MonoBehaviour
{
    public Button summonStreetButton;
    public Button summonGuildButton;
    public Button summonKnightButton;

    public GameObject hirePopupPanel;
    public TextMeshProUGUI mercenaryInfoText;
    public Image mercenaryImage;
    public Button closeButton;

    private void Start()
    {
        summonStreetButton.onClick.AddListener(() => HireAndDisplay(MercenaryManager.Instance.HireStreetMercenary()));
        summonGuildButton.onClick.AddListener(() => HireAndDisplay(MercenaryManager.Instance.HireGuildMercenary()));
        summonKnightButton.onClick.AddListener(() => HireAndDisplay(MercenaryManager.Instance.HireKnightMercenary()));

        closeButton.onClick.AddListener(ClosePopup);
        hirePopupPanel.SetActive(false);
    }

    private void HireAndDisplay(Mercenary mercenary)
    {
        if (mercenary == null)
        {
            Debug.LogError("고용된 용병이 없습니다! (HireAndDisplay()에서 null 반환)");
            return;
        }

        if (mercenaryInfoText == null || mercenaryImage == null)
        {
            Debug.LogError("MercenaryUI의 UI 요소가 제대로 설정되지 않았습니다!");
            return;
        }

        mercenaryInfoText.text = $"{mercenary.name} ({mercenary.starRating}성)\n직업: {mercenary.job}\n속성: {mercenary.element}";

        if (mercenary.appearance != null)
        {
            mercenaryImage.sprite = mercenary.appearance;
        }
        else
        {
            Debug.LogWarning($"{mercenary.name}의 appearance가 null입니다! 기본 이미지 적용");
            mercenaryImage.sprite = Resources.Load<Sprite>("DefaultSprite");
        }

        hirePopupPanel.SetActive(true);
    }

    private void ClosePopup()
    {
        hirePopupPanel.SetActive(false);
    }
}
