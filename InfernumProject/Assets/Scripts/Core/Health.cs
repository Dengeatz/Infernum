using System;
using UnityEngine;

namespace Infernum.FPS.Core
{
    /// <summary>
    /// Простая реализация здоровья; может висеть на игроке и на враге.
    /// </summary>
    public sealed class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;

        private float _current;

        public float Current => _current;
        public float Max => maxHealth;
        public bool IsAlive => _current > 0f;

        public event Action<float, float> HealthChanged;
        public event Action<GameObject> Died;

        private void Awake()
        {
            _current = maxHealth;
        }

        public void TakeDamage(float damage, GameObject instigator)
        {
            if (!IsAlive || damage <= 0f)
            {
                return;
            }

            _current = Mathf.Max(0f, _current - damage);
            HealthChanged?.Invoke(_current, maxHealth);

            if (_current <= 0f)
            {
                Died?.Invoke(instigator);
            }
        }

        public void ResetHealth()
        {
            _current = maxHealth;
            HealthChanged?.Invoke(_current, maxHealth);
        }
    }
}
