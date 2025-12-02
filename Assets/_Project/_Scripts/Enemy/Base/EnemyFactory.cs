using Praxi.Enemy.Data;
using Praxi.Factory;
using UnityEngine;
using UnityEngine.Pool;

namespace Praxi.Enemy.Base
{
    [CreateAssetMenu(fileName = "EnemyFactory", menuName = "Praxi/Factory/Enemy Factory")]
    public class EnemyFactory : ScriptableObject, IFactory<EnemyBase>
    {
        [Header("Pool Settings")]
        [SerializeField] private int _initialPoolSize = 20;
        [SerializeField] private int _maxPoolSize = 1000;

        [Header("Enemies Data")]
        [SerializeField] private EnemyBaseSO[] _enemiesData;

        private IObjectPool<EnemyBase> _pool;


        public EnemyBase Create()
        {
            if (_pool == null)
                IntializePool();

            return _pool.Get();
        }

        private void IntializePool()
        {
            _pool = new ObjectPool<EnemyBase>(
             CreateEnemyInstance, OnGet, OnRelease, OnInstanceDestroy, true, _initialPoolSize, _maxPoolSize);
        }

        private void OnInstanceDestroy(EnemyBase enemy)
        {
            Destroy(enemy.gameObject);
        }

        private void OnRelease(EnemyBase enemy)
        {
            enemy.gameObject.SetActive(false);
        }

        private void OnGet(EnemyBase enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        private EnemyBase CreateEnemyInstance()
        {
            var data = _enemiesData[Random.Range(0, _enemiesData.Length)];
            EnemyBase enemy = Instantiate(data.Prefab);
            enemy.Setup(data, _pool);

            return enemy;
        }
    }
}
