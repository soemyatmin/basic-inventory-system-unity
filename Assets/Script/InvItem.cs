using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvItem : MonoBehaviour {
    private Inventory inv;
    Image image;

    public void ViewInfo() {
        InventoryManager.Instance().ShowItemInfo(inv);
        ///Debug.Log(InventoryManager.Instance().gameObject.name + " ") ;
    }

    public void BindView(Inventory inv) {
        this.inv = inv;
        image = transform.GetChild(0).GetComponent<Image>();
        image.sprite = inv.Image;
    }
}

