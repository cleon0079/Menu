using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cleon
{
    public class Dialogue : MonoBehaviour
    {
        // Faction name that this dialogue is
        public string faction;

        // Greeting if the approval is normal
        public string greeting;

        // Greeting if the approval is like
        public string greetingLike;

        // Greeting if the approval is dislike
        public string greetingDislike;

        // Last line fo dialogue to quit the conversations
        public LineOfDialogue goodBye;

        // Normal dialogue that show up
        public LineOfDialogue[] dialogueOptions;

        // Check if we a loading this dialogue frist
        public bool firstDialogue;

    }
}
