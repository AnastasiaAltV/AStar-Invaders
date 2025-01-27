


namespace AStar.Labyrinth.Data
{
    [System.Flags]
    public enum Side { None, Top, Right, Bottom = 4, Left = 8, All = Top | Right | Bottom | Left }
    
    public enum CellType { None, Free, Wall }


    public class Operations
    {
        public Side OppositeSide(Side side)
        {
            return (side == Side.Top) ? Side.Bottom :
                (side == Side.Bottom) ? Side.Top :
                (side == Side.Left) ? Side.Right :
                (side == Side.Right) ? Side.Left : Side.None;

        }
    }
}