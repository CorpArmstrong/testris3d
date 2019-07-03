using Assets.Scripts.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class InfoPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreLabel;
        [SerializeField] private Image _nextFigureImage;
        [SerializeField] private Sprite[] _imageList;

        private void OnEnable()
        {
            _scoreLabel.text = string.Empty;

            EventBus.StartListening("NextFigureIndex", SetNextFigure);
            EventBus.StartListening("UpdateScore", SetScorePoints);
        }

        private void OnDisable()
        {
            EventBus.StopListening("NextFigureIndex", SetNextFigure);
            EventBus.StopListening("UpdateScore", SetScorePoints);
        }

        private void SetNextFigure(int index)
        {
            if (index >= 0 && index < _imageList.Length)
            {
                _nextFigureImage.overrideSprite = _imageList[index];
            }
        }

        private void SetScorePoints(int pointCount)
        {
            _scoreLabel.text = pointCount.ToString();
        }
    }
}
