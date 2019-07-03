using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class TopPanel : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _restartButton;

        private bool _isPaused;

        private void Awake()
        {
            _backButton.onClick.AddListener(BackButtonPressed);
            _pauseButton.onClick.AddListener(PauseButtonPressed);
            _restartButton.onClick.AddListener(RestartButtonPressed);
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(BackButtonPressed);
            _pauseButton.onClick.RemoveListener(PauseButtonPressed);
            _restartButton.onClick.RemoveListener(RestartButtonPressed);
        }

        private void BackButtonPressed()
        {
            _isPaused = false;
            Time.timeScale = 1f;

            SceneManager.LoadScene(0);
        }

        private void PauseButtonPressed()
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0f : 1f;
        }

        private void RestartButtonPressed()
        {
            _isPaused = false;
            Time.timeScale = 1f;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
