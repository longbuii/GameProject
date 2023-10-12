using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public int itemPrice;
        public int availableQuantity;
        public GameObject itemPrefab; // Prefab của vật phẩm
    }

    public List<ShopItem> shopItems;

    [Header("Shop Items")]
    private int selectedShopItemIndex = -1;

    [Header("Ui Items Section")]
    // Inventory system
    public GameObject ui_Window;
    public Image[] items_Images;
    public TextMeshProUGUI[] item_Price;

    [Header("Ui Items Description")]
    public GameObject ui_Description_Window;
    public Image description_Image;
    public TextMeshProUGUI description_Title;
    public TextMeshProUGUI description_Description;

    [Header("Reference")]

    public InventorySystem inventorySystem;
    public ScoreSystem scoreSystem;


    void Start()
    {
        scoreSystem = GetComponent<ScoreSystem>();
        inventorySystem = GetComponent<InventorySystem>();
       
    }

    // Update is called once per frame
    public void OpenShop()
    {
        ui_Window.SetActive(true);

        for (int i = 0; i < items_Images.Length; i++)
        {
            if (i < shopItems.Count)
            {
                items_Images[i].sprite = shopItems[i].itemPrefab.GetComponent<SpriteRenderer>().sprite;
                int itemPrice = shopItems[i].itemPrice;
                item_Price[i].text = itemPrice.ToString();

                if (shopItems[i].availableQuantity > 0)
                {
                    // Kiểm tra tiền của người chơi và các điều kiện mua hàng
                    if (FindObjectOfType<ScoreSystem>().scoreNum >= itemPrice)
                    {
                        // Nếu đủ điều kiện, người chơi có thể mua item bằng cách click vào hình ảnh.
                        // Sự kiện PurchaseItem đã được cài đặt trong Inspector.
                    }
                }
                else
                {
                    items_Images[i].gameObject.SetActive(false);
                    item_Price[i].gameObject.SetActive(false); // Ẩn giá

                }
            }
            else
            {
                items_Images[i].gameObject.SetActive(false);
                item_Price[i].gameObject.SetActive(false); // Ẩn giá
            }
        }
    }
    

    public void CloseShop()
    {
        ui_Window.SetActive(false);

        // Kiểm tra nếu người chơi không đủ tiền hoặc tiền của họ bằng 0, thì tự động đóng cửa hàng
        if (selectedShopItemIndex == -1 || shopItems[selectedShopItemIndex].itemPrice > FindObjectOfType<ScoreSystem>().scoreNum)
        {
            selectedShopItemIndex = -1; // Đặt lại selectedShopItemIndex để cho phép người chơi mở cửa hàng lại lần sau
            return;
        }

        // Đặt lại sự kiện khi click vào item để mở cửa hàng lại
        for (int i = 0; i < items_Images.Length; i++)
        {
            if (i < shopItems.Count)
            {
                int itemPrice = shopItems[i].itemPrice;
                if (FindObjectOfType<ScoreSystem>().scoreNum >= itemPrice)
                {
                    int index = i;
                    items_Images[i].gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                    items_Images[i].gameObject.GetComponent<Button>().onClick.AddListener(() => PurchaseItem(index));
                }
            }
        }
    }

    public void PurchaseItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < shopItems.Count)
        {
            // Kiểm tra xem vị trí đã chọn có hợp lệ và có thể mua không
            if (selectedShopItemIndex == -1)
            {
                // Thiết lập vị trí đã chọn và sự kiện cho cửa hàng
                selectedShopItemIndex = itemIndex;
                bool itemPurchased = Purchase(shopItems[selectedShopItemIndex]);
                if (itemPurchased)
                {
                    // Cập nhật giao diện cửa hàng chỉ khi mua thành công
                    OpenShop();
                    selectedShopItemIndex = -1; // Đặt lại vị trí đã chọn
                }
            }
        }
    }

    public void ConfirmPurchase(int itemIndex)
    {
        // Hàm này được gọi khi player click vào item để xác nhận mua
        bool itemPurchased = Purchase(shopItems[itemIndex]);
        if (itemPurchased)
        {
            // Cập nhật lại giao diện cửa hàng
            OpenShop();
            selectedShopItemIndex = -1; // Đặt lại vị trí đã chọn
        }
    }
    bool Purchase(ShopItem item)
    {
        ScoreSystem scoreSystem = FindObjectOfType<ScoreSystem>();
        if (scoreSystem.scoreNum >= item.itemPrice && item.availableQuantity > 0)
        {
            scoreSystem.TryPurchaseItem(item.itemPrice);
            item.availableQuantity--; // Trừ số lượng còn lại của vật phẩm
            GameObject purchasedItem = Instantiate(item.itemPrefab);
            purchasedItem.name = item.itemName;
            FindObjectOfType<InventorySystem>().Pickup(purchasedItem);

            // Debug.Log để kiểm tra
            Debug.Log("Purchased " + item.itemName + " for " + item.itemPrice + " coins.");

            return true;
        }
        else
        {
            // Debug.Log để kiểm tra
            Debug.Log("Purchase failed for " + item.itemName);
        }

        return false;
    }


    public void showDescription(int _id)
    {
        // Set the image
        description_Image.sprite = items_Images[_id].sprite;
        // Set the title
        if (shopItems[_id].availableQuantity > 0)
        {
            description_Title.text = shopItems[_id].itemName;
        }
        else
        {
            description_Title.text = "Sold out";
        }
        // Show the description
        description_Description.text = "Price: " + shopItems[_id].itemPrice;
        // Set the window
        ui_Description_Window.SetActive(true);
    }
    public void hideDescription()
    {
        ui_Description_Window.SetActive(false);

    }

}


