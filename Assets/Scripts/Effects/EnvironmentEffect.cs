using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
        }

        private void Update()
        {
            if(MainSystem.IsAlive()) return;
            Destroy(gameObject);
        }
    }
}
