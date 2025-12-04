using System;
using UnityEngine;

namespace Praxi.Combat
{
    public class Health : MonoBehaviour
    {

        public event Action OnTakeDamage;
        public event Action OnDie;

        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private bool _isImuneToDamage = false;
        public bool IsDead { get; private set; }

        private int health;

        private void Start()
        {
            health = _maxHealth;
        }


        public void DealDamage(int damage)
        {
            if (IsDead) return;
            if (_isImuneToDamage) return;

            health = Mathf.Max(health - damage, 0);
            OnTakeDamage?.Invoke();
            if (health == 0)
            {
                OnDie?.Invoke();
                IsDead = true;
            }
        }
    }
}
