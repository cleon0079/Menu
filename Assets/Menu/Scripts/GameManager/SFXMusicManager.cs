using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace cleon
{
    public class SFXMusicManager : MonoBehaviour
    {
        [SerializeField] AudioClip jumpMusic;
        [SerializeField] AudioClip damageMusic;
        [SerializeField] AudioClip spawnMusic;
        [SerializeField] AudioClip deadMusic;
        [SerializeField] AudioMixerGroup sfxMixer;
        AudioSource audioSoure;

        private void Start()
        {
            audioSoure = GetComponent<AudioSource>();
            audioSoure.playOnAwake = false;
            audioSoure.outputAudioMixerGroup = sfxMixer;
        }

        public void PlayJumpMusic()
        {
            audioSoure.clip = jumpMusic;
            audioSoure.Play();
        }

        public void PlayDamageMusic()
        {
            audioSoure.clip = damageMusic;
            audioSoure.Play();
        }

        public void PlaySpawnMusic()
        {
            audioSoure.clip = spawnMusic;
            audioSoure.Play();
        }

        public void PlayDeadMusic()
        {
            audioSoure.clip = deadMusic;
            audioSoure.Play();
        }
    }
}
