using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cleon
{
    // State of the quest
    public enum questState
    {
        Free,
        Accepted,
        Done
    }

    // Type of the quest
    public enum questType
    {
        Kill,
        Jump,
        Eat
    }

    public class Quest : MonoBehaviour
    {
        // What faction is the quest
        public string faction;
        // Name of the quest
        public string questName;
        // Discription of the quest
        [TextArea(3, 5)] public string discription;

        // What level the quest need to be accepted
        public int minLevelNeeded = 1;
        // How many amount of things we have to do
        public int toDoAmount;
        // How many we have done already
        public int currentDoAmount;
        // If we finsih the quest and if it will change the factions approval
        public float changeApproval = 0f;

        public questState questState;
        public questType questType;

        // Reward of exp amount we can get if we done the quest
        public int expReward;
    }
}
