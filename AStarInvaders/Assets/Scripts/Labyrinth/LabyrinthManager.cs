using System.Collections.Generic;
using AStar.Labyrinth.Cells;
using AStar.Labyrinth.Data;
using AStar.Behaviours;
using System.Drawing;
using UnityEngine;
using System.Linq;
using System;


namespace AStar.Labyrinth
{
    public class LabyrinthManager : Utilities.SingleBehaviour<LabyrinthManager>
    {
        [SerializeField] private GameObject _field;

        private List<List<LabyrinthCell>> _cells =
            new List<List<LabyrinthCell>>();
        private Action<Point> _onPlayerPathChange;

        public event Action<Point> OnPlayerPathChange
        {
            add { _onPlayerPathChange += value; }
            remove { _onPlayerPathChange -= value; }
        }
        public Point PlayerPosition { get; private set; }


        private void Start()
        {
            if (_field == null)
                return;

            var unsortedCells = _field.
                GetComponentsInChildren<LabyrinthCell>().ToList();

            for (int i = 0; i < unsortedCells.Count; i++)
            {
                var unorderedCell = unsortedCells[i];
                var row = GetRowIndex(unorderedCell);
                
                if (row >= _cells.Count)
                {
                    _cells.Insert(row, new List<LabyrinthCell>());
                    _cells[row].Add(unorderedCell);
                    continue;
                }
                
                var column = GetColumnIndex(row, unorderedCell);
                if(column == -1)
                    _cells[row].Insert(0, unorderedCell);
                else if(column < _cells[row].Count)
                    _cells[row].Insert(column, unorderedCell);
                else
                    _cells[row].Add(unorderedCell);
            }
        }


        private int GetRowIndex(LabyrinthCell cell)
        {
            if (_cells.Count == 0)
                return 0;
            for (int i = 0; i < _cells.Count; i++)
            {
                bool areSimilar = Mathf.Approximately(_cells[i][0].Position.y, cell.Position.y);
                if (areSimilar) return i;

                if (_cells[i][0].Position.y > cell.Position.y)
                {
                    _cells.Insert(i, new List<LabyrinthCell>());
                    return i;
                }

            }

            return _cells.Count;
        }

        private int GetColumnIndex(int row, LabyrinthCell cell)
        {
            if (_cells.Count == 0)
                return 0;
            for (int i = 0; i < _cells[row].Count; i++)
            {
                bool areSimilar = Mathf.Approximately(_cells[row][i].Position.x, cell.Position.x);
                if (areSimilar || (_cells[row][i].Position.x > cell.Position.x)) return i;
            }

            return _cells.Count;
        }
        
        private bool AreSimilar(LabyrinthCell cellA, LabyrinthCell cellB)
        {
            return Mathf.Approximately(cellA.Position.x, cellB.Position.x) && 
                   Mathf.Approximately(cellA.Position.y, cellB.Position.y);
        }


        public void ClearPathData()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                for (int j = 0; j < _cells[i].Count; j++)
                {
                    _cells[i][j].GCost = float.MaxValue;
                    _cells[i][j].HCost = float.MaxValue;
                    _cells[i][j].Owner = null;
                }
            }
        }

        public bool IsPointWalkable(int row, int column)
        {
            if (((row < 0) || (row >= _cells.Count)) ||
                ((column < 0) || (column >= _cells[row].Count)))
                return false;

            return (_cells[row][column].Type == CellType.Free) && !_cells[row][column].IsOccupied;
        }

        public LabyrinthCell GetCell(int row, int column)
        {
            if (((row < 0) || (row >= _cells.Count)) ||
                ((column < 0) || (column >= _cells[row].Count)))
                return null;
            return _cells[row][column];
        }
        
        public Vector3 GetCellPosition(int row, int column)
        {
            if (((row < 0) || (row >= _cells.Count)) ||
                ((column < 0) || (column >= _cells[row].Count)))
                throw new Exception("Wrong labyrinth position parameters");

            return _cells[row][column].Position;
        }
        
        public void UpdatePlayerPosition(LabyrinthCell cell)
        {
            PlayerPosition = GetCellFieldPosition(cell);
            _onPlayerPathChange?.Invoke(PlayerPosition);
        }

        public Point GetCellFieldPosition(LabyrinthCell cell)
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                for (int j = 0; j < _cells[i].Count; j++)
                {
                    if (_cells[i][j] == cell)
                        return new Point(i, j);
                }
            }

            throw new Exception("There is no such cell in the labyrinth");
        }
        
        public Entity.PathPosition GetPosition(Vector3 position)
        {
            var cellColumn = -1;
            var cellRow = -1;
            
            for (int i = 0; i < _cells.Count; i++)
            {
                bool areSimilar = Mathf.Approximately(_cells[i][0].Position.y, position.y);
                if (areSimilar)
                {
                    cellRow = i;
                    break;
                }
            }

            if (cellRow == -1)
                throw new Exception("No suitable row");
            for (int i = 0; i < _cells[cellRow].Count; i++)
            {
                bool areSimilar = Mathf.Approximately(_cells[cellRow][i].Position.x, position.x);
                if (areSimilar)
                {
                    cellColumn = i;
                    break;
                }
            }
            if (cellColumn == -1)
                throw new Exception("No suitable column");
            
            return new Entity.PathPosition(cellRow, cellColumn);
        }
    }
}