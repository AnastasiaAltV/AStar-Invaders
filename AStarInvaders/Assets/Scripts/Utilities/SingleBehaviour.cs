using UnityEngine;


namespace AStar.Utilities
{
    public class SingleBehaviour<T> : MonoBehaviour where T : SingleBehaviour<T>
    {
        public static T Instance { get; protected set; }

        
        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = (this as T);
        }
    }
}