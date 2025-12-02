using Praxi.Enemy.Base;
using UnityEngine;

namespace Praxi.Enemy
{
    public class FighterEnemy : EnemyBase
    {

        public override void Attack(Transform target)
        {
            Debug.Log("FighterEnemy attacks the target!");
        }
    }
}
