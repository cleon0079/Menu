using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cleon
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] GameObject[] enemySpawnPlace;
        GameObject damageContent;
        Text damageText;

        Rigidbody rigidbodyEnemy;
        GameManager gameManager;
        int enemyHealth = 500;
        int enemyAttack = 100;
        int enemyDefence = 50;
        int enemyExp = 50;
        bool isDead;
        // Start is called before the first frame update
        void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            rigidbodyEnemy = GetComponent<Rigidbody>();
            damageContent = gameManager.damageUI;
            damageText = gameManager.damageText;
        }

        private void Update()
        {
            if (enemyHealth <= 0 && !isDead)
            {
                StartCoroutine(EnemyRespawn());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                transform.LookAt(other.transform);
                transform.Translate(Vector3.back);
                rigidbodyEnemy.AddForce(new Vector3(0, 1), ForceMode.Impulse);
                Damage();
            }
        }

        void Damage()
        {
            gameManager.sFXMusicManager.PlayDamageMusic();
            gameManager.player.currentHealth -= ((enemyAttack - gameManager.player.defence / 2) <= 0) ? 0 : (enemyAttack - gameManager.player.defence / 2);
            enemyHealth -= ((gameManager.player.attack - enemyDefence / 2) <= 0) ? 0 : (gameManager.player.attack - enemyDefence / 2);
            int doDamage = ((gameManager.player.attack - enemyDefence / 2) <= 0) ? 0 : (gameManager.player.attack - enemyDefence / 2);
            Text spawnText = Instantiate<Text>(damageText, damageContent.transform);
            StartCoroutine(DamageTake(doDamage, spawnText));
        }

        IEnumerator EnemyRespawn()
        {
            isDead = true;
            gameManager.player.currentExp += enemyExp;
            int random = Mathf.RoundToInt(Random.Range(0, enemySpawnPlace.Length));
            gameObject.transform.position = enemySpawnPlace[random].transform.position;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(5);
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            enemyHealth = 500;
            isDead = false;
            yield break;
        }

        IEnumerator DamageTake(int _damage, Text _text)
        {
            _text.text = "You did " + _damage + " damager to the enemy";
            yield return new WaitForSeconds(3);
            Destroy(_text.gameObject);
            yield break;
        }
    }
}
