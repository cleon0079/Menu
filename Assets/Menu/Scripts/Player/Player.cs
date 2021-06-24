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
        [SerializeField] GameObject gamePlayUI;
        [SerializeField] GameObject customisationGO;
        [SerializeField] GameObject pauseMenuGO;
        [SerializeField] GameObject mainCamera;
        [SerializeField] GameObject customisationCamera;
        [SerializeField] GameObject player;
        public Vector3 playerPos;
        public Quaternion playerRot;
        Customisation customisation;
        PauseMenu pauseMenu;
        public bool isChanging = false;
        public PlayerData playerData = new PlayerData();

        public int level;
        public int currentExp;
        int maxExp;
        public int pointPool;
        int currentPoint;

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

        int currentStrength;
        int currentAgility;
        int currentPhysique;
        int currentIntelligence;
        int currentPerceive;
        int currentFascination;

        int attack;
        int defence;
        int walkSpeed;
        int health;
        int mana;
        int stamina;

        public int currentHealth;
        public int currentMana;
        public int currentStamina;

        int hit;
        int toughness;
        int jumpspeed;
        int healthRegen;
        int manaRegen;
        int staminaRegen;

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

        private void Start()
        {
            SetDefault();
            pauseMenu = pauseMenuGO.GetComponent<PauseMenu>();
            customisation = customisationGO.GetComponent<Customisation>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C) && pauseMenu.isPause == false && customisation.isChanging == false)
            {
                Change(isChanging);
            }

            SetRaceAndPro();

            playerPos = player.transform.position;
            playerRot = player.transform.rotation;

            if(currentPoint > pointPool)
            {
                currentPoint = pointPool;
            }

            timer -= Time.deltaTime;
            if (currentExp >= maxExp)
            {
                level++;
                pointPool++;
                currentPoint++;
                currentExp -= maxExp;
                maxExp *= 2;
            }
            if (timer <= 0)
            {
                GetCurrentHealth();
                GetCurrentMana();
                GetCurrentStamina();
                currentExp += 10;
                timer = 1;
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
            healthRegen = GetHealthRegen();
            manaRegen = GetManaRegen();
            staminaRegen = GetStaminaRegen();

            UpdateText();
        }

        void SetDefault()
        {
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
            currentStrength = strength;
            currentAgility = agility;
            currentPhysique = physique;
            currentIntelligence = intelligence;
            currentPerceive = perceive;
            currentFascination = fascination;
        }

        public void ConfirmChange()
        {
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

            if (currentPoint == 0 && _dir == false && currentStrength < strength)
            {
                strength--;
                currentPoint++;
            }
        }

        public void AgilityChange(bool _dir)
        {
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

        public void Change(bool _isChanging)
        {
            // Convert the ispause bool to a int
            // Because if ispause is ture it will return 1 else return 0
            float timeScale = System.Convert.ToInt32(_isChanging);
            // Start and pause the game time
            Time.timeScale = timeScale;
            // Open and close the gameplay UI
            gamePlayUI.SetActive(_isChanging);
            // Open and close the pause menu
            playerStatsUI.SetActive(!_isChanging);
            mainCamera.SetActive(_isChanging);
            customisationCamera.SetActive(!_isChanging);
            // Show or lock the cursor
            if (!_isChanging)
            {
                Cursor.lockState = CursorLockMode.None;
                SetDefaultPoint();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            Cursor.visible = !_isChanging;
            isChanging = !_isChanging;
        }

        void UpdateText()
        {
            stat1.text = "Attack: " + attack.ToString() + "\nDefence: " + defence.ToString() + "\nWalkSpeed: " + walkSpeed.ToString()
        + "\nHealth: " + health.ToString() + "\nMana: " + mana.ToString() + "\nStamina: " + stamina.ToString();
            stat2.text = "Hit: " + hit.ToString() + "\nToughness: " + toughness.ToString() + "\nJumpSpeed: " + jumpspeed.ToString()
        + "\nHealthRegen: " + healthRegen.ToString() + "/s" + "\nManaRegen: " + manaRegen.ToString() + "/s" + "\nStaminaRegen: " + staminaRegen.ToString() + "/s";

            strengthText.text = "Strength: " + strength.ToString();
            agilityText.text = "Strength: " + agility.ToString();
            physiqueText.text = "Strength: " + physique.ToString();
            intelligenceText.text = "Strength: " + intelligence.ToString();
            perceiveText.text = "Strength: " + perceive.ToString();
            fascinationText.text = "Strength: " + fascination.ToString();

            levelText.text = "Level: " + level.ToString();
            expText.text = "EXP: " + currentExp.ToString() + "/" + maxExp.ToString();
            pointPoolText.text = "Point: (" + currentPoint.ToString() + "/" + pointPool.ToString() + ")";
        }

        void GetCurrentHealth()
        {
            if (currentHealth <= health)
            {
                currentHealth += healthRegen;
                healthBar.value = (float)currentHealth / (float)health;
                currentHealthText.text = "Health: " + Mathf.RoundToInt((float)currentHealth / (float)health * 100).ToString() + "%";
            }
        }

        void GetCurrentMana()
        {
            if (currentMana <= mana)
            {
                currentMana += manaRegen;
                manaBar.value = (float)currentMana / (float)mana;
                currentManaText.text = "Mana: " + Mathf.RoundToInt((float)currentMana / (float)mana * 100).ToString() + "%";
            }
        }

        void GetCurrentStamina()
        {
            if (currentStamina <= stamina)
            {
                currentStamina += staminaRegen;
                staminaBar.value = (float)currentStamina / (float)stamina;
                currentStaminaText.text = "Stamina: " + Mathf.RoundToInt((float)currentStamina / (float)stamina * 100).ToString() + "%";
            }
        }

        int GetAttack()
        {
            int attack = 10 + strength * 2 + agility;
            if (isOrc)
                attack *= 2;
            return attack;
        }

        int GetDefence()
        {
            int defence = 5 + physique * 2 + strength;
            if (isKnight)
                defence *= 2;
            return defence;
        }

        int GetWalkSpeed()
        {
            int walkSpeed = agility;
            if (isRogue)
                walkSpeed += 5;
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
            int jumpSpeed = agility;
            if (isRogue)
                jumpSpeed += 5;
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
    }
}