using Praxi.Enemy.Data;
using UnityEngine;
using UnityEngine.Pool;

namespace Praxi.Enemy.Base
{
    public abstract class EnemyBase : MonoBehaviour
    {
        protected EnemyBaseSO _data;
        protected IObjectPool<EnemyBase> _pool;


        public void Setup(EnemyBaseSO data, IObjectPool<EnemyBase> pool)
        {
            _data = data;
            _pool = pool;
        }


        public abstract void Attack(Transform target);

        public void Kill()
        {
            _pool.Release(this);
        }
    }
}
