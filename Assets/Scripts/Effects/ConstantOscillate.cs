using UnityEngine;

namespace Jam.Effects
{
    public class ConstantOscillate : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _offset;
        private Vector3 Offset => _offset;

        [SerializeField]
        private float _timeTaken = 1f;
        private float TimeTaken => _timeTaken;
        private Vector3 OriginalPosition { get; set; }
        private float Timer { get; set; }

        private Transform _cachedTransform;
        private Transform CachedTransform => _cachedTransform ??= transform;
        private void Start()
        {
            OriginalPosition = transform.position;
            Timer += Random.Range(0f, 1f); //Start randomly along path
        }

        private void Update()
        {
            Timer += Time.deltaTime / TimeTaken;
            CachedTransform.position = OriginalPosition + (Offset * Mathf.Sin(Timer));
        }
    }
}
