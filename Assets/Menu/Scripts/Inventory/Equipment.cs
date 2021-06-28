using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cleon
{
    [System.Serializable]
    public struct EquipmentSlot
    {
        // Make a slot for the equipment system
        // We have to have a item
        [SerializeField] private Item item;

        // If we have equiped a item or replace a new item then use the event
        public Item EquipedItem
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
                itemEquiped.Invoke(this);
            }
        }

        // To get the right location that the item is spawning
        public Transform visualLocation;

        // To set the Pos, Rot and scale to the right size
        public Vector3 offset;

        // Make a event for equip the item
        public delegate void ItemEquiped(EquipmentSlot item);
        public event ItemEquiped itemEquiped;
    }

    public class Equipment : MonoBehaviour
    {
        // Make a slot for the right hand, left hand, helmet, and bag
        public EquipmentSlot rightHand;
        public EquipmentSlot leftHand;
        public EquipmentSlot helmet;
        public EquipmentSlot bag;

        private void Awake()
        {
            // Add all the slot to listen to the event
            rightHand.itemEquiped += EquipItem;
            leftHand.itemEquiped += EquipItem;
            helmet.itemEquiped += EquipItem;
            bag.itemEquiped += EquipItem;
        }

        public void EquipItem(EquipmentSlot _item)
        {
            // If we didnt set a right location to spawn then dont run
            if (_item.visualLocation == null)
            {
                return;
            }

            // Remove the item we have equiped
            foreach (Transform child in _item.visualLocation)
            {
                GameObject.Destroy(child.gameObject);
            }

            // Null check
            if (_item.EquipedItem.Mesh == null)
            {
                return;
            }

            // Spawn the item prefab at the right location
            GameObject meshInstance = Instantiate(_item.EquipedItem.Mesh, _item.visualLocation);

            // Set the localposition to 0
            meshInstance.transform.localPosition = _item.offset;

            // If we have equiped the item, then remove its collider and rigidbody
            Destroy(meshInstance.GetComponent<Rigidbody>());
            Destroy(meshInstance.GetComponent<Collider>());

            // Get the right offset and set its right pos, rot and scale that we save in the prefab to fit
            OffsetLocation offset = meshInstance.GetComponent<OffsetLocation>();
            if (offset != null)
            {
                meshInstance.transform.localPosition += offset.positionOffset;

                meshInstance.transform.localRotation = Quaternion.Euler(offset.rotationOffset);

                meshInstance.transform.localScale = offset.scaleOffset;
            }
        }

    }
}
