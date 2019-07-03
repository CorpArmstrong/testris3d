using Assets.Scripts.Entity;
using Assets.Scripts.Events;
using Assets.Scripts.Game;
using UnityEngine;

namespace Assets.Scripts.Management
{
    public class FigureSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _figureList;

        private Figure _currentFigure;

        public Figure CurrentFigure
        {
            get
            {
                return _currentFigure;
            }
        }

        private int _currentRandomIndex;
        private int _nextRandomIndex = -1;

        private GameField _gameField;

        public void Initialize(GameField gameField)
        {
            _gameField = gameField;
        }

        public void SpawnRandomFigure(float acceleration)
        {
            _currentFigure = null;

            _currentRandomIndex = _nextRandomIndex == -1 ? Random.Range(0, _figureList.Length) : _nextRandomIndex;
            _nextRandomIndex = Random.Range(0, _figureList.Length);

            EventBus.TriggerEvent(EventConstants.NextFigureIndex, _nextRandomIndex);

            var figureObject = Instantiate(_figureList[_currentRandomIndex], transform.position, Quaternion.identity);
            var figure = figureObject.GetComponent<Figure>();
            figure.Initialize(_gameField, acceleration);

            _currentFigure = figure;
        }
    }
}
