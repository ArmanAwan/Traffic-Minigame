using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jam.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class EnvironmentEffect : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _soundEffect;
        private AudioClip SoundEffect => _soundEffect;
        
        private ParticleSystem MainSystem { get; set; }

        private void Start()
        {
            MainSystem = GetComponent<ParticleSystem>();
            if (!SoundEffect) return;
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = SoundEffect;
            audioSource.Play();
        }

        private void Update()
        {
            if(MainSystem.IsAlive()) return;
            Destroy(gameObject);
        }
    }
}
