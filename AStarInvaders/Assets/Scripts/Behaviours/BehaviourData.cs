


namespace AStar.Behaviours.Data
{
    [System.Serializable]
    public struct LabyrinthCellPosition
    {
        public int ColumnPosition;
        public int RowPosition;

        
        public LabyrinthCellPosition(int row, int column)
        {
            ColumnPosition = row;
            RowPosition = column;
        }
    }
}