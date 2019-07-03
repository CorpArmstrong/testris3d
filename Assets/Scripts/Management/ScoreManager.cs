using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts.Management
{
    public class ScoreManager : MonoBehaviour
    {
        private const int OneFigurePointCount = 8;
        private const int OneLineScoreCount = 100;
        private const int TwoLineScoreCount = 300;
        private const int ThreeLineScoreCount = 700;
        private const int FourLineScoreCount = 1500;

        private int _totalScore;

        private void OnEnable()
        {
            _totalScore = 0;
            EventBus.StartListening(EventConstants.DecreasedRow, CountPoints);
        }

        private void OnDisable()
        {
            EventBus.StopListening(EventConstants.DecreasedRow, CountPoints);
        }

        private void CountPoints(int decreasedRowCount)
        {
            // Giving at least 8 point for figure
            _totalScore += OneFigurePointCount;

            switch (decreasedRowCount)
            {
                case 1:
                    _totalScore += OneLineScoreCount;
                    break;
                case 2:
                    _totalScore += TwoLineScoreCount;
                    break;
                case 3:
                    _totalScore += ThreeLineScoreCount;
                    break;
                case 4:
                    _totalScore += FourLineScoreCount;
                    break;
                default:
                    break;
            }

            EventBus.TriggerEvent(EventConstants.UpdateScore, _totalScore);
        }
    }
}
