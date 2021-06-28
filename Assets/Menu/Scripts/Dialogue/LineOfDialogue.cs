using UnityEngine;

namespace cleon
{
    [System.Serializable]
    public class LineOfDialogue
    {
        // Option is the word we answer, response is the npc response
        [TextArea(3, 5)]
        public string option, response;

        // To check if this dialogue will show up
        public float minApproval = -1f;

        // If its not 0, that mean change the approval of this type of faction
        public float changeApproval = 0f;

        // To load the next dialogue
        public Dialogue nextDialogue;

        // To check with dialogue it is
        [System.NonSerialized]
        public int buttomID;
    }
}