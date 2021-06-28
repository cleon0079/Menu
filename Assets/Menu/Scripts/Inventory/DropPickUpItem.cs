using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cleon
{
    public class DropPickUpItem : MonoBehaviour
    {
        [SerializeField] Transform dropPoint;
        [SerializeField] Camera mainCamera;
        GameManager gameManager;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        private void Update()
        {
            // If we are not opened any other menu then we can pick up a item 
            if (Input.GetKeyDown(KeyCode.F) && !gameManager.isOpen)
            {
                PickItem();
            }
        }

        public void PickItem()
        {
            // Get the middle point form the camera and shoot a Ray
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0));
            RaycastHit hitInfo;

            // If we hit a item that is a item
            if (Physics.Raycast(ray, out hitInfo, 5f, LayerMask.GetMask("Item")))
            {
                // Get the item that we pick and save it in the inventory, if the item do not have a dropeditem.cs by any chance, do nothing
                DropItem dropedItem = hitInfo.collider.gameObject.GetComponent<DropItem>();
                if (dropedItem != null)
                {
                    // Add the item to the inventory and destroy the item in the scene
                    gameManager.inventory.AddItem(dropedItem.item);
                    Destroy(hitInfo.collider.gameObject);
                }
            }
        }

        public void DropItem()
        {
            // Null check
            if (gameManager.inventory.selectedItem == null)
            {
                return;
            }

            // Get the mesh from selectedItem in inventory and do a null check
            GameObject mesh = gameManager.inventory.selectedItem.Mesh;
            if (mesh != null)
            {
                // Spawn the item in the world
                GameObject spawnedMesh = Instantiate(mesh, null);

                // Set the item's position at the dropPoint
                spawnedMesh.transform.position = dropPoint.position;

                // Get the item's id from item prefab
                DropItem dropedItem = mesh.GetComponent<DropItem>();

                // Null check
                if (dropedItem != null)
                {
                    // We just drop 1 amount of this item
                    dropedItem.item = new Item(gameManager.inventory.selectedItem, 1);
                }
            }

            // - the item amount in the inventory
            gameManager.inventory.selectedItem.Amount--;

            // Reflash the amount in the canvas(UI)
            gameManager.inventory.DisplaySelectedItemOnCanvas(gameManager.inventory.selectedItem);

            // If we dont have this item any more then remove this item in the inventory, and set the selected item to null
            if (gameManager.inventory.selectedItem.Amount <= 0)
            {
                gameManager.inventory.RemoveItem(gameManager.inventory.selectedItem);
                gameManager.inventory.selectedItem = null;
            }
        }
    }
}
