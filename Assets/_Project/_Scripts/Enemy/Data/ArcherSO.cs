using UnityEngine;

namespace Praxi.Enemy.Data
{

    [CreateAssetMenu(fileName = "ArcherSO", menuName = "Praxi/Enemy/ArcherSO", order = 3)]
    public class ArcherSO : EnemyBaseSO
    {
        public GameObject Weapon;

        // Additional archer-specific stats can be added here
    }
}
