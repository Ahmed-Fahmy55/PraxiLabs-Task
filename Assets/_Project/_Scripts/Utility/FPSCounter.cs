using System.Text;
using TMPro;
using UnityEngine;

namespace Praxi.Utility
{
    [RequireComponent(typeof(TMP_Text))]
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private float _updateRate = 0.5f;

        private TMP_Text _fpsText;
        private int _frames;
        private float _timer;
        private StringBuilder _sb = new StringBuilder(10);


        private void Awake()
        {
            _fpsText = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _frames++;
            _timer += Time.unscaledDeltaTime;

            if (_timer >= _updateRate)
            {
                float fps = _frames / _timer;
                _sb.Clear();
                _sb.Append("FPS: ").Append(Mathf.RoundToInt(fps));
                _fpsText.text = _sb.ToString();

                _frames = 0;
                _timer = 0;
            }
        }
    }
}
