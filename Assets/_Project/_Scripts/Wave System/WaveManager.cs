using Praxi.Enemy.Base;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zone8.Events;

namespace Praxi.WaveSystem
{
    public enum WaveState { Idle, Spawning, WaitingForClear, Delay }

    public class WaveManager : MonoBehaviour
    {
        public event Action<int, int> WaveStarted;
        public event Action WaveEnded;
        public event Action<float> TimeToNextWaveUpdated;

        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private float _timeBetweenWaves = 5f;
        [SerializeField] private Transform[] _spawnPoints;

        private readonly List<EnemyStateMachine> _enemiesInWave = new();
        private int _waveToSpawnNumb = 1;
        private float _passedTime;
        private EventBinding<EnemyDieEvent> _enemyKilledBinding;

        public WaveState CurrentState { get; private set; } = WaveState.Idle;
        private bool _isSpawning;

        public bool DynamicSpawnActive { get; private set; } = true;



        private void Start()
        {
            CurrentState = WaveState.Spawning;
            _enemyKilledBinding = new EventBinding<EnemyDieEvent>(OnEnemyKilled);
            EventBus<EnemyDieEvent>.Register(_enemyKilledBinding);
        }

        private void OnDestroy()
        {
            EventBus<EnemyDieEvent>.Deregister(_enemyKilledBinding);
        }


        //Small state Machine No neede to implement a full one
        private void Update()
        {
            switch (CurrentState)
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

            CurrentState = WaveState.WaitingForClear;
            _isSpawning = false;
        }

        private void HandleWaitingForClear()
        {
            if (_enemiesInWave.Count == 0)
            {
                WaveEnded?.Invoke();
                _passedTime = 0f;
                CurrentState = WaveState.Delay;
            }
        }

        private void HandleDelay()
        {
            if (!DynamicSpawnActive) return;

            _passedTime += Time.deltaTime;
            TimeToNextWaveUpdated?.Invoke(Mathf.Max(0f, _timeBetweenWaves - _passedTime));

            if (_passedTime >= _timeBetweenWaves)
            {
                CurrentState = WaveState.Spawning;
            }
        }

        private void SpawnEnemy()
        {
            EnemyStateMachine enemy = _enemyFactory.Create();
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

        private void OnEnemyKilled(EnemyDieEvent data)
        {
            if (_enemiesInWave.Contains(data.Enemy))
                _enemiesInWave.Remove(data.Enemy);
        }


        [Button]
        public void SpawnNextWave() => CurrentState = WaveState.Spawning;


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
