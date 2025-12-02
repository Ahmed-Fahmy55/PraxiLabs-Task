using Praxi.Enemy.Base;
using UnityEngine;

namespace Praxi.Enemy
{
    public class MageEnemy : EnemyBase
    {
        public override void Attack(Transform target)
        {
            Debug.Log("Mage Enemy attacks the target!");
        }
    }
}
