using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Events;
using Assets.Scripts.Game;

namespace Assets.Scripts.Entity
{
    public class Figure : MonoBehaviour
    {
        private const float DefaultFallTime = 1f;

        [SerializeField] private bool _canRotate;

        private readonly Vector3 _leftPosition = new Vector3(-1f, 0f, 0f);
        private readonly Vector3 _rightPosition = new Vector3(1f, 0f, 0f);
        private readonly Vector3 _rotateRightPosition = new Vector3(0f, 0f, -90f);
        private readonly Vector3 _rotateLeftPosition = new Vector3(0f, 0f, 90f);
        private readonly Vector3 _fallPosition = new Vector3(0f, -1f, 0f);

        private float _lastFallTime; // Time.time since last fall
        private float _acceleration;

        private GameField _gameField;
        private MoveDirection _moveDirection;

        private bool _isPlayable;

        public void Initialize(GameField gameField, float acceleration)
        {
            _gameField = gameField;
            _acceleration = acceleration;

            if (!_gameField.IsFreePlace(transform))
            {
                GameOver();
            }
            else
            {
                _isPlayable = true;
            }
        }

        public void SetMoveDirection(MoveDirection moveDirection)
        {
            _moveDirection = moveDirection;
        }

        private void GameOver()
        {
            _isPlayable = false;
            enabled = false;
            EventBus.TriggerEvent(EventConstants.GameOver);
        }

        private void UpdateNextFigure()
        {
            _gameField.DecreaseWholeRows();
            _isPlayable = false;
            enabled = false;
            EventBus.TriggerEvent(EventConstants.Stacked);
        }

        #region Input handling
        private void Update()
        {
            if (!_isPlayable)
            {
                return;
            }

            if (!_gameField.IsFreePlace(transform))
            {
                GameOver();
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || _moveDirection == MoveDirection.Down)
            {
                MoveDown();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || _moveDirection == MoveDirection.Left)
            {
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || _moveDirection == MoveDirection.Right)
            {
                MoveRight();
            }
            else if (_canRotate && (Input.GetKeyDown(KeyCode.UpArrow) || _moveDirection == MoveDirection.Rotate))
            {
                Rotate();
            }
            else if (Time.time - _lastFallTime >= DefaultFallTime * _acceleration)
            {
                FreeFall();
            }

            _moveDirection = MoveDirection.None;
        }
        #endregion

        #region Move commands

        private void MoveDown()
        {
            var cellFreeItems = new List<int>();

            foreach (Transform child in transform)
            {
                Vector2 v = _gameField.GetRoundedPosition(child.position);
                cellFreeItems.Add(_gameField.FindClosestFreePosition(transform, v));
            }

            int minMoveCount = cellFreeItems.Min();

            if (minMoveCount > 0)
            {
                transform.position += new Vector3(0f, -minMoveCount, 0f);
                _gameField.UpdateFigurePosition(transform);

                UpdateNextFigure();
            }
        }

        private void MoveLeft()
        {
            if (_gameField.IsNextPositionValid(transform, _leftPosition))
            {
                transform.position += _leftPosition;
                _gameField.UpdateFigurePosition(transform);
            }
        }

        private void MoveRight()
        {
            if (_gameField.IsNextPositionValid(transform, _rightPosition))
            {
                transform.position += _rightPosition;
                _gameField.UpdateFigurePosition(transform);
            }
        }

        private void Rotate()
        {
            transform.Rotate(_rotateRightPosition);

            if (_gameField.IsNextRotationValid(transform))
            {
                _gameField.UpdateFigurePosition(transform);
            }
            else
            {
                transform.Rotate(_rotateLeftPosition);
            }
        }

        private void FreeFall()
        {
            if (_gameField.IsNextPositionValid(transform, _fallPosition))
            {
                transform.position += _fallPosition;
                _gameField.UpdateFigurePosition(transform);
            }
            else
            {
                UpdateNextFigure();
            }

            _lastFallTime = Time.time;
        }
        #endregion
    }
}
