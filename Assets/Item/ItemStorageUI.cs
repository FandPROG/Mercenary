using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemStorageUI : MonoBehaviour
{
    public GameObject itemStoragePanel; // 창고 패널
    public GameObject itemListPanel; // 아이템 리스트 패널
    public GameObject itemDetailPanel; // 아이템 상세 정보 패널
    public GameObject useItemPanel; // 소모품 사용 패널
    public Button backButton, useButton, closeUseItemButton;
    public Transform itemGrid; // 아이템이 배치될 그리드
    public GameObject itemSlotPrefab; // 아이템 슬롯 프리팹
    public ScrollRect itemScrollRect; // 스크롤 기능
    public Transform mercenaryListPanel; // 용병 리스트 패널
    public GameObject mercenarySlotPrefab; // 용병 슬롯 프리팹

    public Image itemIcon;
    public TextMeshProUGUI itemName, itemDescription;
    
    private BaseItem selectedItem; // 현재 선택된 아이템

    private void Start()
    {
        backButton.onClick.AddListener(CloseItemStorage);
        closeUseItemButton.onClick.AddListener(CloseUseItemPanel);
        useButton.onClick.AddListener(OpenUseItemPanel);

        itemStoragePanel.SetActive(false);
        itemDetailPanel.SetActive(false);
        useItemPanel.SetActive(false);
    }

    // 창고 UI 열기
    public void OpenItemStorage()
    {
        itemStoragePanel.SetActive(true);
        PopulateItemList();
    }

    // 창고 UI 닫기
    private void CloseItemStorage()
    {
        itemStoragePanel.SetActive(false);
    }

    // 아이템 리스트 채우기
    private void PopulateItemList()
    {
        Debug.Log("아이템 리스트를 업데이트합니다...");

        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);
        }

        Dictionary<int, int> ownedItems = Inventory.Instance.GetAllOwnedItems();
        Debug.Log($"현재 보유한 아이템 개수: {ownedItems.Count}");

        foreach (var itemEntry in ownedItems)
        {
            Debug.Log($"아이템 로드: ID {itemEntry.Key}, 개수 {itemEntry.Value}");

            BaseItem item = ItemDatabase.GetItemByID(itemEntry.Key);
            if (item == null)
            {
                Debug.LogError($"아이템 데이터베이스에서 ID {itemEntry.Key} 아이템을 찾을 수 없습니다.");
                continue;
            }

            GameObject slot = Instantiate(itemSlotPrefab, itemGrid);
            Image icon = slot.transform.Find("ItemIcon").GetComponent<Image>();
            TextMeshProUGUI quantityText = slot.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>();
            Button button = slot.GetComponent<Button>();

            icon.sprite = item.icon;
            quantityText.text = $"{itemEntry.Value}개";

            button.onClick.AddListener(() => ShowItemDetails(item, itemEntry.Value));
        }

        Debug.Log("아이템 리스트 업데이트 완료!");
    }


    // 아이템 상세 정보 표시
    private void ShowItemDetails(BaseItem item, int quantity)
    {
        selectedItem = item;
        itemDetailPanel.SetActive(true);

        itemIcon.sprite = item.icon;
        itemName.text = item.name;
        itemDescription.text = $"{item.description}\n보유량: {quantity}개";

        // 소모품일 경우 사용 버튼 활성화
        useButton.gameObject.SetActive(item is ConsumableItem);
    }

    // 소모품 사용 패널 열기
    private void OpenUseItemPanel()
    {
        if (!(selectedItem is ConsumableItem)) return;

        useItemPanel.SetActive(true);
        PopulateMercenaryList();
    }

    // 소모품 사용 패널 닫기
    private void CloseUseItemPanel()
    {
        useItemPanel.SetActive(false);
    }

    // 용병 리스트 채우기 (소모품 사용)
    private void PopulateMercenaryList()
    {
        foreach (Transform child in mercenaryListPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (var merc in LoadManager.mercenaryList)
        {
            GameObject slot = Instantiate(mercenarySlotPrefab, mercenaryListPanel);
            Image mercImage = slot.transform.Find("MercenaryImage").GetComponent<Image>();
            TextMeshProUGUI mercName = slot.transform.Find("MercenaryName").GetComponent<TextMeshProUGUI>();
            Button useButton = slot.transform.Find("UseButton").GetComponent<Button>();

            mercImage.sprite = merc.appearance;
            mercName.text = merc.name;

            useButton.onClick.AddListener(() => UseItemOnMercenary(merc));
        }
    }

    // 아이템 사용 로직
    private void UseItemOnMercenary(Mercenary mercenary)
    {
        if (!(selectedItem is ConsumableItem consumable)) return;

        mercenary.ApplyStatBoost(consumable.increaseStats);
        Inventory.Instance.RemoveItem(selectedItem.id, 1);

        Debug.Log($" {selectedItem.name}을(를) {mercenary.name}에게 사용!");
        PopulateItemList(); // UI 갱신
        CloseUseItemPanel();
    }
}
