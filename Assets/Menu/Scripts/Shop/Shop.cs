using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cleon
{
    public class Shop : MonoBehaviour
    {
        // Faction of the Npc
        public string faction;
        // Approval to check if we can open the shop
        public float minApproval = -1f;
        // Items that this Npc sell
        public List<Item> shopItem;
    }
}
