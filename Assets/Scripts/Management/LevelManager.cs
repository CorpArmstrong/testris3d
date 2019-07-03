using Assets.Scripts.Entity;
using Assets.Scripts.Events;
using Assets.Scripts.Game;
using Assets.Scripts.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Management
{
    public class LevelManager : MonoBehaviour
    {
        private const float AccelerationStep = 0.012f;
        private const float MinAcceleration = 0.18f;

        [SerializeField] private FigureSpawner _figureSpawner;
        [SerializeField] private MovementPanel _movementPanel;
        [SerializeField] private GameObject _gameOverPanel;

        private GameField _gameField;

        private UnityAction _nextFigureListener;
        private UnityAction _gameOverListener;

        private float _currentAcceleration = 0.55f;
        
        private void OnEnable()
        {
            _gameField = new GameField();
            _figureSpawner.Initialize(_gameField);
            _movementPanel.UpdateDirection += UpdateDirection;
            _nextFigureListener = new UnityAction(MakeStep);
            _gameOverListener = new UnityAction(GameOver);
            EventBus.StartListening(EventConstants.Stacked, _nextFigureListener);
            EventBus.StartListening(EventConstants.GameOver, _gameOverListener);

            MakeStep();
        }

        private void OnDisable()
        {
            _movementPanel.UpdateDirection -= UpdateDirection;
            EventBus.StopListening(EventConstants.Stacked, _nextFigureListener);
            EventBus.StopListening(EventConstants.GameOver, _gameOverListener);
        }

        private void MakeStep()
        {
            _currentAcceleration -= AccelerationStep;

            if (_currentAcceleration < MinAcceleration)
            {
                _currentAcceleration = MinAcceleration;
            }

            _figureSpawner.SpawnRandomFigure(_currentAcceleration);
        }

        private void GameOver()
        {
            _gameOverPanel.SetActive(true);
        }

        private void UpdateDirection(MoveDirection moveDirection)
        {
            if (_figureSpawner != null && _figureSpawner.CurrentFigure != null)
            {
                _figureSpawner.CurrentFigure.SetMoveDirection(moveDirection);
            }
        }
    }
}
