using Praxi.Enemy.Base;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Praxi.Enemy
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private float _timeBetweenWaves = 5f;
        [SerializeField] private Transform[] _spawnPoints;

        private List<EnemyBase> _enemiesInWave = new();
        private int _currentWave = 1;
        private int _aliveEnemies = 0;

        private void Start()
        {
            StartCoroutine(StartWaveRoutine());
        }

        private IEnumerator StartWaveRoutine()
        {
            while (true)
            {
                int enemiesToSpawn = GetEnemyCountForWave(_currentWave);
                Debug.Log($"Starting Wave {_currentWave}, Spawning: {enemiesToSpawn}");

                yield return StartCoroutine(SpawnWave(enemiesToSpawn));

                // Wait for all enemies to die
                while (_aliveEnemies > 0)
                    yield return null;

                Debug.Log($"Wave {_currentWave} completed. Next wave in {_timeBetweenWaves}s");

                yield return new WaitForSeconds(_timeBetweenWaves);
                _currentWave++;
            }
        }

        private IEnumerator SpawnWave(int count)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnEnemy();
                yield return null;
            }
        }

        private void SpawnEnemy()
        {
            EnemyBase enemy = _enemyFactory.Create();
            enemy.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
            _aliveEnemies++;
            _enemiesInWave.Add(enemy);
        }

        private int GetEnemyCountForWave(int wave)
        {
            if (wave == 1) return 30;
            if (wave == 2) return 50;
            if (wave == 3) return 70;

            return 70 + (wave - 3) * 10;
        }



        ///Testing Purpose

        [Button]
        public void ClearCurrentWave()
        {
            foreach (var enemy in _enemiesInWave)
            {
                if (enemy != null)
                {
                    enemy.Kill();
                }
            }
            _enemiesInWave.Clear();
            _aliveEnemies = 0;
        }

        [Button]
        public void SpawnNextWave()
        {
            StopAllCoroutines();
            ClearCurrentWave();
            _currentWave++;
            StartCoroutine(StartWaveRoutine());
        }
    }
}
