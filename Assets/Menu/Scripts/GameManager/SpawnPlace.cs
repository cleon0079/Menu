using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cleon
{
    public class SpawnPlace : MonoBehaviour
    {
        Player player;

        private void Start()
        {
            player = FindObjectOfType<Player>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if(!player.isDead)
                {
                    player.healthRegen += 5;
                    player.manaRegen += 5;
                    player.staminaRegen += 5;
                    player.isHealing = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if(!player.isDead)
                {
                    player.healthRegen -= 5;
                    player.manaRegen -= 5;
                    player.staminaRegen -= 5;
                    player.isHealing = false;
                }
            }
        }
    }
}
