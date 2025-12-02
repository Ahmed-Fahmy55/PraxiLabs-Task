using Praxi.Enemy.Base;
using UnityEngine;

namespace Praxi.Enemy.Data
{
    public abstract class EnemyBaseSO : ScriptableObject
    {
        [Header("Reference Prefab")]
        public EnemyBase Prefab;

        [Header("Stats")]
        public float MaxHealth = 100;
        public float MoveSpeed = 3;

        [Space]
        public float AttackRange = 2;
        public float AttackCooldown = 1.5f;
        public float DetectionRange = 5;
        public float Damage = 10;
    }
}
