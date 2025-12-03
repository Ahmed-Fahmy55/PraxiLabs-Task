using Praxi.Enemy.Data;
using Praxi.Factory;
using UnityEngine;
using UnityEngine.Pool;

namespace Praxi.Enemy.Base
{
    [CreateAssetMenu(fileName = "EnemyFactory", menuName = "Praxi/Factory/Enemy Factory")]
    public class EnemyFactory : ScriptableObject, IFactory<EnemyStateMachine>
    {
        [Header("Pool Settings")]
        [SerializeField] private int _initialPoolSize = 20;
        [SerializeField] private int _maxPoolSize = 1000;

        [Header("Enemies Data")]
        [SerializeField] private EnemyBaseSO[] _enemiesData;

        private IObjectPool<EnemyStateMachine> _pool;


        public EnemyStateMachine Create()
        {
            if (_pool == null)
                IntializePool();

            return _pool.Get();
        }

        private void IntializePool()
        {
            _pool = new ObjectPool<EnemyStateMachine>(
             CreateEnemyInstance, OnGet, OnRelease, OnInstanceDestroy, true, _initialPoolSize, _maxPoolSize);
        }

        private void OnInstanceDestroy(EnemyStateMachine enemy)
        {
            Destroy(enemy.gameObject);
        }

        private void OnRelease(EnemyStateMachine enemy)
        {
            enemy.gameObject.SetActive(false);
        }

        private void OnGet(EnemyStateMachine enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        private EnemyStateMachine CreateEnemyInstance()
        {
            var data = _enemiesData[Random.Range(0, _enemiesData.Length)];
            EnemyStateMachine enemy = Instantiate(data.Prefab);
            enemy.Setup(data, _pool);

            return enemy;
        }
    }
}
