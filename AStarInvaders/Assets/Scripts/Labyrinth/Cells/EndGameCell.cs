using UnityEngine;

namespace AStar.Labyrinth.Cells
{
    public class EndGameCell : LabyrinthCell
    {
        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Game end");
            General.LevelManager.Instance.EndGame();
        }
    }
}