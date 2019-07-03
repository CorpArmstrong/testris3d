using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameField
    {
        public readonly int RowCount = 10;
        public readonly int ColumnCount = 20;

        [SerializeField] public Transform[,] FigureGrid;

        private Vector3 _moveVector = new Vector3(0f, -1f, 0f);
        private int _rowsDecreasedCount;

        public GameField()
        {
            FigureGrid = new Transform[RowCount, ColumnCount];
        }

        public Vector2 GetRoundedPosition(Vector2 position)
        {
            return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
        }

        public bool IsInBounds(Vector2 position)
        {
            return (int)position.x >= 0 && (int)position.x < RowCount && (int)position.y >= 0;
        }

        public void DeleteRow(int y)
        {
            for (int x = 0; x < RowCount; x++)
            {
                GameObject.Destroy(FigureGrid[x, y].gameObject);
                FigureGrid[x, y] = null;
            }
        }

        public void DecreaseRow(int y)
        {
            for (int x = 0; x < RowCount; x++)
            {
                if (FigureGrid[x, y] != null)
                {
                    FigureGrid[x, y - 1] = FigureGrid[x, y];
                    FigureGrid[x, y] = null;
                    FigureGrid[x, y - 1].position += _moveVector;
                }
            }
        }

        public void DecreaseRowsAbove(int y)
        {
            for (int i = y; i < ColumnCount; i++)
            {
                DecreaseRow(i);
            }
        }

        public bool IsRowFull(int y)
        {
            for (int x = 0; x < RowCount; x++)
            {
                if (FigureGrid[x, y] == null)
                {
                    return false;
                }
            }

            return true;
        }

        public void DecreaseWholeRows()
        {
            for (int y = 0; y < ColumnCount; y++)
            {
                if (IsRowFull(y))
                {
                    DeleteRow(y);
                    _rowsDecreasedCount++;
                    DecreaseRowsAbove(y + 1);
                    y--;
                }
            }

            EventBus.TriggerEvent(EventConstants.DecreasedRow, _rowsDecreasedCount);
            _rowsDecreasedCount = 0;
        }

        public int FindClosestFreePosition(Transform figureTrans, Vector2 pos)
        {
            for (int y = (int)pos.y; y >= 0; y--)
            {
                if (FigureGrid[(int)pos.x, y] != null)
                {
                    foreach (Transform child in figureTrans)
                    {
                        if (FigureGrid[(int)pos.x, y].parent != child.parent)
                        {
                            int occupiedPos = y + 1; // move up from occupied pos
                            return (int)pos.y - occupiedPos;
                        }
                    }
                }
            }

            return (int)pos.y;
        }

        public bool IsFreePlace(Transform figureTrans)
        {
            foreach (Transform child in figureTrans)
            {
                Vector2 v = GetRoundedPosition(child.position);

                if (FigureGrid[(int)v.x, (int)v.y] != null &&
                    FigureGrid[(int)v.x, (int)v.y].parent != child.parent)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsNextPositionValid(Transform figureTrans, Vector3 posToCheck)
        {
            foreach (Transform child in figureTrans)
            {
                Vector2 pos = GetRoundedPosition(child.position + posToCheck);

                if (!IsInBounds(pos))
                {
                    return false;
                }

                if (FigureGrid[(int)pos.x, (int)pos.y] != null &&
                    FigureGrid[(int)pos.x, (int)pos.y].parent != figureTrans)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsNextRotationValid(Transform figureTrans)
        {
            foreach (Transform child in figureTrans)
            {
                Vector2 pos = GetRoundedPosition(child.position);

                if (!IsInBounds(pos))
                {
                    return false;
                }

                if (FigureGrid[(int)pos.x, (int)pos.y] != null &&
                    FigureGrid[(int)pos.x, (int)pos.y].parent != figureTrans)
                {
                    return false;
                }
            }

            return true;
        }

        public void UpdateFigurePosition(Transform figureTrans)
        {
            // Update our figure position
            for (int y = 0; y < ColumnCount; y++)
            {
                for (int x = 0; x < RowCount; x++)
                {
                    if (FigureGrid[x, y] != null)
                    {
                        if (FigureGrid[x, y].parent == figureTrans)
                        {
                            FigureGrid[x, y] = null;
                        }
                    }
                }
            }

            // Write actual block positions
            foreach (Transform child in figureTrans)
            {
                Vector2 v = GetRoundedPosition(child.position);
                FigureGrid[(int)v.x, (int)v.y] = child;
            }
        }
    }
}
