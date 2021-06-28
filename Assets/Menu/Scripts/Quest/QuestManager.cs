using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace cleon
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] GameObject questMenu;
        [HideInInspector] public bool isOpen = false;

        // List of quest to save the quest we are doing
        public List<Quest> CurrentQuests = new List<Quest>();

        [SerializeField] GameObject questContent;
        [SerializeField] GameObject finishGO;

        [SerializeField] Button buttonPrefab;
        [SerializeField] Button giveUpButton;

        [SerializeField] Text questDescription;
        [SerializeField] Text questState;
        [SerializeField] Text finishText;

        GameManager gameManager;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            DisplayQuestCanvas();
        }

        private void Update()
        {
            // If we didnt open any menu then open and close the quest menu
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if(!gameManager.isOpen)
                {
                    questMenu.SetActive(true);
                    isOpen = true;
                    DisplayQuestCanvas();
                }
                else if(isOpen)
                {
                    questMenu.SetActive(false);
                    isOpen = false;
                }
            }
            // Check what state the quest is in run time
            CheckQuestState();
        }

        void CheckQuestState()
        {
            // Load all the quest
            foreach (Quest quest in CurrentQuests)
            {
                if (quest.questState == cleon.questState.Accepted)
                {
                    // If we have done the quest
                    if(quest.toDoAmount <= quest.currentDoAmount)
                    {
                        // Shows the finish text UI and change the state
                        StartCoroutine(ShowText(quest, finishText, finishGO));
                        quest.questState = cleon.questState.Done;
                    }
                }
            }
        }

        public void AddQuest(Quest _quest)
        {
            // Null check see if we have this quest already
            Quest foundQuest = CurrentQuests.Find((x) => x.questName == _quest.questName);     
            if(foundQuest == null)
            {
                // If not then we add the quest to the quest manager
                CurrentQuests.Add(_quest);
            }

            // Reflash the menu
            DisplayQuestCanvas();
        }

        public void RemoveQuest(Quest _quest)
        {
            // Null check
            if (CurrentQuests.Contains(_quest))
            {
                // If we have this quest and give out alreay, then remove it from the quest manager
                CurrentQuests.Remove(_quest);
            }

            // Reflash the menu
            DisplayQuestCanvas();
        }

        void DisplayQuestCanvas()
        {
            // Clean the quest every time and load it again
            DestroyAllChildren(questContent.transform);
            // If there is no quest then show nothing
            questDescription.text = "";
            questState.text = "";

            // Spawn all the quest we got and show it in the content
            for (int i = 0; i < CurrentQuests.Count; i++)
            {
                Button buttonGO = Instantiate<Button>(buttonPrefab, questContent.transform);
                Text buttonText = buttonGO.GetComponentInChildren<Text>();
                buttonText.text = CurrentQuests[i].questName;

                Quest quest = CurrentQuests[i];
                buttonGO.onClick.AddListener(() => { DisplaySelectedQuestOnCanvas(quest); }) ;
            }
        }

        public void DisplaySelectedQuestOnCanvas(Quest _quest)
        {
            // When we have selected a quest then shows the detail of the quest
            questDescription.text = _quest.discription;
            if (_quest.questState == cleon.questState.Done)
                questState.text = "Done";
            else if (_quest.questState == cleon.questState.Accepted)
                questState.text = "(" + _quest.currentDoAmount + "/" + _quest.toDoAmount + ")";
            giveUpButton.onClick.AddListener(() => { GiveUp(_quest); });
        }

        void DestroyAllChildren(Transform _parent)
        {
            foreach (Transform child in _parent)
            {
                Destroy(child.gameObject);
            }
        }

        void GiveUp(Quest _quest)
        {
            // If we dont wanna do this quest then remove it and set the quest back to free
            _quest.questState = cleon.questState.Free;
            RemoveQuest(_quest);
        }

        IEnumerator ShowText(Quest _quest, Text _text, GameObject _gameObject)
        {
            // If we finish the quest then show the finish text for 3 seconds
            _text.text = "You Have Finish The " + _quest.questName + " Quest!!!";
            _gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            _gameObject.SetActive(false);
        }
    }
}
