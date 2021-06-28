using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cleon
{
    public class DialogueManager : MonoBehaviour
    {
        Dialogue currentDialogue;

        [SerializeField] GameObject mainCamera;
        [SerializeField] GameObject dialoguePanel;
        [SerializeField] GameObject buttonPrefab;
        [HideInInspector] public bool isOpen;

        [SerializeField] Transform dialogueRayPoint;
        [SerializeField] Transform dialogueButtonPanel;

        [SerializeField] Text responseText;
        [SerializeField] Text approvalText;

        // To get the npc's camera
        Camera dialogueCamera;
        AudioListener dialogueListener;

        FactionManager factionManager;
        GameManager gameManager;

        private void Start()
        {
            factionManager = FindObjectOfType<FactionManager>();
            gameManager = FindObjectOfType<GameManager>();
        }

        private void Update()
        {
            // If there is no menu is open and press E then do
            if (Input.GetKeyDown(KeyCode.E) && !gameManager.isOpen)
            {
                RaycastHit hit;
                if (Physics.Raycast(dialogueRayPoint.position, dialogueRayPoint.forward, out hit, 2))
                {
                    if (hit.transform.tag == "NPC")
                    {
                        // Load all the dialogue and quest and shop with the npc
                        dialogueCamera = hit.transform.GetComponentInChildren<Camera>();
                        dialogueListener = hit.transform.GetComponentInChildren<AudioListener>();
                        Dialogue[] npcDialogue = hit.transform.GetComponentsInChildren<Dialogue>();
                        Quest[] npcQuest = hit.transform.GetComponentsInChildren<Quest>();
                        Shop npcShop = hit.transform.GetComponentInChildren<Shop>();

                        // If the Npc can talk
                        if (npcDialogue != null)
                        {
                            foreach (Dialogue dialogue in npcDialogue)
                            {
                                // Find the first dialogue and load it, and change the camera to the dialogue camera
                                if (dialogue.firstDialogue)
                                {
                                    mainCamera.SetActive(false);
                                    dialogueCamera.enabled = true;
                                    dialogueListener.enabled = true;
                                    LoadDialogue(dialogue, npcQuest, npcShop);
                                }
                            }
                        }
                    }
                }
            }

            if(dialoguePanel.activeSelf)
                isOpen = true;
            else
                isOpen = false;
        }

        public void LoadDialogue(Dialogue _dialogue, Quest[] _quest = null, Shop _shop = null)
        {
            // Shows the dialogue menu and set the current dialogue to this dialogue
            dialoguePanel.SetActive(true);
            currentDialogue = _dialogue;

            // Clean all the buttons in the options area
            CleanUpButtons();

            // Get the approval from this dialogue
            UpdateApproval(factionManager.FactionsApproval(currentDialogue.faction));
            Button spawnedButton;
            int i = 0;
            // Spawn a button for each dialogue option inside dialogue
            foreach (LineOfDialogue dialogue in _dialogue.dialogueOptions)
            {
                float? currentApproval = factionManager.FactionsApproval(currentDialogue.faction);

                if (currentApproval != null && currentApproval > dialogue.minApproval)
                {
                    spawnedButton = Instantiate(buttonPrefab, dialogueButtonPanel).GetComponent<Button>();
                    spawnedButton.GetComponentInChildren<Text>().text = dialogue.option;

                    dialogue.buttomID = i;
                    spawnedButton.onClick.AddListener(() => ButtonClicked(dialogue.buttomID));
                }
                i++;
            }

            // If the Npc has a shop with it then give a option to open the shop
            if (_shop != null)
            {
                float? currentApproval = factionManager.FactionsApproval(_shop.faction);

                if (currentApproval != null && currentApproval > _shop.minApproval)
                {
                    spawnedButton = Instantiate(buttonPrefab, dialogueButtonPanel).GetComponent<Button>();
                    spawnedButton.GetComponentInChildren<Text>().text = "Open Shop";

                    spawnedButton.onClick.AddListener(() => OpenShop(_shop.shopItem));
                }
            }

            // If the Npc has quests with it then give a option to see what quest he has
            if (_quest != null)
            {
                foreach (Quest quest in _quest)
                {
                    if (quest.questState != questState.Accepted)
                    {
                        spawnedButton = Instantiate(buttonPrefab, dialogueButtonPanel).GetComponent<Button>();
                        spawnedButton.GetComponentInChildren<Text>().text = "Quest: " + quest.questName + "(Lv: " + quest.minLevelNeeded + ")";

                        spawnedButton.onClick.AddListener(() => { QuestButtonClicked(quest); });
                    }
                }
            }


            // Spawn the goodbye button
            spawnedButton = Instantiate(buttonPrefab, dialogueButtonPanel).GetComponent<Button>();
            spawnedButton.GetComponentInChildren<Text>().text = currentDialogue.goodBye.option;
            currentDialogue.goodBye.buttomID = i;
            spawnedButton.onClick.AddListener(() => EndConversation());

            // Print the word that the NPC says
            PrintResponse(factionManager.FactionsApproval(currentDialogue.faction));

        }

        void EndConversation()
        {
            // End of the conversation then close the dialogue menu and set the main camera to active
            CleanUpButtons();
            dialogueCamera.enabled = false;
            dialogueListener.enabled = false;
            mainCamera.SetActive(true);
            dialoguePanel.SetActive(false);
        }

        void OpenShop(List<Item> _shopItems)
        {
            // End the conversation and open up the shop and pass in what the npc have with him
            EndConversation();
            gameManager.shopManager.DisplayShopCanvas(_shopItems);
        }

        void ButtonClicked(int _dialogueNum)
        {
            // If this dialogue change the approval then change it
            factionManager.FactionsApproval(currentDialogue.faction, currentDialogue.dialogueOptions[_dialogueNum].changeApproval);

            // If there is a next dialogue then load it, if not, update the approval and npc says the response
            if (currentDialogue.dialogueOptions[_dialogueNum].nextDialogue != null)
            {
                LoadDialogue(currentDialogue.dialogueOptions[_dialogueNum].nextDialogue);
            }
            else
            {
                UpdateApproval(factionManager.FactionsApproval(currentDialogue.faction));
                responseText.text = currentDialogue.dialogueOptions[_dialogueNum].response;
            }
        }

        void QuestButtonClicked(Quest _quest)
        {
            // If the quest is free to accepted then change the options area to quest's option
            if(_quest.questState == questState.Free)
            {
                CleanUpButtons();

                // Shows the name, discription, and what level its has to be
                responseText.text = "Quest: " + _quest.questName+ "\n" + _quest.discription + "\nDo u wanna do?" + "\nLevel: " + _quest.minLevelNeeded;

                // Spawn the option in the option area
                Button yesButton = Instantiate(buttonPrefab, dialogueButtonPanel).GetComponent<Button>();
                yesButton.GetComponentInChildren<Text>().text = "Yes";
                Button noButton = Instantiate(buttonPrefab, dialogueButtonPanel).GetComponent<Button>();
                noButton.GetComponentInChildren<Text>().text = "No";

                yesButton.onClick.AddListener(() => { YesForQuest(_quest); });
                noButton.onClick.AddListener(() => { EndConversation(); });
            }
            // If we have done this quest and come back to get reward
            else
            {
                CleanUpButtons();

                // Change the response text to what we get
                responseText.text = "U have done the quest and here is your Reward!" + "\nExp: " + _quest.expReward;

                // Spawn a option to quit the conversation
                Button thanksButton = Instantiate(buttonPrefab, dialogueButtonPanel).GetComponent<Button>();
                thanksButton.GetComponentInChildren<Text>().text = "Thanks";

                // If the quest can change approval then change it
                factionManager.FactionsApproval(_quest.faction, _quest.changeApproval);
                UpdateApproval(factionManager.FactionsApproval(_quest.faction));

                // Make the quest can be accept again
                _quest.questState = questState.Free;

                // Get the reward and reset the quest
                gameManager.player.currentExp += _quest.expReward;
                _quest.currentDoAmount = 0;
                
                // Remove the quest from the quest manager
                gameManager.questManager.RemoveQuest(_quest);

                thanksButton.onClick.AddListener(() => { EndConversation(); });
            }
        }

        void YesForQuest(Quest _quest)
        {
            // If we say yes to accept the quest
            if(_quest.minLevelNeeded > gameManager.player.level)
            {
                // If we cant accept the quest then tell the player
                responseText.text = _quest.discription + "\nDo u wanna do?" + "\nLevel: " + _quest.minLevelNeeded + "\nGo level up man";
            }
            else
            {
                // If our level is ok to accpet then accept it and add to the quest manager and end the conversation
                _quest.questState = questState.Accepted;
                gameManager.questManager.AddQuest(_quest);
                EndConversation();
            }
        }

        void UpdateApproval(float? _approval)
        {
            // Null check
            if(_approval == null)
            {
                return;
            }

            // Shows the npc like us or not
            if (_approval > 0.5f)
            {
                approvalText.text = "Like You";
            }
            else if (_approval >= -0.5f)
            {
                approvalText.text = "Dont Care You";
            }
            else
            {
                approvalText.text = "Dislike You";
            }
        }

        void CleanUpButtons()
        {
            foreach (Transform child in dialogueButtonPanel)
            {
                Destroy(child.gameObject);
            }
        }

        void PrintResponse(float? _approval)
        {
            // Null check
            if (_approval == null)
            {
                return;
            }

            // Depen on the npc like us or not then shows the UI to tell the player
            if (_approval > 0.5f)
            {
                responseText.text = currentDialogue.greetingLike;
            }
            else if (_approval >= -0.5f)
            {
                responseText.text = currentDialogue.greeting;
            }
            else
            {
                responseText.text = currentDialogue.greetingDislike;
            }
        }
    }
}
