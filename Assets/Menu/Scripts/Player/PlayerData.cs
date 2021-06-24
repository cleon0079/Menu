using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cleon
{
    [System.Serializable]
    public class PlayerData
    {
        public float[] position = new float[3];
        public float[] rotation = new float[4];
        public Vector3 Position => new Vector3(position[0], position[1], position[2]);
        public Quaternion Rotation => new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);

        public int level;
        public int point;
        public int exp;
        public bool isHuman;
        public bool isOrc;
        public bool isElves;
        public bool isKnight;
        public bool isWizard;
        public bool isRogue;
        public int strength;
        public int agility;
        public int physique;
        public int intelligence;
        public int perceive;
        public int fascination;
        public int currentHealth;
        public int currentMana;
        public int currentStamina;

        public PlayerStats human;
        public PlayerStats orc;
        public PlayerStats elves;
        public PlayerStats knight;
        public PlayerStats wizard;
        public PlayerStats rogue;

        public PlayerData()
        {
            knight = new PlayerStats(15, 10, 15, 10, 10, 10);
            wizard = new PlayerStats(10, 10, 10, 15, 15, 10);
            rogue = new PlayerStats(10, 15, 10, 10, 15, 10);
            human = new PlayerStats(5, 5, 5, 10, 10, 5);
            orc = new PlayerStats(10, 5, 10, 5, 5, 5);
            elves = new PlayerStats(5, 10, 5, 5, 5, 10);
        }

        public PlayerData(Player _player)
        {
            level = _player.level;
            point = _player.pointPool;
            exp = _player.currentExp;
            isHuman = _player.isHuman;
            isOrc = _player.isOrc;
            isElves = _player.isElves;
            isKnight = _player.isKnight;
            isWizard = _player.isWizard;
            isRogue = _player.isRogue;
            strength = _player.strength;
            agility = _player.agility;
            physique = _player.physique;
            intelligence = _player.intelligence;
            perceive = _player.perceive;
            fascination = _player.fascination;
            currentHealth = _player.currentHealth;
            currentMana = _player.currentMana;
            currentStamina = _player.currentStamina;

            position[0] = _player.playerPos.x;
            position[1] = _player.playerPos.y;
            position[2] = _player.playerPos.z;

            rotation[0] = _player.playerRot.x;
            rotation[1] = _player.playerRot.y;
            rotation[2] = _player.playerRot.z;
            rotation[3] = _player.playerRot.w;
        }

        public void LoadPlayerData(Player _player)
        {
            _player.level = level;
            _player.pointPool = point;
            _player.currentExp = exp;
            _player.isHuman = isHuman;
            _player.isOrc = isOrc;
            _player.isElves = isElves;
            _player.isKnight = isKnight;
            _player.isWizard = isWizard;
            _player.isRogue = isRogue;
            _player.strength = strength;
            _player.agility = agility;
            _player.physique = physique;
            _player.intelligence = intelligence;
            _player.perceive = perceive;
            _player.fascination = fascination;
            _player.currentHealth = currentHealth;
            _player.currentMana = currentMana;
            _player.currentStamina = currentStamina;

            _player.playerPos.x = position[0];
            _player.playerPos.y = position[1];
            _player.playerPos.z = position[2];

            _player.playerRot.x = rotation[0];
            _player.playerRot.y = rotation[1];
            _player.playerRot.z = rotation[2];
            _player.playerRot.w = rotation[3];
        }
    }
}
