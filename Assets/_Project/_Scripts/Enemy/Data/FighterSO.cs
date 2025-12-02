using UnityEngine;

namespace Praxi.Enemy.Data
{
    [CreateAssetMenu(fileName = "FighterSO", menuName = "Praxi/Enemy/FighterSO", order = 1)]
    public class FighterSO : EnemyBaseSO
    {
        public GameObject Weapon;
        // Additional fighter-specific stats can be added here
    }
}
