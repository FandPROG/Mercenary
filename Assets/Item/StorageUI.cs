using UnityEngine;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour
{
    public GameObject storagePanel; // 창고 패널
    public Button storageButton; // 창고 열기 버튼
    public Button closeStorageButton; // 창고 닫기 버튼
    public Button itemButton; // 아이템 패널 열기 버튼
    public Button equipmentButton; // 장비 패널 열기 버튼
    public GameObject itemStoragePanel; // 아이템 창고 패널
    public GameObject equipmentStoragePanel; // 장비 창고 패널

    private void Start()
    {
        // 창고 버튼 설정
        storageButton.onClick.AddListener(OpenStoragePanel);
        closeStorageButton.onClick.AddListener(CloseStoragePanel);

        // 아이템/장비 버튼 설정
        itemButton.onClick.AddListener(OpenItemStorage);
        equipmentButton.onClick.AddListener(OpenEquipmentStorage);

        // 처음에는 창고 패널과 모든 서브 패널을 비활성화
        storagePanel.SetActive(false);
        itemStoragePanel.SetActive(false);
        equipmentStoragePanel.SetActive(false);
    }

    // 📌 창고 패널 열기
    private void OpenStoragePanel()
    {
        storagePanel.SetActive(true);
        OpenItemStorage(); // 기본적으로 아이템 패널을 활성화
    }

    // 📌 창고 패널 닫기
    private void CloseStoragePanel()
    {
        storagePanel.SetActive(false);
        itemStoragePanel.SetActive(false);
        equipmentStoragePanel.SetActive(false);
    }

    // 📌 아이템 창고 패널 활성화
    private void OpenItemStorage()
    {
        itemStoragePanel.SetActive(true);
        equipmentStoragePanel.SetActive(false);
    }

    // 📌 장비 창고 패널 활성화
    private void OpenEquipmentStorage()
    {
        itemStoragePanel.SetActive(false);
        equipmentStoragePanel.SetActive(true);
    }
}
