using UnityEngine;

namespace Jam.Effects
{
    public class ClickEffect : MonoBehaviour
    {
        private float EffectTimer { get; set; }
        private const float MoveSpeed = -5f;

        public void Activate(Vector3 newPosition)
        {
            transform.position = newPosition;
            EffectTimer = 0;
            gameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            if (EffectTimer > 1f) return;
            EffectTimer += Time.deltaTime;
            transform.position -= Vector3.down * (MoveSpeed * Time.deltaTime);
        }
    }
}
