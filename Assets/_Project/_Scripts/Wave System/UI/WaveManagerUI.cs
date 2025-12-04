using Praxi.Enemy.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zone8.Events;

namespace Praxi.WaveSystem.UI
{
    public class WaveManagerUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _waveNumbText;
        [SerializeField] private TextMeshProUGUI _activeEnmiesText;
        [SerializeField] private TextMeshProUGUI _timeToNextWaveText;
        [SerializeField] private TextMeshProUGUI _spawnStatusText;

        [SerializeField] private Button _startNextWaveButton;
        [SerializeField] private Button _stopButton;
        [SerializeField] private Button _destroyCurrentWaveButton;

        private WaveManager _waveManager;
        EventBinding<EnemyDieEvent> _enemyDieBinding;

        private int _enemiesCount;

        private void Awake()
        {
            _waveManager = FindAnyObjectByType<WaveManager>();
        }

        private void OnEnable()
        {
            _startNextWaveButton.onClick.AddListener(SpawnNextWave);
            _stopButton.onClick.AddListener(OnStopButtonClicked);
            _destroyCurrentWaveButton.onClick.AddListener(OnDestroyCurrentWaveClicked);

            _waveManager.WaveStarted += WaveManager_OnWaveStartd;
            _waveManager.WaveEnded += WaveManager_OnWaveEnded;
            _waveManager.TimeToNextWaveUpdated += WaveManager_TimeToNextWaveUpdated;
            _enemyDieBinding = new EventBinding<EnemyDieEvent>(OnEnemyDied);
            EventBus<EnemyDieEvent>.Register(_enemyDieBinding);

        }


        private void OnDisable()
        {
            _destroyCurrentWaveButton.onClick.RemoveListener(OnDestroyCurrentWaveClicked);
            _stopButton.onClick.RemoveListener(OnStopButtonClicked);
            _startNextWaveButton.onClick.RemoveListener(SpawnNextWave);


            _waveManager.WaveStarted -= WaveManager_OnWaveStartd;
            _waveManager.WaveEnded -= WaveManager_OnWaveEnded;
            _waveManager.TimeToNextWaveUpdated -= WaveManager_TimeToNextWaveUpdated;

            EventBus<EnemyDieEvent>.Deregister(_enemyDieBinding);
        }

        private void WaveManager_TimeToNextWaveUpdated(float remainingTime)
        {
            _timeToNextWaveText.SetText("Time to next wave: {0}", Mathf.CeilToInt(remainingTime));
        }

        private void WaveManager_OnWaveEnded()
        {
            _timeToNextWaveText.gameObject.SetActive(_waveManager.DynamicSpawnActive);
        }

        private void WaveManager_OnWaveStartd(int waveNumb, int enemiesCount)
        {
            _enemiesCount = enemiesCount;
            _waveNumbText.text = $"Wave numbre: {waveNumb.ToString()}";
            _activeEnmiesText.text = $"Active enemies: {enemiesCount.ToString()}";
            _timeToNextWaveText.gameObject.SetActive(false);
        }

        private void OnDestroyCurrentWaveClicked()
        {
            _waveManager.ClearCurrentWave();
        }

        private void OnStopButtonClicked()
        {
            _waveManager.ToggleSpawn();
            _spawnStatusText.text = _waveManager.DynamicSpawnActive ? "Stop" : "Resume";

            if (_waveManager.CurrentState != WaveState.Delay) return;

            _timeToNextWaveText.gameObject.SetActive(_waveManager.DynamicSpawnActive);
        }

        private void SpawnNextWave()
        {
            _waveManager.SpawnNextWave();
        }

        private void OnEnemyDied(EnemyDieEvent @event)
        {
            _enemiesCount--;
            _activeEnmiesText.text = $"Active enemies: {_enemiesCount.ToString()}";
        }

    }
}
