using System;
using UnityEngine;

namespace Jam.Effects
{
    public class SoundManager : MonoBehaviour
    {
        public enum SoundType { LevelStart, LevelEnd }
        
        //TODO replace with serialised dictionary
        [SerializeField]
        private AudioClip[] _audioClips;
        private AudioClip[] AudioClips => _audioClips;
        
        private AudioSource _mainSource;
        private AudioSource MainSource => _mainSource ??= gameObject.AddComponent<AudioSource>();
        
        public void PlaySound(SoundType soundType)
        {
            MainSource.clip = AudioClips[(int)soundType];
            MainSource.Play();
        }
    }
}
