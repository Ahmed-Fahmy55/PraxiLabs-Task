using Praxi.Enemy.Base;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Praxi.WaveSystem
{
    public class WaveManager : MonoBehaviour
    {
        public event Action<int, int> WaveStarted;
        public event Action WaveEnded;

        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private float _timeBetweenWaves = 5f;
        [SerializeField] private Transform[] _spawnPoints;

        private readonly List<EnemyBase> _enemiesInWave = new();
        private int _waveToSpawnNumb = 1;
        private float _passedTime;

        private enum WaveState { Idle, Spawning, WaitingForClear, Delay }
        private WaveState _state = WaveState.Idle;
        private bool _isSpawning;

        public bool DynamicSpawnActive { get; private set; } = true;
        public float TimeToNextWave =>
            DynamicSpawnActive && _state == WaveState.Delay ?
            Mathf.Max(0f, _timeBetweenWaves - _passedTime) : 0f;



        private void Start()
        {
            _state = WaveState.Spawning;
        }

        private void Update()
        {
            switch (_state)
            {
                case WaveState.Spawning:
                    HandleSpawning();
                    break;

                case WaveState.WaitingForClear:
                    HandleWaitingForClear();
                    break;

                case WaveState.Delay:
                    HandleDelay();
                    break;
            }
        }


        private void HandleSpawning()
        {
            if (_isSpawning) return;
            _isSpawning = true;
            StartCoroutine(SpawnWaveCoroutine());
        }

        private IEnumerator SpawnWaveCoroutine()
        {
            int spawnCount = GetEnemyCountForWave(_waveToSpawnNumb);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy();
                yield return null;
            }

            WaveStarted?.Invoke(_waveToSpawnNumb, spawnCount);
            _waveToSpawnNumb++;

            _state = WaveState.WaitingForClear;
            _isSpawning = false;
        }

        private void HandleWaitingForClear()
        {
            if (_enemiesInWave.Count == 0)
            {
                WaveEnded?.Invoke();
                _passedTime = 0f;
                _state = WaveState.Delay;
            }
        }

        private void HandleDelay()
        {
            if (!DynamicSpawnActive) return;

            _passedTime += Time.deltaTime;

            if (_passedTime >= _timeBetweenWaves)
            {
                _state = WaveState.Spawning;
            }
        }

        private void SpawnEnemy()
        {
            EnemyBase enemy = _enemyFactory.Create();
            enemy.transform.position = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)].position;

            _enemiesInWave.Add(enemy);
        }

        private int GetEnemyCountForWave(int wave)
        {
            if (wave == 1) return 30;
            if (wave == 2) return 50;
            if (wave == 3) return 70;
            return 70 + (wave - 3) * 10;
        }

        [Button]
        public void SpawnNextWave() => _state = WaveState.Spawning;


        [Button]
        public void ClearCurrentWave()
        {
            foreach (var enemy in _enemiesInWave)
                if (enemy != null) enemy.Kill();

            _enemiesInWave.Clear();
        }

        [Button]
        public void ToggleSpawn()
        {
            DynamicSpawnActive = !DynamicSpawnActive;
            _passedTime = 0f;
        }
    }
}
