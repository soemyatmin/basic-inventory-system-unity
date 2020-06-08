using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item Data", order = 51)]
public class Inventory : ScriptableObject {

    public int ID;
    public string Name;
    public int Rank;
    public int UnitPrice;
    public string Desception;
    public string Category;
    public Sprite Image;

    private int inventoryCount;
    public Inventory(int ID, int count) {
        this.ID = ID;
        this.inventoryCount= count;
    }

    public void SetInventoryItem(Inventory inv) { // to fill all the information to the item list from 
        this.Name = inv.Name;
        this.Rank = inv.Rank;
        this.UnitPrice = inv.UnitPrice;
        this.Desception = inv.Desception;
        this.Category = inv.Category;
        this.Image = inv.Image;
    }

    public int GetInventoryCount() {
        return inventoryCount;
    }

    public void SetInventoryCount(int value) {
        inventoryCount = value;
    }

    public void SellItem(int value) {
        inventoryCount -= value;
    }
}
