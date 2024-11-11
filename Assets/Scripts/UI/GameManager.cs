using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Talk")]
    public Animator talkPanel;
    public TypeEffect talk;
    public GameObject shopButtons;
    public GameObject upgradeButton;
    public GameObject scanObject;
    public int talkIndex;

    [Header("# Quest")]
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
    public GameObject[] halfHps;
    public Text playerInfo;
    public int itemId;
    public bool isAction;
    bool mapShow;
    [SerializeField] GameObject selectPanel;
    [SerializeField] GameObject selectPanel2;
    [SerializeField] GameObject selectPanel3;

    void Awake() // 시작되면 각종 창들 비활성화
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 

        else
        {
            if (instance != this) 
            {
                Destroy(this.gameObject);
            }
        }

        player = FindObjectOfType<Player>(); // player를 찾는 코드 추가
        if (player == null) 
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다");
        }

        if (itemDesc.activeInHierarchy) // 아이템 설명창 비활성화
        {
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
        for (int i = 0; i < shopList.Length; i++) 
        {
            Image[] itemImage = shopList[i].GetComponentsInChildren<Image>();
            Text itemName = shopList[i].GetComponentInChildren<Text>();

            itemImage[1].sprite = item[i].itemImage;
            itemName.text = item[i].itemName + "  가격: " + item[i].price + "골드";
        }

        shopPanel.SetActive(false);

        // 판매창 비활성화
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();
        int sellIndex = 0;
        for (; sellIndex < inven.items.Count; sellIndex++) 
        {
            Image[] itemImage = sellList[sellIndex].GetComponentsInChildren<Image>();
            Text itemName = sellList[sellIndex].GetComponentInChildren<Text>();

            itemImage[1].sprite = inven.items[sellIndex].itemImage;
            itemName.text = inven.items[sellIndex].itemName + "  가격: " + inven.items[sellIndex].price * 0.8 + "골드";
        }

        for (; sellIndex < sellList.Length; sellIndex++) 
        {
            sellList[sellIndex].SetActive(false);
        }

        RectTransform sellTrans = sellPanel.GetComponent<RectTransform>();
        sellTrans.anchoredPosition = new Vector2(-2000f, 0f);

        // 업그레이드창 비활성화
        int upgradeIndex = 0;
        for (; upgradeIndex < inven.items.Count; upgradeIndex++) 
        {
            Image[] itemImage = upgradeList[upgradeIndex].GetComponentsInChildren<Image>();
            Text itemName = upgradeList[upgradeIndex].GetComponentInChildren<Text>();

            itemImage[1].sprite = inven.items[upgradeIndex].itemImage;
            itemName.text = inven.items[upgradeIndex].itemName + "  비용: " + inven.items[upgradeIndex].price * 0.1 + "골드";
        }

        for (; upgradeIndex < upgradeList.Length; upgradeIndex++) 
        {
            upgradeList[upgradeIndex].SetActive(false);
        }

        RectTransform upgradeTrans = upgradePanel.GetComponent<RectTransform>();
        upgradeTrans.anchoredPosition = new Vector2(0f, 1500f);

        if (selectPanel.activeInHierarchy) // 구매선택창 비활성화
        {
            selectPanel.SetActive(false);
        }

        if (selectPanel2.activeInHierarchy) // 판매선택창 비활성화
        {
            selectPanel2.SetActive(false);
        }

        // 미니맵 비활성화
        mapShow = false;
        Map map = minimap.GetComponent<Map>();
        map.Init();
        minimap.SetActive(false);

        for (int i = 0; i < QuestManager.instance.quests.Length; i++) 
        {
            player.clearStatus.Add(false);
        }
    }

    public void SaveGame()
    {
        if (player != null && player.inventory != null)
        {
            SaveData data = new SaveData();

            // 플레이어 위치 데이터 저장
            data.playerPositionX = player.transform.position.x;
            data.playerPositionY = player.transform.position.y;
            data.playerPositionZ = player.transform.position.z;
            data.gold = player.inventory.gold;

            // 인벤토리 데이터 저장
            data.inventoryItems = new List<string>(player.inventory.names);
            data.inventoryCounts = new List<int>(player.inventory.itemCounts);

            // 퀘스트 데이터 저장
            if (player.quest != null)
            {
                data.questId = player.quest.questId;
                data.questStatus = player.questStatus;
            }

            // 직렬화 및 저장
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/saveData.dat");
            formatter.Serialize(file, data);
            file.Close();

            Debug.Log("저장 완료");
        }

        else
        {
            Debug.LogError("플레이어 혹은 인벤토리 데이터 오류");
        }
    }


    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/saveData.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveData.dat", FileMode.Open);
            SaveData data = (SaveData)formatter.Deserialize(file);
            file.Close();

            // 플레이어 위치 데이터 불러오기
            player.transform.position = new Vector3(data.playerPositionX, data.playerPositionY, data.playerPositionZ);
            player.inventory.gold = data.gold;

            // 인벤토리 데이터 불러오기
            player.inventory.names = new List<string>(data.inventoryItems);
            player.inventory.itemCounts = new List<int>(data.inventoryCounts);
            player.inventory.FreshSlot();

            // 퀘스트 데이터 불러오기
            if (data.questId >= 0 && data.questId < QuestManager.instance.quests.Length)
            {
                player.quest = QuestManager.instance.quests[data.questId];
                player.questStatus = data.questStatus;
                QuestManager.instance.Refresh();
            }

            Debug.Log("게임 불러오기 완료");
        }

        else
        {
            Debug.LogError("세이브 파일이 없습니다");
        }
    }

    void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        if (itemDesc.activeInHierarchy) // 아이템 설명창 비활성화
        {
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
        for (int i = 0; i < shopList.Length; i++) 
        {
            Image[] itemImage = shopList[i].GetComponentsInChildren<Image>();
            Text itemName = shopList[i].GetComponentInChildren<Text>();

            itemImage[1].sprite = item[i].itemImage;
            itemName.text = item[i].itemName + "  가격: " + item[i].price + "골드";
        }
        shopPanel.SetActive(false);

        // 판매창 비활성화
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();
        int sellIndex = 0;
        for (; sellIndex < inven.items.Count; sellIndex++) 
        {
            Image[] itemImage = sellList[sellIndex].GetComponentsInChildren<Image>();
            Text itemName = sellList[sellIndex].GetComponentInChildren<Text>();
            itemImage[1].sprite = inven.items[sellIndex].itemImage;
            itemName.text = inven.items[sellIndex].itemName + "  가격: " + inven.items[sellIndex].price * 0.8 + "골드";
        }

        for (; sellIndex < sellList.Length; sellIndex++) 
        {
            sellList[sellIndex].SetActive(false);
        }
        RectTransform sellTrans = sellPanel.GetComponent<RectTransform>();
        sellTrans.anchoredPosition = new Vector2(-2000f, 0f);

        // 업그레이드창 비활성화
        int upgradeIndex = 0;
        for (; upgradeIndex < inven.items.Count; upgradeIndex++) 
        {
            Image[] itemImage = upgradeList[upgradeIndex].GetComponentsInChildren<Image>();
            Text itemName = upgradeList[upgradeIndex].GetComponentInChildren<Text>();

            itemImage[1].sprite = inven.items[upgradeIndex].itemImage;
            itemName.text = inven.items[upgradeIndex].itemName + "  비용: " + inven.items[upgradeIndex].price * 0.1 + "골드";
        }

        for (; upgradeIndex < upgradeList.Length; upgradeIndex++) 
        {
            upgradeList[upgradeIndex].SetActive(false);
        }
        RectTransform upgradeTrans = upgradePanel.GetComponent<RectTransform>();
        upgradeTrans.anchoredPosition = new Vector2(0f, 1500f);

        if (selectPanel.activeInHierarchy) // 구매선택창 비활성화
        {
            selectPanel.SetActive(false);
        }

        if (selectPanel2.activeInHierarchy) // 판매선택창 비활성화
        {
            selectPanel2.SetActive(false);
        }

        // 미니맵 비활성화
        mapShow = false;
        Map map = minimap.GetComponent<Map>();
        map.Init();
        minimap.SetActive(false);
    }

    public bool GetInventoryShow() 
    {
        return inventoryShow;
    }

    public void SetInventoryShow(bool inventoryShow) 
    {
        this.inventoryShow = inventoryShow;
    }

    public void ShowInventory() 
    {
        RectTransform trans = inventory.gameObject.GetComponent<RectTransform>();
        trans.anchoredPosition = new Vector2(0f, 0f);
        isAction = true;
    }

    public void HideInventory() 
    {
        RectTransform trans = inventory.gameObject.GetComponent<RectTransform>();
        trans.anchoredPosition = new Vector2(2000f, 0f);
        isAction = false;
    }

    public Transform GetInventory() 
    {
        return inventory;
    }

    public ItemBar GetItemBar() 
    {
        return itemBar;
    }

    public void ShowQuest() 
    {
        RectTransform questTrans = questPanel.GetComponent<RectTransform>();
        questTrans.anchoredPosition = new Vector2(0f, 0f);
        isAction = true;
    }

    public void HideQuest() 
    {
        RectTransform questTrans = questPanel.GetComponent<RectTransform>();
        questTrans.anchoredPosition = new Vector2(2000f, 1500f);
        isAction = false;
    }

    public void ShowDesc(int id) 
    {
        Image[] childSprite = slots[id].gameObject.GetComponentsInChildren<Image>();

        if (childSprite[1].sprite != null && !itemDesc.activeInHierarchy) 
        {
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

            if (R && !B) // 오른쪽이 잘림 -> 슬롯의 좌하단에 표시
            {
                rt.position = new Vector2(pos.x - width - slotWidth + 10f, pos.y);
            }

            else if (!R && B) // 아래쪽이 잘림 -> 슬롯의 우상단에 표시
            { 
                rt.position = new Vector2(pos.x, pos.y + height + slotHeight - 10f);
            }

            else if (R && B) // 둘 다 잘림 -> 슬롯의 좌상단에 표시
            { 
                rt.position = new Vector2(pos.x - width - slotWidth + 10f, pos.y + height + slotHeight - 10f);
            }

            Text itemName = itemDesc.transform.GetChild(0).gameObject.GetComponent<Text>();
            itemName.text = slots[id].item.itemName;
            Image itemSprite = itemDesc.transform.GetChild(1).gameObject.GetComponent<Image>();
            itemSprite.sprite = slots[id].item.itemImage;
            Text itemDescription = itemDesc.transform.GetChild(2).gameObject.GetComponent<Text>();
            itemDescription.text = slots[id].item.itemDesc;
            itemDesc.SetActive(true);
            Debug.Log(id + "번 설명창이 켜짐");
        }
    }

    public void HideDesc() 
    {
        if (itemDesc.activeInHierarchy) 
        {
            itemDesc.SetActive(false);
            Debug.Log("설명창이 꺼짐");
        }
    }

    public void UseItem(int id) {
        Image[] childSprite = slots[id].gameObject.GetComponentsInChildren<Image>();
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();

        if (childSprite[1].sprite != null) {
            if (slots[id].item.type == Item.ItemType.use) {
                HideDesc();
                player.RestoreHP(inven.itemAbility[id]);
                SetHP(player.hp);
                inven.RemoveItem(id);
                itemBar.RefreshSlot();
                SoundManager.instance.SFXPlay(14);
                Debug.Log("체력이 회복되었습니다");
            }
        }
    }

    public void ShowShop() 
    {
        shopPanel.SetActive(true);
    }

    public void HideShop() 
    {
        shopPanel.SetActive(false);
    }

    public void ShowSell() 
    {
        RectTransform sellTrans = sellPanel.GetComponent<RectTransform>();
        sellTrans.anchoredPosition = new Vector2(0f, 0f);
    }

    public void HideSell() 
    {
        RectTransform sellTrans = sellPanel.GetComponent<RectTransform>();
        sellTrans.anchoredPosition = new Vector2(-2000f, 0f);
    }

    public void ShowUpagrde() 
    {
        RectTransform upgradeTrans = upgradePanel.GetComponent<RectTransform>();
        upgradeTrans.anchoredPosition = new Vector2(0f, 0f);
    }

    public void HideUpgrade() 
    {
        RectTransform upgradeTrans = upgradePanel.GetComponent<RectTransform>();
        upgradeTrans.anchoredPosition = new Vector2(0f, 1500f);
    }

    public void ShowSelect() 
    {
        selectPanel.SetActive(true);
    }

    public void HideSelect() 
    {
        selectPanel.SetActive(false);
    }

    public void ShowSelect2() 
    {
        selectPanel2.SetActive(true);
    }

    public void HideSelect2() 
    {
        selectPanel2.SetActive(false);
    }

    public void ShowSelect3() 
    {
        selectPanel3.SetActive(true);
    }

    public void HideSelect3() 
    {
        selectPanel3.SetActive(false);
    }

    public void BuyItem() 
    {
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();
        if (inven.gold >= item[itemId].price) 
        {
            inven.UseGold(item[itemId].price);
            inven.AddItem(item[itemId]);
            itemBar.RefreshSlot();
            RefreshSell();
            RefreshUpgrade();
            HideSelect();
        }

        else 
        {
            Debug.Log("돈이 부족합니다");
            HideSelect();
        }
    }

    public void SellItem() 
    {
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();
        inven.AddGold((int)(item[itemId].price * 0.8));
        inven.RemoveItem(itemId);
        itemBar.RefreshSlot();
        RefreshSell();
        RefreshUpgrade();
        HideSelect2();
    }

    public void Upgrade() 
    {
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();
        if (inven.gold >= (int)(item[itemId].price * 0.1)) 
        {
            inven.UseGold((int)(item[itemId].price * 0.1));
            inven.UpgradeItem(itemId);
            itemBar.RefreshSlot();
            RefreshUpgrade();
            RefreshSell();
            HideSelect3();
        }

        else 
        {
            Debug.Log("돈이 부족합니다");
            HideSelect3();
        }
    }

    public void RefreshSell() 
    {
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();

        for (int i = 0; i < sellList.Length; i++) 
        {
            sellList[i].SetActive(true);
        }

        int sellIndex = 0;
        for (; sellIndex < inven.items.Count; sellIndex++) 
        {
            Image[] itemImage = sellList[sellIndex].GetComponentsInChildren<Image>();
            Text itemName = sellList[sellIndex].GetComponentInChildren<Text>();

            itemImage[1].sprite = inven.items[sellIndex].itemImage;
            itemName.text = inven.items[sellIndex].itemName + "  가격: " + inven.items[sellIndex].price * 0.8 + "골드";
        }

        for (; sellIndex < sellList.Length; sellIndex++) 
        {
            sellList[sellIndex].SetActive(false);
        }
    }

    public void RefreshUpgrade() 
    {
        Inventory inven = inventory.gameObject.GetComponent<Inventory>();

        for (int i = 0; i < upgradeList.Length; i++) 
        {
            upgradeList[i].SetActive(true);
        }

        int upgradeIndex = 0;
        for (; upgradeIndex < inven.items.Count; upgradeIndex++) 
        {
            Image[] itemImage = upgradeList[upgradeIndex].GetComponentsInChildren<Image>();
            Text itemName = upgradeList[upgradeIndex].GetComponentInChildren<Text>();

            itemImage[1].sprite = inven.items[upgradeIndex].itemImage;
            itemName.text = inven.items[upgradeIndex].itemName + "  비용: " + inven.items[upgradeIndex].price * 0.1 + "골드";
        }

        for (; upgradeIndex < upgradeList.Length; upgradeIndex++) 
        {
            upgradeList[upgradeIndex].SetActive(false);
        }
    }

    public bool GetMapShow() 
    {
        return mapShow;
    }

    public void SetMapShow(bool mapShow) 
    {
        this.mapShow = mapShow;
    }

    public void ShowMap() 
    {
        minimap.SetActive(true);
        isAction = true;
    }

    public void HideMap() 
    {
        minimap.SetActive(false);
        isAction = false;
    }

    public void SetHP(int hp) 
    {
        if (hp <= 10) {
            switch (hp) {
                case 0:
                    for (int i = 0; i < 5; i++) {
                        hps[i].SetActive(false);
                    }

                    for (int i = 0; i < 5; i++) {
                        halfHps[i].SetActive(false);
                    }
                    break;

                case 1:
                    for (int i = 0; i < 5; i++) {
                        hps[i].SetActive(false);
                    }

                    halfHps[0].SetActive(true);
                    for (int i = 1; i < 5; i++) {
                        halfHps[i].SetActive(false);
                    }
                    break;

                case 2:
                    hps[0].SetActive(true);
                    for (int i = 1; i < 5; i++) {
                        hps[i].SetActive(false);
                    }

                    for (int i = 0; i < 5; i++) {
                        halfHps[i].SetActive(false);
                    }
                    break;

                case 3:
                    hps[0].SetActive(true);
                    for (int i = 1; i < 5; i++) {
                        hps[i].SetActive(false);
                    }

                    halfHps[0].SetActive(true);
                    halfHps[1].SetActive(true);
                    for (int i = 2; i < 5; i++) {
                        halfHps[i].SetActive(false);
                    }
                    break;

                case 4:
                    hps[0].SetActive(true);
                    hps[1].SetActive(true);
                    for (int i = 2; i < 5; i++) {
                        hps[i].SetActive(false);
                    }

                    for (int i = 0; i < 5; i++) {
                        halfHps[i].SetActive(false);
                    }
                    break;

                case 5:
                    hps[0].SetActive(true);
                    hps[1].SetActive(true);
                    for (int i = 2; i < 5; i++) {
                        hps[i].SetActive(false);
                    }

                    halfHps[0].SetActive(true);
                    halfHps[1].SetActive(true);
                    halfHps[2].SetActive(true);
                    for (int i = 3; i < 5; i++) {
                        halfHps[i].SetActive(false);
                    }
                    break;

                case 6:
                    hps[0].SetActive(true);
                    hps[1].SetActive(true);
                    hps[2].SetActive(true);
                    for (int i = 3; i < 5; i++) {
                        hps[i].SetActive(false);
                    }

                    for (int i = 0; i < 5; i++) {
                        halfHps[i].SetActive(false);
                    }
                    break;

                case 7:
                    hps[0].SetActive(true);
                    hps[1].SetActive(true);
                    hps[2].SetActive(true);
                    for (int i = 3; i < 5; i++) {
                        hps[i].SetActive(false);
                    }

                    halfHps[0].SetActive(true);
                    halfHps[1].SetActive(true);
                    halfHps[2].SetActive(true);
                    halfHps[3].SetActive(true);
                    for (int i = 4; i < 5; i++) {
                        halfHps[i].SetActive(false);
                    }
                    break;

                case 8:
                    hps[0].SetActive(true);
                    hps[1].SetActive(true);
                    hps[2].SetActive(true);
                    hps[3].SetActive(true);
                    hps[4].SetActive(false);

                    for (int i = 0; i < 5; i++) {
                        halfHps[i].SetActive(false);
                    }
                    break;

                case 9:
                    hps[0].SetActive(true);
                    hps[1].SetActive(true);
                    hps[2].SetActive(true);
                    hps[3].SetActive(true);
                    hps[4].SetActive(false);

                    for (int i = 0; i < halfHps.Length; i++) {
                        halfHps[i].SetActive(true);
                    }
                    break;

                case 10:
                    for (int i = 0; i < hps.Length; i++) {
                        hps[i].SetActive(true);
                    }
                    for (int i = 0; i < halfHps.Length; i++) {
                        halfHps[i].SetActive(true);
                    }
                    break;
            }
        }

        else {
            for (int i = 0; i < hps.Length; i++) {
                hps[i].SetActive(true);
            }

            for (int i = 0; i < halfHps.Length; i++) {
                halfHps[i].SetActive(true);
            }
        }
    }

    public void SetPlayerInfoText(int maxhp, int damage) {
        playerInfo.text = "최대 체력: " + maxhp + " / 공격력: " + damage;
    }

    public void Action(GameObject scanObj) 
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        
        if (objData != null)
        {
            bool conversationEnded = Talk(objData);

            if (conversationEnded && objData.itemToGive != null)
            {
                AddItemToInventory(objData.itemToGive);
                objData.itemToGive = null;
                objData.id += 1;
            }
        }

        talkPanel.SetBool("isShow", isAction);
    }

    private void AddItemToInventory(Item item)
    {
        if (player.inventory != null)
        {
            player.inventory.AddItem(item);
            Debug.Log("Item added to inventory: " + item.name);
        }
    }

    private bool Talk(ObjData data)
    {
        string talkData = "";

        if (talk.isAnim)
        {
            talk.SetMsg("");
            return false;
        }

        else
        {
            if (data.isNPC && data.type == ObjData.NPCType.quest)
            {
                if (data.questId[0] == 0 && data.id % 1000 == 0 && player.clearStatus[data.questId[0]] == false && player.quest == null) {
                    data.id += 1;
                }

                else if (data.questId[0] != 0 && player.clearStatus[data.questId[0] - 1] && data.id % 1000 == 0 && player.clearStatus[data.questId[0]] == false && player.quest == null) {
                    data.id += 1;
                }

                if (player.quest != null && player.quest.questId == data.questId[0] && data.id % 1000 == 0)
                {
                    data.id += 2;
                }

                if (player.clearStatus[data.questId[0]] && data.id % 1000 == 0)
                {
                    data.id += 4;
                }
            }

            if (data.isNPC && data.type == ObjData.NPCType.quest && data.id % 1000 == 2)
            {
                if (QuestManager.instance.CheckClear(data.questId[data.questIndex]))
                {
                    data.id += 1;
                }
            }
            talkData = TalkManager.instance.GetTalk(data.id, talkIndex);
        }

        if (talkData == null)
        {
            if (data.type == ObjData.NPCType.quest)
            {
                switch (data.id % 1000)
                {
                    case 1:
                        QuestManager.instance.Accept(data.questId[data.questIndex]);
                        if (QuestManager.instance.CheckClear(data.questId[data.questIndex]))
                        {
                            data.id += 2;
                            break;
                        }
                        data.id += 1;
                        break;

                    case 3:
                        QuestManager.instance.QuestClear(data.questId[data.questIndex]);
                        data.id += 1;
                        data.questIndex += 1;
                        if (data.questIndex == data.questId.Length)
                        {
                            data.type = ObjData.NPCType.normal;
                        }
                        break;

                    default:
                        break;
                }
            }
            talkIndex = 0;
            isAction = false;
            return true;
        }

        if (data.isNPC)
        {
            switch (data.type)
            {
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
        return false;
    }
}