using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cleon
{
    public class OffsetLocation : MonoBehaviour
    {
        // Get the off set of the item we want to equip to place at the right place of the player
        public Vector3 positionOffset = Vector3.zero;
        public Vector3 rotationOffset = Vector3.zero;
        public Vector3 scaleOffset = Vector3.zero;
    }
}
