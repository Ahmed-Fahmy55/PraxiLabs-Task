using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Praxi.WaveSystem.UI
{
    public class WaveManagerUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _waveNumbText;
        [SerializeField] private TextMeshProUGUI _activeEnmiesText;

        [SerializeField] private Button _startNextWaveButton, _stopButton, _destroyCurrentWaveButton;

        private WaveManager _waveManager;

        private void Awake()
        {
            _waveManager = FindAnyObjectByType<WaveManager>();
        }

        private void OnEnable()
        {
            _startNextWaveButton.onClick.AddListener(SpawnNextWave);
            _stopButton.onClick.AddListener(OnStopButtonClicked);
            _destroyCurrentWaveButton.onClick.AddListener(OnDestroyCurrentWaveClicked);

        }


        private void OnDisable()
        {
            _destroyCurrentWaveButton.onClick.RemoveListener(OnDestroyCurrentWaveClicked);
            _stopButton.onClick.RemoveListener(OnStopButtonClicked);
            _startNextWaveButton.onClick.RemoveListener(SpawnNextWave);
        }

        private void OnDestroyCurrentWaveClicked()
        {

        }

        private void OnStopButtonClicked()
        {

        }

        private void SpawnNextWave()
        {

        }

    }
}
