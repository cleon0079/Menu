using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cleon
{
    [System.Serializable]
    public class PlayerStats
    {
        public int strength;
        public int agility;
        public int physique;
        public int intelligence;
        public int perceive;
        public int fascination;

        public PlayerStats(int _strength, int _agility, int _physique, int _intelligence, int _perceive, int _fascination)
        {
            strength = _strength;
            agility = _agility;
            physique = _physique;
            intelligence = _intelligence;
            perceive = _perceive;
            fascination = _fascination;
        }
    }
}
