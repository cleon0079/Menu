using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace cleon
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] GameObject shopUIGO;
        [HideInInspector] public bool isOpen = false;
        GameManager gameManager;

        [SerializeField] GameObject shopContent;

        [SerializeField] RawImage itemImage;

        [SerializeField] Text itemName;
        [SerializeField] Text itemDescription;
        [SerializeField] Text moneyText;
        [SerializeField] Text buySellButtonText;

        [SerializeField] Button shopButton;
        [SerializeField] Button bagButton;
        [SerializeField] Button buySellButton;
        [SerializeField] Button closeButton;
        [SerializeField] Button buttonPrefab;

        // Do add money to inventory if we dont have anymore
        [SerializeField] Item money;

        // Make a list to save all the item
        [SerializeField] List<Item> shopItems = new List<Item>();
        // A copy of inventory when we are shoping
        [SerializeField] List<Item> inventoryItems = new List<Item>();

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        public void AddItem(Item _item, int _amount)
        {
            // Check if we have this item already and get it
            Item foundItem = inventoryItems.Find((x) => x.Name == _item.Name);

            // If we dont have, add in the inventory as a new one
            if (foundItem == null)
            {
                inventoryItems.Add(_item);
            }
            //If we do have, add how many we wanna add to the inventory
            else
            {
                foundItem.Amount += _amount;
            }
        }

        public void RemoveItem(Item _item)
        {
            // If we have this item already then remove it
            if (inventoryItems.Contains(_item))
            {
                inventoryItems.Remove(_item);
            }

            // If there is anymore item beside money
            Item foundItem = inventoryItems.Find((x) => x.Name != "Money");

            // If there is
            if(foundItem != null)
            {
                // Display it and reflash the UI
                DisplaySelectedInventoryItemOnCanvas(foundItem);
                DisplayInventoryItemCanvas();
            }
            else
            {
                // If there is not then shows the shop
                DisplayShopItemCanvas();
                DisplaySelectedShopItemOnCanvas(shopItems[0]);
            }
        }

        public void UpdateInventory(List<Item> _inventory)
        {
            // Get the inventory and copy it here for the shop
            inventoryItems = _inventory;
        }

        public void DisplayShopCanvas(List<Item> _items)
        {
            // Null check
            if (_items == null)
                return;

            // Shows the shop
            shopUIGO.SetActive(true);
            isOpen = true;

            // Set the shopitems to the items that this npc has
            shopItems = _items;
            // Shows the first item
            DisplaySelectedShopItemOnCanvas(shopItems[0]);

            // Update the inventory
            UpdateInventory(gameManager.inventory.inventorys);

            // Shows how many money we have
            Item foundItem = inventoryItems.Find((x) => x.Name == "Money");
            if(foundItem == null)
            {
                ShowMoneyText(0);
            }
            else
            {
                ShowMoneyText(foundItem.Amount);
            }

            closeButton.onClick.AddListener(() => { CloseMenu(); });
            shopButton.onClick.AddListener(() => { DisplayShopItemCanvas(); });
            bagButton.onClick.AddListener(() => { DisplayInventoryItemCanvas(); });

            DisplayShopItemCanvas();
        }

        public void DisplayShopItemCanvas()
        {
            DestroyAllChildren(shopContent.transform);
            DisplaySelectedShopItemOnCanvas(shopItems[0]);

            // Shows all the item that the shop has
            for (int i = 0; i < shopItems.Count; i++)
            {
                if(shopItems[i].Type != Item.ItemType.Money)
                {
                    // Create buttons and change the text to the item name
                    Button buttonGO = Instantiate<Button>(buttonPrefab, shopContent.transform);
                    Text buttonText = buttonGO.GetComponentInChildren<Text>();
                    buttonGO.name = shopItems[i].Name + " Button";
                    buttonText.text = shopItems[i].Name;

                    // Add a onclick function to shows the detail of the item
                    Item item = shopItems[i];
                    buttonGO.onClick.AddListener(() => { DisplaySelectedShopItemOnCanvas(item); });
                }
            }
        }

        public void DisplayInventoryItemCanvas()
        {
            // Shows all the item beside money that player has
            DestroyAllChildren(shopContent.transform);
            Item foundItem = inventoryItems.Find((x) => x.Name != "Money");
            if (foundItem == null)
            {
                DisplayShopItemCanvas();
            }
            else
            {
                DisplaySelectedInventoryItemOnCanvas(foundItem);
            }

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].Type != Item.ItemType.Money)
                {
                    // Create buttons and change the text to the item name
                    Button buttonGO = Instantiate<Button>(buttonPrefab, shopContent.transform);
                    Text buttonText = buttonGO.GetComponentInChildren<Text>();
                    buttonGO.name = inventoryItems[i].Name + " Button";
                    buttonText.text = inventoryItems[i].Name;

                    // Add a onclick function to shows the detail of the item
                    Item item = inventoryItems[i];
                    buttonGO.onClick.AddListener(() => { DisplaySelectedInventoryItemOnCanvas(item); });
                }
            }
        }

        void DisplaySelectedInventoryItemOnCanvas(Item _item)
        {
            // Shows the detial of the item the player have
            itemImage.texture = _item.Icon;
            itemName.text = _item.Name;
            itemDescription.text = _item.Description +
                "\nValue: " + _item.Value +
                "\nAmount: " + _item.Amount +
                // If the item have damage then shows damage, same to heal and armour
                ((_item.Damage > 0) ? "\nDamage: " + _item.Damage :
                (_item.Armour > 0) ? "\nArmour: " + _item.Armour :
                (_item.Heal > 0) ? "\nHeal: " + _item.Heal : "");

            buySellButton.onClick.RemoveAllListeners();
            buySellButton.onClick.AddListener(() => { SpendMoney(_item, -_item.Value); });
            buySellButtonText.text = "Sell";
        }

        void DisplaySelectedShopItemOnCanvas(Item _item)
        {
            // Shows the detail of the item that shop has
            itemImage.texture = _item.Icon;
            itemName.text = _item.Name;
            itemDescription.text = _item.Description +
                "\nValue: " + _item.Value +
                // If the item have damage then shows damage, same to heal and armour
                ((_item.Damage > 0) ? "\nDamage: " + _item.Damage :
                (_item.Armour > 0) ? "\nArmour: " + _item.Armour :
                (_item.Heal > 0) ? "\nHeal: " + _item.Heal : "");

            buySellButton.onClick.RemoveAllListeners();
            buySellButton.onClick.AddListener(() => { SpendMoney(_item, _item.Value); });
            buySellButtonText.text = "Buy";
        }

        public void ShowMoneyText(int _money)
        {
            moneyText.text = "Money: " + _money;
        }

        public void SpendMoney(Item _item, int _value)
        {
            // Check if we have money in the inventory
            Item foundMoney = inventoryItems.Find((x) => x.Name == "Money");

            // If we do have money
            if (foundMoney != null)
            {
                // We are buying things
                if (foundMoney.Amount >= _value && _value >= 0)
                {
                    // Add 1 of the item we buy in to the inventory
                    AddItem(_item, 1);
                    // - the money var price and update the money text in UI
                    foundMoney.Amount -= _value;
                    ShowMoneyText(foundMoney.Amount);
                }
                // If we are selling
                if(_value < 0)
                {
                    // Add money in to inventory
                    foundMoney.Amount -= _value;
                    // - the amount of the item we have sell and update UI
                    _item.Amount--;
                    DisplaySelectedInventoryItemOnCanvas(_item);
                    ShowMoneyText(foundMoney.Amount);

                    // If we dont have this item anymore then remove it from inventory
                    if (_item.Amount <= 0)
                    {
                        RemoveItem(_item);
                    }
                }
            }
            // If we dont have money
            else
            {
                // We are selling
                if(_value < 0)
                {
                    // Add money to the inventory
                    money.Amount = -_value;
                    AddItem(money, 1);
                    // - the amount of the item we have sell and update the UI
                    _item.Amount--;
                    DisplaySelectedInventoryItemOnCanvas(_item);
                    ShowMoneyText(-_value);

                    // If we dont have this item anymore then remove it from inventory
                    if (_item.Amount <= 0)
                    {
                        RemoveItem(_item);
                    }
                }
            }
        }

        void DestroyAllChildren(Transform _parent)
        {
            foreach (Transform child in _parent)
            {
                Destroy(child.gameObject);
            }
        }

        public void CloseMenu()
        {
            shopUIGO.SetActive(false);
            isOpen = false;
            // If we closes the shop then set the shopitems to null, and Update the players inventory
            shopItems = null;
            gameManager.inventory.UpdateInventory(inventoryItems);
        }
    }
}
