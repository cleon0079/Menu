using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cleon
{
    public class Player : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] GameObject playerStatsUI;
        [SerializeField] GameObject mainCamera;
        [SerializeField] GameObject dieMenu;
        [SerializeField] GameObject customisationCamera;
        [SerializeField] GameObject player;
        [SerializeField] GameObject spawnPlace;
        [HideInInspector] public bool isOpen = false;
        [HideInInspector] public PlayerData playerData = new PlayerData();
        [HideInInspector] public bool isHealing = false;
        [HideInInspector] public bool isDead = false;

        Equipment equipment;
        GameManager gameManager;

        [HideInInspector] public Vector3 playerPos;
        [HideInInspector] public Quaternion playerRot;

        [HideInInspector] public int level;
        [HideInInspector] public int currentExp;
        int maxExp;
        [HideInInspector] public int pointPool;
        int currentPoint;

        [HideInInspector] public bool isHuman;
        [HideInInspector] public bool isOrc;
        [HideInInspector] public bool isElves;
        [HideInInspector] public bool isKnight;
        [HideInInspector] public bool isWizard;
        [HideInInspector] public bool isRogue;

        [HideInInspector] public int strength;
        [HideInInspector] public int agility;
        [HideInInspector] public int physique;
        [HideInInspector] public int intelligence;
        [HideInInspector] public int perceive;
        [HideInInspector] public int fascination;

        int currentStrength;
        int currentAgility;
        int currentPhysique;
        int currentIntelligence;
        int currentPerceive;
        int currentFascination;

        [HideInInspector] public int attack;
        [HideInInspector] public int defence;
        [HideInInspector] public int walkSpeed;
        int health;
        int mana;
        int stamina;

        [HideInInspector] public int currentHealth;
        [HideInInspector] public int currentMana;
        [HideInInspector] public int currentStamina;

        int hit;
        int toughness;
        [HideInInspector] public int jumpspeed;
        [HideInInspector] public int healthRegen;
        [HideInInspector] public int manaRegen;
        [HideInInspector] public int staminaRegen;

        float timer;

        [Header("Slider")]
        [SerializeField] Slider healthBar;
        [SerializeField] Slider manaBar;
        [SerializeField] Slider staminaBar;

        [Header("Text")]
        [SerializeField] Text levelText;
        [SerializeField] Text expText;
        [SerializeField] Text strengthText;
        [SerializeField] Text agilityText;
        [SerializeField] Text physiqueText;
        [SerializeField] Text intelligenceText;
        [SerializeField] Text perceiveText;
        [SerializeField] Text fascinationText;
        [SerializeField] Text pointPoolText;
        [SerializeField] Text humanText;
        [SerializeField] Text orcText;
        [SerializeField] Text elvesText;
        [SerializeField] Text knightText;
        [SerializeField] Text wizardText;
        [SerializeField] Text rogueText;
        [SerializeField] Text raceName;
        [SerializeField] Text raceAbility;
        [SerializeField] Text professionName;
        [SerializeField] Text professionAbility;
        [SerializeField] Text stat1;
        [SerializeField] Text stat2;
        [SerializeField] Text currentHealthText;
        [SerializeField] Text currentManaText;
        [SerializeField] Text currentStaminaText;
        [SerializeField] Text spawnText;

        private void Start()
        {
            SetDefault();
            equipment = FindObjectOfType<Equipment>();
            gameManager = FindObjectOfType<GameManager>();
        }

        private void Update()
        {
            // If we press C and not opening any menu then open playerstat menu
            if (Input.GetKeyDown(KeyCode.C))
            {
                if(!gameManager.isOpen)
                {
                    playerStatsUI.SetActive(true);
                    // Use the customisation camera to look at the player and disable the main camera
                    mainCamera.SetActive(false);
                    customisationCamera.SetActive(true);
                    // Set the default when we open the menu
                    SetDefaultPoint();
                    isOpen = true;
                }
                else if(isOpen)
                {
                    playerStatsUI.SetActive(false);
                    // Set the main camera back then we close the menu
                    mainCamera.SetActive(true);
                    customisationCamera.SetActive(false);
                    isOpen = false;
                }
            }

            if(currentStamina < 0)
            {
                currentStamina = 0;
            }

            if (currentHealth <= 0 && !isDead)
            {
                gameManager.sFXMusicManager.PlayDeadMusic();
                isDead = true;
                player.transform.Rotate(-90, 0, 0);
                player.transform.position = new Vector3(player.transform.position.x, .3f, player.transform.position.z);
                dieMenu.SetActive(true);

                StartCoroutine(PlayerDieAndRespawn());
            }

            // Set the text for race and pro
            SetRaceAndPro();

            // Save the pos and rot
            playerPos = player.transform.position;
            playerRot = player.transform.rotation;

            // Point Check
            if(currentPoint > pointPool)
            {
                currentPoint = pointPool;
            }

            // Timer and get exp and health and mana and stamina per sec
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                GetCurrentHealth();
                GetCurrentMana();
                GetCurrentStamina();
                currentExp += 1;
                timer = 1;
            }

            // Level up
            if (currentExp >= maxExp)
            {
                level++;
                pointPool++;
                currentPoint++;
                currentExp -= maxExp;
                maxExp *= 2;
            }


            attack = GetAttack();
            defence = GetDefence();
            walkSpeed = GetWalkSpeed();
            health = GetHealth();
            mana = GetMana();
            stamina = GetStamina();

            hit = GetHit();
            toughness = GetToughness();
            jumpspeed = GetJumpSpeed();
            if (!isDead && !isHealing)
            {
                healthRegen = GetHealthRegen();
                manaRegen = GetManaRegen();
                staminaRegen = GetStaminaRegen();
            }

            UpdateText();
        }

        void SetDefault()
        {
            // Set all the default setting
            timer = 1f;
            currentHealth = 50;
            currentMana = 50;
            currentStamina = 50;
            maxExp = 100;
            currentExp = 0;
            level = 1;
            health = 100;
            mana = 100;
            stamina = 100;
            pointPool = 5;
            currentPoint = pointPool;
        }

        void SetDefaultPoint()
        {
            // Everytime we make a change with race and pro then update this
            currentStrength = strength;
            currentAgility = agility;
            currentPhysique = physique;
            currentIntelligence = intelligence;
            currentPerceive = perceive;
            currentFascination = fascination;
        }

        public void ConfirmChange()
        {
            // Confirm the change and save it
            currentStrength = strength;
            currentAgility = agility;
            currentPhysique = physique;
            currentIntelligence = intelligence;
            currentPerceive = perceive;
            currentFascination = fascination;
            pointPool = currentPoint;
        }

        public void StrengthChange(bool _dir)
        {
            // We set the Bool on the GUI button and true is right false is left
            if (currentPoint > 0)
            {
                if (_dir)
                {
                    strength++;
                    currentPoint--;
                }
                else
                {
                    if (currentStrength < strength)
                    {
                        strength--;
                        currentPoint++;
                    }
                }
            }

            // If currentpoint is 0 and we have not comfirm
            if (currentPoint == 0 && _dir == false && currentStrength < strength)
            {
                strength--;
                currentPoint++;
            }
        }

        public void AgilityChange(bool _dir)
        {
            // Same as Strength
            if (currentPoint > 0)
            {
                if (_dir)
                {
                    agility++;
                    currentPoint--;
                }
                else
                {
                    if (currentAgility < agility)
                    {
                        agility--;
                        currentPoint++;
                    }
                }
            }

            if (currentPoint == 0 && _dir == false && currentAgility < agility)
            {
                agility--;
                currentPoint++;
            }
        }

        public void PhysiqueChange(bool _dir)
        {
            // Same as Strength
            if (currentPoint > 0)
            {
                if (_dir)
                {
                    physique++;
                    currentPoint--;
                }
                else
                {
                    if (currentPhysique < physique)
                    {
                        physique--;
                        currentPoint++;
                    }
                }
            }

            if (currentPoint == 0 && _dir == false && currentPhysique < physique)
            {
                physique--;
                currentPoint++;
            }
        }

        public void IntelligenceChange(bool _dir)
        {
            // Same as Strength
            if (currentPoint > 0)
            {
                if (_dir)
                {
                    intelligence++;
                    currentPoint--;
                }
                else
                {
                    if (currentIntelligence < intelligence)
                    {
                        intelligence--;
                        currentPoint++;
                    }
                }
            }

            if (currentPoint == 0 && _dir == false && currentIntelligence < intelligence)
            {
                intelligence--;
                currentPoint++;
            }
        }

        public void PerceiveChange(bool _dir)
        {
            // Same as Strength
            if (currentPoint > 0)
            {
                if (_dir)
                {
                    perceive++;
                    currentPoint--;
                }
                else
                {
                    if (currentPerceive < perceive)
                    {
                        perceive--;
                        currentPoint++;
                    }
                }
            }

            if (currentPoint == 0 && _dir == false && currentPerceive < perceive)
            {
                perceive--;
                currentPoint++;
            }
        }

        public void FascinationChange(bool _dir)
        {
            // Same as Strength
            if (currentPoint > 0)
            {
                if (_dir)
                {
                    fascination++;
                    currentPoint--;
                }
                else
                {
                    if (currentFascination < fascination)
                    {
                        fascination--;
                        currentPoint++;
                    }
                }
            }

            if (currentPoint == 0 && _dir == false && currentFascination < fascination)
            {
                fascination--;
                currentPoint++;
            }
        }

        void UpdateText()
        {
            // Update all the text in run time
            stat1.text = "Attack: " + attack.ToString() + "\nDefence: " + defence.ToString() + "\nWalkSpeed: " + walkSpeed.ToString()
        + "\nHealth: " + health.ToString() + "\nMana: " + mana.ToString() + "\nStamina: " + stamina.ToString();
            stat2.text = "Hit: " + hit.ToString() + "\nToughness: " + toughness.ToString() + "\nJumpSpeed: " + jumpspeed.ToString()
        + "\nHealthRegen: " + healthRegen.ToString() + "/s" + "\nManaRegen: " + manaRegen.ToString() + "/s" + "\nStaminaRegen: " + staminaRegen.ToString() + "/s";

            strengthText.text = "Strength: " + strength.ToString();
            agilityText.text = "Agility: " + agility.ToString();
            physiqueText.text = "Physique: " + physique.ToString();
            intelligenceText.text = "Intelligence: " + intelligence.ToString();
            perceiveText.text = "Perceive: " + perceive.ToString();
            fascinationText.text = "Fascination: " + fascination.ToString();

            levelText.text = "Level: " + level.ToString();
            expText.text = "EXP: " + currentExp.ToString() + "/" + maxExp.ToString();
            pointPoolText.text = "Point: (" + currentPoint.ToString() + "/" + pointPool.ToString() + ")";
        }

        void GetCurrentHealth()
        {
            // Heal and show the current health in UI
            if (currentHealth <= health)
            {
                currentHealth += healthRegen;
                healthBar.value = (float)currentHealth / (float)health;
                currentHealthText.text = "Health: (" + currentHealth + "/" + health + ")";
            }
            else
            {
                currentHealth = health;
                healthBar.value = (float)currentHealth / (float)health;
                currentHealthText.text = "Health: (" + currentHealth + "/" + health + ")";
            }
        }

        void GetCurrentMana()
        {
            // Heal and show the current mana in UI
            if (currentMana <= mana)
            {
                currentMana += manaRegen;
                manaBar.value = (float)currentMana / (float)mana;
                currentManaText.text = "Mana: (" + currentMana + "/" + mana + ")";
            }
            else
            {
                currentMana = mana;
                manaBar.value = (float)currentMana / (float)mana;
                currentManaText.text = "Mana: (" + currentMana + "/" + mana + ")";
            }
        }

        void GetCurrentStamina()
        {
            // Heal and show the current stamina in UI
            if (currentStamina <= stamina)
            {
                currentStamina += staminaRegen;
                staminaBar.value = (float)currentStamina / (float)stamina;
                currentStaminaText.text = "Stamina: (" + currentStamina + "/" + stamina + ")";
            }
            else
            {
                currentStamina = stamina;
                staminaBar.value = (float)currentStamina / (float)stamina;
                currentStaminaText.text = "Stamina: (" + currentStamina + "/" + stamina + ")";
            }
        }

        int GetAttack()
        {
            // If we have equipeditem then add to attack
            int attack = 10 + strength * 2 + agility + 
                ((equipment.rightHand.EquipedItem != null) ? equipment.rightHand.EquipedItem.Damage : 0) +
                ((equipment.leftHand.EquipedItem != null) ? equipment.leftHand.EquipedItem.Damage : 0);
            if (isOrc)
                attack *= 2;
            return attack;
        }

        int GetDefence()
        {
            // If we have equipeditem then add to defence
            int defence = 5 + physique * 2 + strength + 
                ((equipment.helmet.EquipedItem != null) ? equipment.helmet.EquipedItem.Damage : 0) +
                ((equipment.bag.EquipedItem != null) ? equipment.bag.EquipedItem.Damage : 0);
            if (isKnight)
                defence *= 2;
            return defence;
        }

        int GetWalkSpeed()
        {
            int walkSpeed = 5 + Mathf.RoundToInt(agility/50);
            if (isRogue)
                walkSpeed += 3;
            return walkSpeed;
        }

        int GetHealth()
        {
            int health = 100 + strength * 5 + physique * 10;
            return health;
        }

        int GetMana()
        {
            int mana = 100 + intelligence * 10 + perceive * 5;
            if (isWizard)
                mana *= 2;
            return mana;
        }

        int GetStamina()
        {
            int stamina = 100 + physique * 10 + strength * 2 + agility * 2 + perceive;
            return stamina;
        }

        int GetHit()
        {
            int hit = 10 + agility * 2 + perceive;
            if (isElves)
                hit *= 2;
            return hit;
        }

        int GetToughness()
        {
            int toughness = 10 + physique * 2 + perceive;
            if (isKnight)
                toughness *= 2;
            return toughness;
        }

        int GetJumpSpeed()
        {
            int jumpSpeed = 5 + Mathf.RoundToInt(agility / 50);
            if (isRogue)
                jumpSpeed += 3;
            return jumpSpeed;
        }

        int GetHealthRegen()
        {
            int healthRegen = 1 + Mathf.RoundToInt(physique / 10);
            return healthRegen;
        }

        int GetManaRegen()
        {
            int manaRegen = 1 + Mathf.RoundToInt(intelligence / 10);
            if (isWizard)
                manaRegen *= 2;
            return manaRegen;
        }

        int GetStaminaRegen()
        {
            int staminaRegen = 1 + Mathf.RoundToInt(physique / 10);
            return staminaRegen;
        }

        void SetRaceAndPro()
        {
            // If we set a race or pro then bold the text and change the color and change the text
            if (isHuman)
            {
                humanText.color = Color.red;
                humanText.fontStyle = FontStyle.Bold;
                raceName.text = "Human";
                raceAbility.text = "Learning:\nEnabling players to gain experience more quickly";
            }
            if (isOrc)
            {
                orcText.color = Color.red;
                orcText.fontStyle = FontStyle.Bold;
                raceName.text = "Orc";
                raceAbility.text = "Power Attack:\nMake the player's attack more powerful";
            }
            if (isElves)
            {
                elvesText.color = Color.red;
                elvesText.fontStyle = FontStyle.Bold;
                raceName.text = "Elves";
                raceAbility.text = "Precision:\nAttacks are easier to hit";
            }
            if (isKnight)
            {
                knightText.color = Color.red;
                knightText.fontStyle = FontStyle.Bold;
                professionName.text = "Knight";
                professionAbility.text = "Defense:\nMake the player's defense attributes stronger";
            }
            if (isRogue)
            {
                rogueText.color = Color.red;
                rogueText.fontStyle = FontStyle.Bold;
                professionName.text = "Rogue";
                professionAbility.text = "High-speed movement:\nAllows players to run faster and jump higher";
            }
            if (isWizard)
            {
                wizardText.color = Color.red;
                wizardText.fontStyle = FontStyle.Bold;
                professionName.text = "Wizard";
                professionAbility.text = "Source of magic power:\nIncrease the regen of mana and get more mana";
            }
        }

        public void Human()
        {
            // Set the default to the player
            if (isElves)
            {
                strength -= playerData.elves.strength + level;
                agility -= playerData.elves.agility + level * 2;
                physique -= playerData.elves.physique + level;
                intelligence -= playerData.elves.intelligence;
                perceive -= playerData.elves.perceive + level;
                fascination -= playerData.elves.fascination;
                elvesText.color = Color.black;
                elvesText.fontStyle = FontStyle.Normal;
                isElves = false;
            }

            if (isOrc)
            {
                strength -= playerData.orc.strength + level * 2;
                agility -= playerData.orc.agility + level;
                physique -= playerData.orc.physique + level * 2;
                intelligence -= playerData.orc.intelligence;
                perceive -= playerData.orc.perceive;
                fascination -= playerData.orc.fascination;
                orcText.color = Color.black;
                orcText.fontStyle = FontStyle.Normal;
                isOrc = false;
            }

            strength += playerData.human.strength + level;
            agility += playerData.human.agility + level;
            physique += playerData.human.physique + level;
            intelligence += playerData.human.intelligence + level;
            perceive += playerData.human.perceive + level;
            fascination += playerData.human.fascination;
            isHuman = true;
            SetDefaultPoint();
        }

        public void Orc()
        {
            // Set the default to the player
            if (isElves)
            {
                strength -= playerData.elves.strength + level;
                agility -= playerData.elves.agility + level * 2;
                physique -= playerData.elves.physique + level;
                intelligence -= playerData.elves.intelligence;
                perceive -= playerData.elves.perceive + level;
                fascination -= playerData.elves.fascination;
                elvesText.color = Color.black;
                elvesText.fontStyle = FontStyle.Normal;
                isElves = false;
            }

            if (isHuman)
            {
                strength -= playerData.human.strength + level;
                agility -= playerData.human.agility + level;
                physique -= playerData.human.physique + level;
                intelligence -= playerData.human.intelligence + level;
                perceive -= playerData.human.perceive + level;
                fascination -= playerData.human.fascination + level;
                humanText.color = Color.black;
                humanText.fontStyle = FontStyle.Normal;
                isHuman = false;
            }

            strength += playerData.orc.strength + level * 2;
            agility += playerData.orc.agility + level;
            physique += playerData.orc.physique + level * 2;
            intelligence += playerData.orc.intelligence;
            perceive += playerData.orc.perceive;
            fascination += playerData.orc.fascination;
            isOrc = true;
            SetDefaultPoint();
        }

        public void Elves()
        {
            // Set the default to the player
            if (isOrc)
            {
                strength -= playerData.orc.strength + level * 2;
                agility -= playerData.orc.agility + level;
                physique -= playerData.orc.physique + level * 2;
                intelligence -= playerData.orc.intelligence;
                perceive -= playerData.orc.perceive;
                fascination -= playerData.orc.fascination;
                orcText.color = Color.black;
                orcText.fontStyle = FontStyle.Normal;
                isOrc = false;
            }

            if (isHuman)
            {
                strength -= playerData.human.strength + level;
                agility -= playerData.human.agility + level;
                physique -= playerData.human.physique + level;
                intelligence -= playerData.human.intelligence + level;
                perceive -= playerData.human.perceive + level;
                fascination -= playerData.human.fascination + level;
                humanText.color = Color.black;
                humanText.fontStyle = FontStyle.Normal;
                isHuman = false;
            }

            strength += playerData.elves.strength + level;
            agility += playerData.elves.agility + level * 2;
            physique += playerData.elves.physique + level;
            intelligence += playerData.elves.intelligence;
            perceive += playerData.elves.perceive + level;
            fascination += playerData.elves.fascination;
            isElves = true;
            SetDefaultPoint();
        }

        public void Knight()
        {
            // Set the default to the player
            if (isRogue)
            {
                strength -= playerData.rogue.strength;
                agility -= playerData.rogue.agility;
                physique -= playerData.rogue.physique;
                intelligence -= playerData.rogue.intelligence;
                perceive -= playerData.rogue.perceive;
                fascination -= playerData.rogue.fascination;
                rogueText.color = Color.black;
                rogueText.fontStyle = FontStyle.Normal;
                isRogue = false;
            }

            if (isWizard)
            {
                strength -= playerData.wizard.strength;
                agility -= playerData.wizard.agility;
                physique -= playerData.wizard.physique;
                intelligence -= playerData.wizard.intelligence;
                perceive -= playerData.wizard.perceive;
                fascination -= playerData.wizard.fascination;
                wizardText.color = Color.black;
                wizardText.fontStyle = FontStyle.Normal;
                isWizard = false;
            }

            strength += playerData.knight.strength;
            agility += playerData.knight.agility;
            physique += playerData.knight.physique;
            intelligence += playerData.knight.intelligence;
            perceive += playerData.knight.perceive;
            fascination += playerData.knight.fascination;
            isKnight = true;
            SetDefaultPoint();
        }

        public void Rogue()
        {
            // Set the default to the player
            if (isKnight)
            {
                strength -= playerData.knight.strength;
                agility -= playerData.knight.agility;
                physique -= playerData.knight.physique;
                intelligence -= playerData.knight.intelligence;
                perceive -= playerData.knight.perceive;
                fascination -= playerData.knight.fascination;
                knightText.color = Color.black;
                knightText.fontStyle = FontStyle.Normal;
                isKnight = false;
            }

            if (isWizard)
            {
                strength -= playerData.wizard.strength;
                agility -= playerData.wizard.agility;
                physique -= playerData.wizard.physique;
                intelligence -= playerData.wizard.intelligence;
                perceive -= playerData.wizard.perceive;
                fascination -= playerData.wizard.fascination;
                wizardText.color = Color.black;
                wizardText.fontStyle = FontStyle.Normal;
                isWizard = false;
            }

            strength += playerData.rogue.strength;
            agility += playerData.rogue.agility;
            physique += playerData.rogue.physique;
            intelligence += playerData.rogue.intelligence;
            perceive += playerData.rogue.perceive;
            fascination += playerData.rogue.fascination;
            isRogue = true;
            SetDefaultPoint();
        }

        public void Wizard()
        {
            // Set the default to the player
            if (isKnight)
            {
                strength -= playerData.knight.strength;
                agility -= playerData.knight.agility;
                physique -= playerData.knight.physique;
                intelligence -= playerData.knight.intelligence;
                perceive -= playerData.knight.perceive;
                fascination -= playerData.knight.fascination;
                knightText.color = Color.black;
                knightText.fontStyle = FontStyle.Normal;
                isKnight = false;
            }

            if (isRogue)
            {
                strength -= playerData.rogue.strength;
                agility -= playerData.rogue.agility;
                physique -= playerData.rogue.physique;
                intelligence -= playerData.rogue.intelligence;
                perceive -= playerData.rogue.perceive;
                fascination -= playerData.rogue.fascination;
                rogueText.color = Color.black;
                rogueText.fontStyle = FontStyle.Normal;
                isRogue = false;
            }

            strength += playerData.wizard.strength;
            agility += playerData.wizard.agility;
            physique += playerData.wizard.physique;
            intelligence += playerData.wizard.intelligence;
            perceive += playerData.wizard.perceive;
            fascination += playerData.wizard.fascination;
            isWizard = true;
            SetDefaultPoint();
        }

        IEnumerator PlayerDieAndRespawn()
        {
            for (int i = (5 + level); i < (6 + level); i--)
            {
                if (i == 0)
                {
                    player.transform.Rotate(0, 0, 0);
                    player.transform.position = new Vector3(spawnPlace.transform.position.x, 1f, spawnPlace.transform.position.z);
                    dieMenu.SetActive(false);
                    if(currentHealth <= 0)
                    {
                        currentHealth = 10;
                    }
                    isDead = false;
                    gameManager.sFXMusicManager.PlaySpawnMusic();
                    yield break;
                }
                spawnText.text = "Respawn in " + i + " second";
                yield return new WaitForSecondsRealtime(1);
            }
        }
    }
}