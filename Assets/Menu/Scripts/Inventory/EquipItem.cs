using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cleon
{
    [RequireComponent(typeof(DropItem), typeof(OffsetLocation))]
    public class EquipItem : MonoBehaviour
    {
        // For the item that can equip and get its item's id
        DropItem dropItem;
        [HideInInspector]public Item item;

        private void Start()
        {
            dropItem = gameObject.GetComponent<DropItem>();
            item = dropItem.item;
        }
    }
}
