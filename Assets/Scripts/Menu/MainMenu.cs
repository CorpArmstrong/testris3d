using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _exitGameButton;

        private void Awake()
        {
            _newGameButton.onClick.AddListener(NewGameButtonPressed);
            _exitGameButton.onClick.AddListener(ExitGameButtonPressed);
        }

        private void OnDestroy()
        {
            _newGameButton.onClick.RemoveListener(NewGameButtonPressed);
            _exitGameButton.onClick.AddListener(ExitGameButtonPressed);
        }

        private void NewGameButtonPressed()
        {
            SceneManager.LoadScene(1);
        }

        private void ExitGameButtonPressed()
        {
            Application.Quit();
        }
    }
}
