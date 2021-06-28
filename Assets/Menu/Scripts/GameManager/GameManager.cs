using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace cleon
{
    public class GameManager : MonoBehaviour
    {
        // Save all the manager here
        [SerializeField] GameObject gamePlayUI;
        public GameObject damageUI;
        public Text damageText;
        [HideInInspector] public PauseMenu pauseMenu;
        [HideInInspector] public Inventory inventory;
        [HideInInspector] public Customisation customisation;
        [HideInInspector] public Player player;
        [HideInInspector] public DialogueManager dialogueManager;
        [HideInInspector] public QuestManager questManager;
        [HideInInspector] public ShopManager shopManager;
        [HideInInspector] public SFXMusicManager sFXMusicManager;

        [SerializeField] AudioClip backgroundMusic;
        [SerializeField] AudioMixerGroup musicMixer;
        AudioSource audioSoure;

        [HideInInspector] public bool isOpen;

        private void Start()
        {
            pauseMenu = GetComponentInChildren<PauseMenu>();
            inventory = GetComponentInChildren<Inventory>();
            customisation = GetComponentInChildren<Customisation>();
            player = GetComponentInChildren<Player>();
            dialogueManager = GetComponentInChildren<DialogueManager>();
            questManager = GetComponentInChildren<QuestManager>();
            shopManager = GetComponentInChildren<ShopManager>();
            sFXMusicManager = GetComponentInChildren<SFXMusicManager>();

            audioSoure = GetComponent<AudioSource>();
            audioSoure.playOnAwake = false;
            audioSoure.loop = true;
            audioSoure.outputAudioMixerGroup = musicMixer;
            PlayMusic();
        }

        public void PlayMusic()
        {
            audioSoure.clip = backgroundMusic;
            audioSoure.Play();
        }

        private void Update()
        {
            // If there is any menu is open, then we cant open any other menu
            if (!pauseMenu.isOpen && !inventory.isOpen && !customisation.isOpen && !player.isOpen && !dialogueManager.isOpen && !questManager.isOpen && !shopManager.isOpen && !player.isDead)
            {
                isOpen = false;
            }
            else
            {
                isOpen = true;
            }

            // If we have open any menu then lock the game and shows the cursor and disable the gameplayUI
            if(isOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                gamePlayUI.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
                gamePlayUI.SetActive(true);
            }
        }
    }
}
