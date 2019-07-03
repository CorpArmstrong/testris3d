using Assets.Scripts.Entity;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class MovementPanel : MonoBehaviour
    {
        public Action<MoveDirection> UpdateDirection;

        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _downButton;
        [SerializeField] private Button _rotateButton;

        private void OnEnable()
        {
            _leftButton.onClick.AddListener(delegate { SetInput(MoveDirection.Left); });
            _rightButton.onClick.AddListener(delegate { SetInput(MoveDirection.Right); });
            _downButton.onClick.AddListener(delegate { SetInput(MoveDirection.Down); });
            _rotateButton.onClick.AddListener(delegate { SetInput(MoveDirection.Rotate); });
        }

        private void OnDisable()
        {
            _leftButton.onClick.RemoveAllListeners();
            _rightButton.onClick.RemoveAllListeners();
            _downButton.onClick.RemoveAllListeners();
            _rotateButton.onClick.RemoveAllListeners();
        }

        private void SetInput(MoveDirection moveDirection)
        {
            if (UpdateDirection != null)
            {
                UpdateDirection(moveDirection);
            }
        }
    }
}
