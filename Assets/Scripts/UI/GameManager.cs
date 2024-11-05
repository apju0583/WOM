using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Talk")]
    // public TalkManager talkManager;
    public Animator talkPanel;
    public TypeEffect talk;
    public GameObject shopButtons;
    public GameObject upgradeButton;
    public GameObject scanObject;
    public int talkIndex;

    [Header("# Quest")]
    // public QuestManager questManager;
    public GameObject questPanel;

    [Header("# Inventory")]
    [SerializeField] Transform inventory;
    [SerializeField] ItemBar itemBar;
    [SerializeField] GameObject itemDesc;
    [SerializeField] Slot[] slots;
    [SerializeField] CanvasScaler scaler;
    bool inventoryShow;

    [Header("# Shop")]
    public List<Item> item;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject[] shopList;
    [SerializeField] GameObject sellPanel;
    [SerializeField] GameObject[] sellList;

    [Header("# Upgrade")]
    [SerializeField] GameObject upgradePanel;
    [SerializeField] GameObject[] upgradeList;

    [Header("# Map")]
    [SerializeField] GameObject minimap;
    [SerializeField] Animator[] areaAnims;

    [Header("# ETC")]
    public Player player;
    public GameObject[] hps;
    public int itemId;
    public bool isAction;
    // public static GameObject[] dontDestroy;
    bool mapShow;
    [SerializeField] SceneAsset[] scenes;
    [SerializeField] GameObject selectPanel;
    [SerializeField] GameObject selectPanel2;
    [SerializeField] GameObject selectPanel3;

    void Awake() { // 시작되면 각종 창들 비활성화
        // instance = this;
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) {
                Destroy(this.gameObject);
            }
        }

        // 아이템 설명창 비활성화
        if (itemDesc.activeInHierarchy) {
            itemDesc.SetActive(false);
        }
        slots = inventory.gameObject.GetComponentsInChildren<Slot>();

        // 인벤토리창 비활성화
        inventoryShow = false;
        RectTransform inventoryTrans = inventory.gameObject.GetComponent<RectTransform>();
        inventoryTrans.anchoredPosition = new Vector2(2000f, 0f);

        // 퀘스트창 비활성화
        RectTransform questTrans = questPanel.GetComponent<RectTransform>();
        questTrans.anchoredPosition = new Vector2(2000f, 1500f);

        // 상점창 비활성화
        for (int i = 0; i < shopList.Length; i++) {
            Image[] itemImage = shopList[i].GetComponentsInChildren<Image>();
            Text itemName = shopList[i].GetComponentInChildren<Text>();

            itemImage[1].sprite = item[i].itemImage;
            itemName.text = item[i].itemName + "  가격: " + item[i].price + "골드";
        }
        shopPanel.SetActive(false);

        // 판매창 비활성화
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();
        int sellIndex = 0;
        for (; sellIndex < inven.items.Count; sellIndex++) {
            Image[] itemImage = sellList[sellIndex].GetComponentsInChildren<Image>();
            Text itemName = sellList[sellIndex].GetComponentInChildren<Text>();

            itemImage[1].sprite = inven.items[sellIndex].itemImage;
            itemName.text = inven.items[sellIndex].itemName + "  가격: " + inven.items[sellIndex].price * 0.8 + "골드";
        }
        for (; sellIndex < sellList.Length; sellIndex++) {
            sellList[sellIndex].SetActive(false);
        }
        RectTransform sellTrans = sellPanel.GetComponent<RectTransform>();
        sellTrans.anchoredPosition = new Vector2(-2000f, 0f);

        // 업그레이드창 비활성화
        int upgradeIndex = 0;
        for (; upgradeIndex < inven.items.Count; upgradeIndex++) {
            Image[] itemImage = upgradeList[upgradeIndex].GetComponentsInChildren<Image>();
            Text itemName = upgradeList[upgradeIndex].GetComponentInChildren<Text>();

            itemImage[1].sprite = inven.items[upgradeIndex].itemImage;
            itemName.text = inven.items[upgradeIndex].itemName + "  비용: " + inven.items[upgradeIndex].price * 0.1 + "골드";
        }
        for (; upgradeIndex < upgradeList.Length; upgradeIndex++) {
            upgradeList[upgradeIndex].SetActive(false);
        }
        RectTransform upgradeTrans = upgradePanel.GetComponent<RectTransform>();
        upgradeTrans.anchoredPosition = new Vector2(0f, 1500f);

        // 구매선택창 비활성화
        if (selectPanel.activeInHierarchy) {
            selectPanel.SetActive(false);
        }

        // 판매선택창 비활성화
        if (selectPanel2.activeInHierarchy) {
            selectPanel2.SetActive(false);
        }

        // 미니맵 비활성화
        mapShow = false;
        Map map = minimap.GetComponent<Map>();
        map.Init();
        minimap.SetActive(false);

        // 오브젝트들 파괴 안되게 설정
        // for (int i = 0; i < dontDestroy.Length; i++) {
        //     DontDestroyOnLoad(dontDestroy[i]);
        // }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // 아이템 설명창 비활성화
        if (itemDesc.activeInHierarchy) {
            itemDesc.SetActive(false);
        }
        slots = inventory.gameObject.GetComponentsInChildren<Slot>();

        // 인벤토리창 비활성화
        inventoryShow = false;
        RectTransform inventoryTrans = inventory.gameObject.GetComponent<RectTransform>();
        inventoryTrans.anchoredPosition = new Vector2(2000f, 0f);

        // 퀘스트창 비활성화
        RectTransform questTrans = questPanel.GetComponent<RectTransform>();
        questTrans.anchoredPosition = new Vector2(2000f, 1500f);

        // 상점창 비활성화
        for (int i = 0; i < shopList.Length; i++) {
            Image[] itemImage = shopList[i].GetComponentsInChildren<Image>();
            Text itemName = shopList[i].GetComponentInChildren<Text>();

            itemImage[1].sprite = item[i].itemImage;
            itemName.text = item[i].itemName + "  가격: " + item[i].price + "골드";
        }
        shopPanel.SetActive(false);

        // 판매창 비활성화
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();
        int sellIndex = 0;
        for (; sellIndex < inven.items.Count; sellIndex++) {
            Image[] itemImage = sellList[sellIndex].GetComponentsInChildren<Image>();
            Text itemName = sellList[sellIndex].GetComponentInChildren<Text>();

            itemImage[1].sprite = inven.items[sellIndex].itemImage;
            itemName.text = inven.items[sellIndex].itemName + "  가격: " + inven.items[sellIndex].price * 0.8 + "골드";
        }
        for (; sellIndex < sellList.Length; sellIndex++) {
            sellList[sellIndex].SetActive(false);
        }
        RectTransform sellTrans = sellPanel.GetComponent<RectTransform>();
        sellTrans.anchoredPosition = new Vector2(-2000f, 0f);

        // 업그레이드창 비활성화
        int upgradeIndex = 0;
        for (; upgradeIndex < inven.items.Count; upgradeIndex++) {
            Image[] itemImage = upgradeList[upgradeIndex].GetComponentsInChildren<Image>();
            Text itemName = upgradeList[upgradeIndex].GetComponentInChildren<Text>();

            itemImage[1].sprite = inven.items[upgradeIndex].itemImage;
            itemName.text = inven.items[upgradeIndex].itemName + "  비용: " + inven.items[upgradeIndex].price * 0.1 + "골드";
        }
        for (; upgradeIndex < upgradeList.Length; upgradeIndex++) {
            upgradeList[upgradeIndex].SetActive(false);
        }
        RectTransform upgradeTrans = upgradePanel.GetComponent<RectTransform>();
        upgradeTrans.anchoredPosition = new Vector2(0f, 1500f);

        // 구매선택창 비활성화
        if (selectPanel.activeInHierarchy) {
            selectPanel.SetActive(false);
        }

        // 판매선택창 비활성화
        if (selectPanel2.activeInHierarchy) {
            selectPanel2.SetActive(false);
        }

        // 미니맵 비활성화
        mapShow = false;
        Map map = minimap.GetComponent<Map>();
        map.Init();
        minimap.SetActive(false);
    }

    public bool GetInventoryShow() {
        return inventoryShow;
    }

    public void SetInventoryShow(bool inventoryShow) {
        this.inventoryShow = inventoryShow;
    }

    public void ShowInventory() {
        RectTransform trans = inventory.gameObject.GetComponent<RectTransform>();
        trans.anchoredPosition = new Vector2(0f, 0f);
        isAction = true;
    }

    public void HideInventory() {
        RectTransform trans = inventory.gameObject.GetComponent<RectTransform>();
        trans.anchoredPosition = new Vector2(2000f, 0f);
        isAction = false;
    }

    public Transform GetInventory() {
        return inventory;
    }

    public ItemBar GetItemBar() {
        return itemBar;
    }

    public void ShowQuest() {
        RectTransform questTrans = questPanel.GetComponent<RectTransform>();
        questTrans.anchoredPosition = new Vector2(0f, 0f);
        isAction = true;
    }

    public void HideQuest() {
        RectTransform questTrans = questPanel.GetComponent<RectTransform>();
        questTrans.anchoredPosition = new Vector2(2000f, 1500f);
        isAction = false;
    }

    public void ShowDesc(int id) {
        Image[] childSprite = slots[id].gameObject.GetComponentsInChildren<Image>();
        if (childSprite[1].sprite != null && !itemDesc.activeInHierarchy) {
            // 스케일러에 맞게 해상도 설정 -> x: 1920 y: 1080
            float wRatio = Screen.width / scaler.referenceResolution.x;
            float hRatio = Screen.height / scaler.referenceResolution.y;
            float ratio = wRatio * (1f - scaler.matchWidthOrHeight) + hRatio * scaler.matchWidthOrHeight;

            float slotWidth = slots[id].GetComponent<RectTransform>().rect.width * ratio;
            float slotHeight = slots[id].GetComponent<RectTransform>().rect.height * ratio;

            // 툴팁 초기 위치 설정 (슬롯의 우하단)
            RectTransform rt = itemDesc.GetComponent<RectTransform>();
            rt.position = slots[id].GetComponent<RectTransform>().position + new Vector3(slotWidth - 50f, -slotHeight + 50f);
            Vector2 pos = rt.position;

            // 툴팁 크기
            float width = rt.rect.width * ratio;
            float height = rt.rect.height * ratio;

            // 잘린부분 확인
            bool rightTruncated = pos.x + width > Screen.width;
            bool bottomTruncated = pos.y - height < 0f;

            ref bool R = ref rightTruncated;
            ref bool B = ref bottomTruncated;

            if (R && !B) { // 오른쪽이 잘림 -> 슬롯의 좌하단에 표시
                rt.position = new Vector2(pos.x - width - slotWidth + 10f, pos.y);
            }
            else if (!R && B) { // 아래쪽이 잘림 -> 슬롯의 우상단에 표시
                rt.position = new Vector2(pos.x, pos.y + height + slotHeight - 10f);
            }
            else if (R && B) { // 둘 다 잘림 -> 슬롯의 좌상단에 표시
                rt.position = new Vector2(pos.x - width - slotWidth + 10f, pos.y + height + slotHeight - 10f);
            }

            Text itemName = itemDesc.transform.GetChild(0).gameObject.GetComponent<Text>();
            itemName.text = slots[id].item.itemName;
            // itemName.text = slots[id].item.itemName + " (+" + slots[id].item.rank + ")";
            Image itemSprite = itemDesc.transform.GetChild(1).gameObject.GetComponent<Image>();
            itemSprite.sprite = slots[id].item.itemImage;
            Text itemDescription = itemDesc.transform.GetChild(2).gameObject.GetComponent<Text>();
            itemDescription.text = slots[id].item.itemDesc;

            itemDesc.SetActive(true);
            Debug.Log(id + "번 설명창이 켜짐");
        }
    }

    public void HideDesc() {
        if (itemDesc.activeInHierarchy) {
            itemDesc.SetActive(false);
            Debug.Log("설명창이 꺼짐");
        }
    }

    public void ShowShop() {
        shopPanel.SetActive(true);
    }

    public void HideShop() {
        shopPanel.SetActive(false);
    }

    public void ShowSell() {
        RectTransform sellTrans = sellPanel.GetComponent<RectTransform>();
        sellTrans.anchoredPosition = new Vector2(0f, 0f);
    }

    public void HideSell() {
        RectTransform sellTrans = sellPanel.GetComponent<RectTransform>();
        sellTrans.anchoredPosition = new Vector2(-2000f, 0f);
    }

    public void ShowUpagrde() {
        RectTransform upgradeTrans = upgradePanel.GetComponent<RectTransform>();
        upgradeTrans.anchoredPosition = new Vector2(0f, 0f);
    }

    public void HideUpgrade() {
        RectTransform upgradeTrans = upgradePanel.GetComponent<RectTransform>();
        upgradeTrans.anchoredPosition = new Vector2(0f, 1500f);
    }

    public void ShowSelect() {
        selectPanel.SetActive(true);
    }

    public void HideSelect() {
        selectPanel.SetActive(false);
    }

    public void ShowSelect2() {
        selectPanel2.SetActive(true);
    }

    public void HideSelect2() {
        selectPanel2.SetActive(false);
    }

    public void ShowSelect3() {
        selectPanel3.SetActive(true);
    }

    public void HideSelect3() {
        selectPanel3.SetActive(false);
    }

    public void BuyItem() {
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();
        if (inven.gold >= item[itemId].price) {
            inven.UseGold(item[itemId].price);
            inven.AddItem(item[itemId]);
            itemBar.RefreshSlot();
            RefreshSell();
            HideSelect();
            for (int i = 0; i < player.quests.Count; i++) {
                player.questStatus[i] = QuestManager.instance.CheckClear(player.quests[i].questId);
            }
        }
        else {
            Debug.Log("돈이 부족합니다");
            HideSelect();
        }
    }

    public void SellItem() {
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();
        inven.AddGold((int)(item[itemId].price * 0.8));
        inven.RemoveItem(itemId);
        itemBar.RefreshSlot();
        RefreshSell();
        HideSelect2();
    }

    public void Upgrade() {
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();
        if (inven.gold >= (int)(item[itemId].price * 0.1)) {
            inven.UseGold((int)(item[itemId].price * 0.1));
            inven.UpgradeItem(itemId);
            itemBar.RefreshSlot();
            RefreshUpgrade();
            HideSelect3();
        }
        else {
            Debug.Log("돈이 부족합니다");
            HideSelect3();
        }
    }

    public void RefreshSell() {
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();
        for (int i = 0; i < sellList.Length; i++) {
            sellList[i].SetActive(true);
        }
        int sellIndex = 0;
        for (; sellIndex < inven.items.Count; sellIndex++) {
            Image[] itemImage = sellList[sellIndex].GetComponentsInChildren<Image>();
            Text itemName = sellList[sellIndex].GetComponentInChildren<Text>();

            itemImage[1].sprite = inven.items[sellIndex].itemImage;
            itemName.text = inven.items[sellIndex].itemName + "  가격: " + inven.items[sellIndex].price * 0.8 + "골드";
        }
        for (; sellIndex < sellList.Length; sellIndex++) {
            sellList[sellIndex].SetActive(false);
        }
    }

    public void RefreshUpgrade() {
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();
        for (int i = 0; i < upgradeList.Length; i++) {
            upgradeList[i].SetActive(true);
        }
        int upgradeIndex = 0;
        for (; upgradeIndex < inven.items.Count; upgradeIndex++) {
            Image[] itemImage = upgradeList[upgradeIndex].GetComponentsInChildren<Image>();
            Text itemName = upgradeList[upgradeIndex].GetComponentInChildren<Text>();

            itemImage[1].sprite = inven.items[upgradeIndex].itemImage;
            itemName.text = inven.items[upgradeIndex].itemName + "  비용: " + inven.items[upgradeIndex].price * 0.1 + "골드";
        }
        for (; upgradeIndex < upgradeList.Length; upgradeIndex++) {
            upgradeList[upgradeIndex].SetActive(false);
        }
    }

    public bool GetMapShow() {
        return mapShow;
    }

    public void SetMapShow(bool mapShow) {
        this.mapShow = mapShow;
    }

    public void ShowMap() {
        minimap.SetActive(true);
        isAction = true;
    }

    public void HideMap() {
        minimap.SetActive(false);
        isAction = false;
    }

    public void SetHP(int hp) {
        int i = 0;
        for (; i < hp; i++) {
            hps[i].SetActive(true);
        }
        for (; i < hps.Length; i++) {
            hps[i].SetActive(false);
        }
    }

    public void Action(GameObject scanObj) {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData);

        talkPanel.SetBool("isShow", isAction);
    }

    void Talk(ObjData data) {
        string talkData = "";

        if (talk.isAnim) {
            talk.SetMsg("");
            return;
        }
        else {
            if (data.isNPC && data.type == ObjData.NPCType.quest && (data.id % 1000 == 1 || data.id % 1000 == 4)) {
                if (QuestManager.instance.CheckClear(data.questId[data.questIndex])) {
                    data.id += 1;
                }
            }
            talkData = TalkManager.instance.GetTalk(data.id, talkIndex);
        }

        if (talkData == null) {
            if (data.type == ObjData.NPCType.quest) {
                switch (data.id % 1000) {
                    case 0:
                    case 3:
                        QuestManager.instance.Accept(data.questId[data.questIndex]);
                        if (QuestManager.instance.CheckClear(data.questId[data.questIndex])) {
                            data.id += 2;
                            break;
                        }
                        data.id += 1;
                        break;
                    case 2:
                    case 5:
                        QuestManager.instance.QuestClear(data.questId[data.questIndex]);
                        data.id += 1;
                        data.questIndex += 1;
                        if (data.questIndex == data.questId.Length) {
                            data.type = ObjData.NPCType.normal;
                        }
                        break;
                    default:
                        break;
                }
            }
            talkIndex = 0;
            isAction = false;
            return;
        }

        if (data.isNPC) {
            switch (data.type) {
                case ObjData.NPCType.merchant:
                    shopButtons.SetActive(true);
                    upgradeButton.SetActive(false);
                    break;
                case ObjData.NPCType.blacksmith:
                    shopButtons.SetActive(false);
                    upgradeButton.SetActive(true);
                    break;
                case ObjData.NPCType.quest:
                case ObjData.NPCType.normal:
                    shopButtons.SetActive(false);
                    upgradeButton.SetActive(false);
                    break;
            }
            talk.SetMsg(talkData);
        }

        isAction = true;
        talkIndex += 1;
    }
}
