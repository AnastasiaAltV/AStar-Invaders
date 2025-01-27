using System.Collections.Generic;
using AStar.Labyrinth.Data;
using System.Drawing;
using System.Linq;
using UnityEngine;
using System;


namespace AStar.Labyrinth
{
    public static class PathGenerator
    {

        private static List<Cells.LabyrinthCell> GetNeighbours(Point center)
        {
            var neighbours = new List<Cells.LabyrinthCell>();

            neighbours.Add(LabyrinthManager.Instance.GetCell(center.X, center.Y - 1));
            neighbours.Add(LabyrinthManager.Instance.GetCell(center.X,center.Y + 1));
            neighbours.Add(LabyrinthManager.Instance.GetCell(center.X - 1, center.Y));
            neighbours.Add(LabyrinthManager.Instance.GetCell(center.X + 1, center.Y));

            return neighbours.Where(cell => cell != null && cell.Type == CellType.Free).ToList();
        }
        
        public static List<Point> GenerateAPath(Point fromPosition, Point toPosition)
        {
            var closedQueue = new List<Cells.LabyrinthCell>();
            var openQueue = new List<Cells.LabyrinthCell>();

            var startCell = LabyrinthManager.Instance.GetCell(fromPosition.X, fromPosition.Y);
            var endCell = LabyrinthManager.Instance.GetCell(toPosition.X, toPosition.Y);
            var currentCell = (Cells.LabyrinthCell)null;

            startCell.GCost = 0.0f;
            startCell.HCost = 0.0f;
            
            openQueue.Add(startCell);
            
            do
            {
                currentCell = openQueue.OrderBy(cell => cell.FCost).First();
                openQueue.Remove(currentCell);
                closedQueue.Add(currentCell);

                if (currentCell == endCell)
                    break;

                var currentNeighbours = GetNeighbours(
                    LabyrinthManager.Instance.GetCellFieldPosition(currentCell)
                );
                
                foreach (var neighbourCell in currentNeighbours)
                {
                    if(closedQueue.Contains(neighbourCell))
                        continue;
                    var newHCost = CalculateCellCost(neighbourCell, endCell);
                    var newGCost = (currentCell.GCost + CalculateCellCost(neighbourCell, currentCell));
                    var newFCost = newHCost + newGCost;

                    if ((newFCost < neighbourCell.FCost) || !openQueue.Contains(neighbourCell))
                    {
                        neighbourCell.GCost = newGCost;
                        neighbourCell.HCost = newHCost;

                        neighbourCell.Owner = currentCell;
                        if(!openQueue.Contains(neighbourCell))
                            openQueue.Add(neighbourCell);
                    }
                }

            } while (openQueue.Count != 0);

            // If complete path wasn't found
            if (currentCell != endCell)
            {
                LabyrinthManager.Instance.ClearPathData();
                return null;
            }

            // If complete path exist
            var completePath = new List<Point>();
            while (currentCell != null && currentCell != startCell)
            {
                completePath.Insert(0, LabyrinthManager.Instance.GetCellFieldPosition(currentCell));
                currentCell = currentCell.Owner;
            }
            LabyrinthManager.Instance.ClearPathData();
            
            return completePath;
        }
        
        private static float CalculateCellCost(Cells.LabyrinthCell currentCell, Cells.LabyrinthCell anotherCell)
        {
            var currentPosition = LabyrinthManager.Instance.GetCellFieldPosition(currentCell);
            var toPosition = LabyrinthManager.Instance.GetCellFieldPosition(anotherCell);
            
            return Mathf.Abs(currentPosition.X - toPosition.X) + Math.Abs(currentPosition.Y - toPosition.Y);
        }
    }
}