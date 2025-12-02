using Praxi.Enemy.Base;
using UnityEngine;

namespace Praxi.Enemy
{
    public class ArcherEnemy : EnemyBase
    {
        public override void Attack(Transform target)
        {
            Debug.Log("Archer attacks the target!");
        }
    }
}
