using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContents : MonoBehaviour
{
    public Item item;  // Item mà rương chứa (có thể là Prefab của item).

    public void ApplyContentsToInventory()
    {
        if (item != null)
        {
            InventorySystem inventory = FindObjectOfType<InventorySystem>();
            if (inventory != null)
            {
                // Thêm item vào inventory.
                inventory.Pickup(item.gameObject);
            }
        }
    }
}
