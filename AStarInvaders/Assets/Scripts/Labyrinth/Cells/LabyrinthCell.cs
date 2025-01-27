using UnityEngine;


namespace AStar.Labyrinth.Cells
{
    public class LabyrinthCell : MonoBehaviour
    {
        [SerializeField] private Data.CellType _type;

        #region AStar
        public LabyrinthCell Owner { get; set; }
        public float GCost { get; set; } = float.MaxValue;
        public float HCost { get; set; } = float.MaxValue;
        public float FCost
        {
            get => GCost + HCost;
        }
        #endregion
        
        public bool IsOccupied { get; protected set; }
        public Vector3 Position { get; private set; }
        public Sprite Image { get; set; }
        public Data.CellType Type
        {
            get { return _type; }
        }


        protected virtual void Awake()
        {
            Image = GetComponent<Sprite>();
            Position = transform.position;
        }


        public void Occupy()
        {
            IsOccupied = true;
        }

        public void Deoccupy()
        {
            IsOccupied = false;
        }
        
        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("New player position");
            LabyrinthManager.Instance.UpdatePlayerPosition(this);
        }
    }
}