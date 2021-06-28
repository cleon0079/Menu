using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    // Type of item we have
    public enum ItemType
    {
        Food,
        RightWeapon,
        LeftWeapon,
        Helmet,
        Bag,
        Potions,
        Scrolls,
        Quest,
        Money
    }

    // Item's ID
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] int value;
    [SerializeField] int amount;
    [SerializeField] int damage;
    [SerializeField] int armour;
    [SerializeField] int heal;
    [SerializeField] Texture2D icon;
    [SerializeField] GameObject mesh;
    [SerializeField] ItemType type;

    public string Name { get { return name; } set { name = value; } }
    public string Description { get { return description; } set { description = value; } }
    public int Value { get { return value; } set { this.value = value; } }
    public int Amount { get { return amount; } set { amount = value; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public int Armour { get { return armour; } set { armour = value; } }
    public int Heal { get { return heal; } set { heal = value; } }
    public Texture2D Icon { get { return icon; } set { icon = value; } }
    public GameObject Mesh { get { return mesh; } set { mesh = value; } }
    public ItemType Type { get { return type; } set { type = value; } }

    // Let us get a type of item to save in inventory
    public Item() { }

    // When we have to drop amount of item use this
    public Item(Item _copyItem, int _copyAmount)
    {
        Name = _copyItem.Name;
        Description = _copyItem.Description;
        Value = _copyItem.Value;
        Damage = _copyItem.Damage;
        Armour = _copyItem.Armour;
        Heal = _copyItem.Heal;
        Icon = _copyItem.Icon;
        Mesh = _copyItem.Mesh;
        Type = _copyItem.Type;
        Amount = _copyAmount;
    }

    // For OnGui
    public virtual void OnClicked() => Debug.Log($"Item pressed was: {name}!");
}
