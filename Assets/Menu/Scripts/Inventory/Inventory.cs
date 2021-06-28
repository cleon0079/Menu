using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace cleon
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] GameObject inventoryGameObject;
        [SerializeField] GameObject equipmentCamera;
        [SerializeField] GameObject mainCamera;
        [HideInInspector] public bool isOpen = false;
        GameManager gameManager;

        // Make a list to save all the item
        public List<Item> inventorys = new List<Item>();
        // The current item we have selected
        [NonSerialized] public Item selectedItem = null;

        [SerializeField] Button useButton;
        [SerializeField] Button buttonPrefab;
        [SerializeField] GameObject inventoryContent;
        [SerializeField] GameObject FilterContent;
        [SerializeField] GameObject displayUI;

        [SerializeField] RawImage itemImage;
        [SerializeField] Text itemName;
        [SerializeField] Text itemDescription;

        // Add a filter type that shows all the item
        string sortType = "All";

        // OnGui
        bool showIMGUIInventory;
        Vector2 scrollPosition;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            DisplayFilterCanvas();
        }
        private void Update()
        {
            // If we didnt open any menu then open the inventory menu and use other camera to look at the player
            if (Input.GetKeyDown(KeyCode.I))
            {
                if(!gameManager.isOpen)
                {
                    inventoryGameObject.SetActive(true);
                    isOpen = true;
                    mainCamera.SetActive(false);
                    equipmentCamera.SetActive(true);
                    // Display the item we have in the menu if we open the inventory menu
                    DisplayItemsCanvas();
                }
                else if(isOpen)
                {
                    inventoryGameObject.SetActive(false);
                    gameManager.shopManager.UpdateInventory(inventorys);
                    isOpen = false;
                    mainCamera.SetActive(true);
                    equipmentCamera.SetActive(false);
                }
            }

            // If we have selected a item then show the item detail in the UI else opposite
            if(selectedItem == null)
            {
                displayUI.SetActive(false);
            }
            else
            {
                displayUI.SetActive(true);
            }
        }

        public void UpdateInventory(List<Item> _inventory)
        {
            // Update the inventory from the shop when we finish shoping
            inventorys = _inventory;
        }

        public void AddItem(Item _item)
        {
            // Add the item in the inventory if we didnt say any, then use its own amount that we set up
            AddItem(_item, _item.Amount);
        }

        public void AddItem(Item _item, int _count)
        {
            // Check if we have this item already and get it
            Item foundItem = inventorys.Find((x) => x.Name == _item.Name);

            // If we dont have, add in the inventory as a new one
            if (foundItem == null)
            {
                inventorys.Add(_item);
            }

            //If we do have, add how many we wanna add to the inventory
            else
            {
                foundItem.Amount += _count;
            }

            // Update the item display
            DisplayItemsCanvas();
        }

        public void RemoveItem(Item _item)
        {
            // If we have this item already then remove it, if we dont do nothing
            if (inventorys.Contains(_item))
            {
                inventorys.Remove(_item);
            }

            // Update the item display
            DisplayItemsCanvas();
        }

        void DisplayFilterCanvas()
        {
            // Get all type of item we set up and save it to a list of string
            List<string> itemTypes = new List<string>(Enum.GetNames(typeof(Item.ItemType)));
            // Set the frist one to All type
            itemTypes.Insert(0, "All");

            // Loop all the type we have
            for (int i = 0; i < itemTypes.Count; i++)
            {
                // Spawn a button and change its text to the type we have
                Button buttonGO = Instantiate<Button>(buttonPrefab, FilterContent.transform);
                Text buttonText = buttonGO.GetComponentInChildren<Text>();
                buttonGO.name = itemTypes[i] + " Filter";
                buttonText.text = itemTypes[i];
                buttonText.fontSize = 50;

                // Give the button a onclick function that when we click, it shows the item of this type
                string itemType = itemTypes[i];
                buttonGO.onClick.AddListener(() => { ChangeFilter(itemType); });
                //buttonGO.onClick.AddListener(delegate { ChangeFilter(itemTypes[x]); });
            }
        }

        void ChangeFilter(string _itemType)
        {
            // Update the area that shows the item var the right type
            sortType = _itemType;
            DisplayItemsCanvas();
        }

        void DestroyAllChildren(Transform _parent)
        {
            // Every we update the item display UI, we remove the one before
            foreach (Transform child in _parent)
            {
                Destroy(child.gameObject);
            }
        }

        void DisplayItemsCanvas()
        {
            // Every we update the item display UI, we remove the one before
            DestroyAllChildren(inventoryContent.transform);

            // Loop all the item in this type
            for (int i = 0; i < inventorys.Count; i++)
            {
                // If its All type then shows all the item, if its one of the type then show this type of item
                if (inventorys[i].Type.ToString() == sortType || sortType == "All")
                {
                    // Create buttons and change the text to the item name
                    Button buttonGO = Instantiate<Button>(buttonPrefab, inventoryContent.transform);
                    Text buttonText = buttonGO.GetComponentInChildren<Text>();
                    buttonGO.name = inventorys[i].Name + " Button";
                    buttonText.text = inventorys[i].Name;
                    buttonText.fontSize = 50;

                    // Add a onclick function to shows the detail of the item
                    Item item = inventorys[i];
                    buttonGO.onClick.AddListener(() => { DisplaySelectedItemOnCanvas(item); });
                }
            }
        }

        public void DisplaySelectedItemOnCanvas(Item _item)
        {
            // If we click on this item then we seleted this item
            selectedItem = _item;

            // Set the item's mesh from the prefab we make var prefab name
            selectedItem.Mesh = (GameObject)Resources.Load("Prefab/Item/" + selectedItem.Name);
            // Set its image and text and description
            itemImage.texture = selectedItem.Icon;
            itemName.text = selectedItem.Name;
            itemDescription.text = selectedItem.Description +
                "\nValue: " + selectedItem.Value +
                "\nAmount: " + selectedItem.Amount +
                // If the item have damage then shows damage, same to heal and armour
                ((selectedItem.Damage > 0) ? "\nDamage: " + selectedItem.Damage : 
                (selectedItem.Armour > 0) ? "\nArmour: " + selectedItem.Armour : 
                (selectedItem.Heal > 0) ? "\nHeal: " + selectedItem.Heal : "");

            // If the item is a type of item that can equip, then give the use button a function to equip it
            if (selectedItem.Type == Item.ItemType.RightWeapon)
            {
                Text buttonText = useButton.GetComponentInChildren<Text>();
                buttonText.text = "Equip";
                buttonText.fontSize = 35;
                useButton.onClick.RemoveAllListeners();
                useButton.onClick.AddListener(() => { EquipIt(); });
            }
            if(selectedItem.Type == Item.ItemType.LeftWeapon)
            {
                Text buttonText = useButton.GetComponentInChildren<Text>();
                buttonText.text = "Equip";
                buttonText.fontSize = 35;
                useButton.onClick.RemoveAllListeners();
                useButton.onClick.AddListener(() => { EquipIt(); });
            }
            if(selectedItem.Type == Item.ItemType.Helmet)
            {
                Text buttonText = useButton.GetComponentInChildren<Text>();
                buttonText.text = "Equip";
                buttonText.fontSize = 35;
                useButton.onClick.RemoveAllListeners();
                useButton.onClick.AddListener(() => { EquipIt(); });
            }
            if(selectedItem.Type == Item.ItemType.Bag)
            {
                Text buttonText = useButton.GetComponentInChildren<Text>();
                buttonText.text = "Equip";
                buttonText.fontSize = 35;
                useButton.onClick.RemoveAllListeners();
                useButton.onClick.AddListener(() => { EquipIt(); });
            }

            // If the item is a type we can use, then give the use button a function to use it
            if(selectedItem.Type == Item.ItemType.Food)
            {
                Text buttonText = useButton.GetComponentInChildren<Text>();
                buttonText.text = "Eat";
                buttonText.fontSize = 35;
                useButton.onClick.RemoveAllListeners();
                useButton.onClick.AddListener(() => { UseIt(); });
            }

            // If the item is a type of money then dont show the use button
            if (selectedItem.Type == Item.ItemType.Money)
            {
                Text buttonText = useButton.GetComponentInChildren<Text>();
                buttonText.text = "";
            }
        }

        void UseIt()
        {
            // If we use the item, then get health
            // That is because i got only one type of heal item
            gameManager.player.currentHealth += selectedItem.Heal;

            // - the amount of the item we have and update to the UI
            selectedItem.Amount--;
            DisplaySelectedItemOnCanvas(selectedItem);

            foreach (Quest quest in gameManager.questManager.CurrentQuests)
            {
                if (quest.questState == questState.Accepted)
                {
                    if(quest.questType == questType.Eat)
                    {
                        quest.currentDoAmount++;
                    }
                }
            }

            // If we dont have anymore then remove it from the inventory
            if (selectedItem.Amount <= 0)
            {
                RemoveItem(selectedItem);
                selectedItem = null;
            }
        }

        void EquipIt()
        {
            Equipment equipment = FindObjectOfType<Equipment>();

            // If the item we wanna equip is a type of right hand weapon
            if (selectedItem.Type == Item.ItemType.RightWeapon)
            {
                // If there is a weapon on our hand already
                if(equipment.rightHand.visualLocation.childCount == 1)
                {
                    // Add the amount and add it back to the inventory
                    equipment.rightHand.EquipedItem.Amount++;
                    AddItem(equipment.rightHand.EquipedItem);
                }
                // Then we equip this item
                equipment.rightHand.EquipedItem = selectedItem;
            }

            // Same as top
            if (selectedItem.Type == Item.ItemType.LeftWeapon)
            {
                if (equipment.leftHand.visualLocation.childCount == 1)
                {
                    equipment.leftHand.EquipedItem.Amount++;
                    AddItem(equipment.leftHand.EquipedItem);
                }
                equipment.leftHand.EquipedItem = selectedItem;
            }

            // Same as top
            if (selectedItem.Type == Item.ItemType.Helmet)
            {
                if (equipment.helmet.visualLocation.childCount == 1)
                {
                    equipment.helmet.EquipedItem.Amount++;
                    AddItem(equipment.helmet.EquipedItem);
                }
                equipment.helmet.EquipedItem = selectedItem;
            }

            // Same as top
            if (selectedItem.Type == Item.ItemType.Bag)
            {
                if (equipment.bag.visualLocation.childCount == 1)
                {
                    equipment.bag.EquipedItem.Amount++;
                    AddItem(equipment.bag.EquipedItem);
                }
                equipment.bag.EquipedItem = selectedItem;
            }

            // If we have equip the item then remove 1 amount of the item in the inventory and update the UI
            selectedItem.Amount--;
            DisplaySelectedItemOnCanvas(selectedItem);
            // If we dont have anymore then remove from the inventory
            if (selectedItem.Amount <= 0)
            {
                RemoveItem(selectedItem);
                selectedItem = null;
            }
        }

        private void OnGUI()
        {
            if (showIMGUIInventory)
            {
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

                List<string> itemTypes = new List<string>(Enum.GetNames(typeof(Item.ItemType)));
                itemTypes.Insert(0, "All");

                for (int i = 0; i < itemTypes.Count; i++)
                {
                    if (GUI.Button(new Rect(
                        (Screen.width / itemTypes.Count) * i,
                        10,
                        Screen.width / itemTypes.Count,
                        20), itemTypes[i]))
                    {
                        sortType = itemTypes[i];
                    }
                }

                Display();
                if (selectedItem != null)
                {
                    DisplaySelecterItem();
                }
            }
        }

        void DisplaySelecterItem()
        {
            GUI.Box(new Rect(Screen.width / 4, Screen.height / 3,
                Screen.width / 5, Screen.height / 5),
                selectedItem.Icon);

            GUI.Box(new Rect(Screen.width / 4, (Screen.height / 3) + (Screen.height / 5),
                Screen.width / 7, Screen.height / 15),
                selectedItem.Name);

            GUI.Box(new Rect(Screen.width / 4, (Screen.height / 3) + (Screen.height / 3),
                Screen.width / 5, Screen.height / 5), selectedItem.Description +
                "\nValue: " + selectedItem.Value +
                "\nAmount: " + selectedItem.Amount);
        }

        void Display()
        {
            scrollPosition = GUI.BeginScrollView(new Rect(0, 40, Screen.width, Screen.height - 40),
                scrollPosition,
                new Rect(0, 0, 0, inventorys.Count * 30),
                false,
                true);
            int count = 0;
            for (int i = 0; i < inventorys.Count; i++)
            {
                if (inventorys[i].Type.ToString() == sortType || sortType == "All")
                {
                    if (GUI.Button(new Rect(30, 0 + (count * 30), 200, 30), inventorys[i].Name))
                    {
                        selectedItem = inventorys[i];
                        selectedItem.OnClicked();
                    }
                    count++;
                }
            }
            GUI.EndScrollView();
        }
    }
}
