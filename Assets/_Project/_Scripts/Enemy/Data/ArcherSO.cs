using UnityEngine;

namespace Praxi.Enemy.Data
{

    //TODO Can Be Refactord to Projectile Based Enemy SO later
    [CreateAssetMenu(fileName = "ArcherSO", menuName = "Praxi/Enemy/ArcherSO", order = 3)]
    public class ArcherSO : EnemyBaseSO
    {
        public GameObject Weapon;
        public float ProjectileSpeed = 15f;
        public bool IsTargetingMovingPlayer = true;

        // Additional archer-specific stats can be added here
    }
}
