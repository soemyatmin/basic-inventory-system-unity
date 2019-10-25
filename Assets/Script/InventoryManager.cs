using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : SingletonBehaviour<InventoryManager> {

    public GameObject itemPrefab;
    public ScrollRect scrollRect;

    public Text ItemName;
    public Image ItemImage;
    public Text Rank;
    public Text Price;
    public Text Count;
    public Text Desception;
    public Text Increasing;

    public GameObject[] OptionListGO;
    public GameObject[] OptionBtnGO;
   
    public Image[] stars;
    public Sprite fullStar, NullStar;
    public Text Adjustment;

    Inventory[] inventoryItems; // Load from Scriptable Object
    Inventory selectedInv;

    List<Inventory> FullItemList; // Load from Database or dynamic Create
    Buttonbtn activeButton;
    // no need if no improvement
    public override void Awake() {
        base.Awake();
    }

    void Start() {
        inventoryItems = Resources.LoadAll<Inventory>("ItemObjects");
        FullItemList = ItemObtainFromDatabase();
        CreateItem();
        if (FullItemList.Count > 0) { ShowItemInfo(FullItemList[0]); }
        ItemInfoSetup();
        activeButton = OptionBtnGO[0].GetComponent<Buttonbtn>();
        activeButton.Click();
    }
    
    private List<Inventory> ItemObtainFromDatabase() {
        List<Inventory> IVlst = new List<Inventory>();
        // just test obtain from database 
        // database might need to add how to obtain from items
        // then update
        IVlst.Add(new Inventory(1, 4));
        IVlst.Add(new Inventory(2, 2));
        IVlst.Add(new Inventory(3, 3));
        IVlst.Add(new Inventory(4, 1));
        IVlst.Add(new Inventory(5, 1));
        IVlst.Add(new Inventory(6, 1));
        IVlst.Add(new Inventory(7, 0));
        IVlst.Add(new Inventory(8, 1));
        IVlst.Add(new Inventory(9, 0));
        IVlst.Add(new Inventory(10, 1));
        return IVlst;
    }

    private void CreateItem() {
        foreach (GameObject ele in OptionListGO) {
            ele.SetActive(true);
        }
        foreach (Inventory ele in FullItemList) {
            if (ele.GetInventoryCount() > 0) {
                ele.SetInventoryItem(InvFind(ele.ID));
                GameObject itemGO = Instantiate(itemPrefab) as GameObject;
                itemGO.transform.SetParent(GameObject.Find(ele.Category).transform, false);// set item to the related parent 
                BindtoItems(itemGO, ele);
            }
        }
        foreach (Inventory ele in FullItemList) { // to show all
            if (ele.GetInventoryCount() > 0) {
                ele.SetInventoryItem(InvFind(ele.ID));
                GameObject itemGO = Instantiate(itemPrefab) as GameObject;
                itemGO.transform.SetParent(GameObject.Find("OptionList5").transform, false);
                BindtoItems(itemGO, ele);
            }
        }
        foreach (GameObject ele in OptionListGO) {
            ele.SetActive(false);
        }
        foreach (GameObject ele in OptionBtnGO) {
            ele.GetComponent<Buttonbtn>().ReClicked();
        }
    }

    void DeleteAllItem() {
        string[] OptionList = { "OptionList1", "OptionList2", "OptionList3", "OptionList4", "OptionList5" };
        for (int i = 0; i < OptionList.Length; i++) {
            GameObject gt = OptionListGO[i];
            for (int j = 0; j < gt.transform.childCount; j++) {
                Destroy(gt.transform.GetChild(j).gameObject);
            }
        }
    }

    Inventory InvFind(int id) { // this id get from db
        int i;
        for (i = 0; i < inventoryItems.Length; i++) {
            if (inventoryItems[i].ID == id) {
                break;
            }
        }
        return inventoryItems[i];
    }

    void BindtoItems(GameObject item, Inventory inv) {
        item.GetComponent<InvItem>().BindView(inv);
    }
        
    void ItemInfoSetup() {
        if (FullItemList.Count > 0) { ShowItemInfo(FullItemList[0]); } else { HideItemInfo(); }
    }
    // public called by single items
    public void ShowItemInfo(Inventory inv) {
        selectedInv = inv;
        Adjustment.text = "0";
        ItemName.text = selectedInv.Name;
        settingRank(selectedInv.Rank);
        Price.text = selectedInv.UnitPrice.ToString() + " $";
        ItemImage.sprite = selectedInv.Image;
        Count.text = "Count: " + selectedInv.GetInventoryCount().ToString();
        Desception.text = selectedInv.Desception;
    }
    void HideItemInfo() {
        selectedInv = null;
        Adjustment.text = "0";
        ItemName.text = "No Item Selected";
        settingRank(0);
        Price.text = "0 $";
        ItemImage.sprite = null;
        Count.text = "Count: 0" ;
        Desception.text = "";
    }
    void settingRank(int lvl) {
        for (int i = 0; i < stars.Length; i++) {
            stars[i].sprite = NullStar;
        }
        for (int i = 0; i < lvl; i++) {
            stars[i].sprite = fullStar;
        }
    }

    // UI buttons action events
    public void minus() {
        int textCount = int.Parse(Adjustment.text.ToString());
        if (textCount > 0) {
            Adjustment.text = (textCount - 1) + "";
        }
    }
    public void plus() {
        int textCount = int.Parse(Adjustment.text.ToString());
        if (textCount < selectedInv.GetInventoryCount()) {
            Adjustment.text = (textCount + 1).ToString();
        }
    }
    public void sell() {
        int textCount = int.Parse(Adjustment.text.ToString());
        if (textCount != 0) {
            selectedInv.SellItem(textCount);
            Adjustment.text = "0";
            Debug.Log("You Just Sold for: " + textCount * selectedInv.UnitPrice); // gaining money will connect to database
        } else {
            Debug.Log("Set the item count ");
        }
        if (selectedInv.GetInventoryCount() == 0) {
            FullItemList.Remove(selectedInv);
            DeleteAllItem(); //remove form list
            CreateItem(); // recreate all items
            ItemInfoSetup();
            
        }
    }
    public void ChangeCategory(GameObject button) {
        Buttonbtn highlightableButton = button.GetComponent<Buttonbtn>();
        scrollRect.content = highlightableButton.optionList.GetComponent<RectTransform>();
        highlightableButton.Click();
        activeButton.Click();
        activeButton = highlightableButton;
    }
}