using UnityEngine;

namespace Praxi.Enemy.Data
{
    [CreateAssetMenu(fileName = "MageSO", menuName = "Praxi/Enemy/MageSO", order = 2)]
    public class MageSO : EnemyBaseSO
    {
        public GameObject ProjectilePrefab;
        // Additional mage-specific stats can be added here
    }
}
