using EngineInput = UnityEngine.Input;
using AStar.Utilities;
using UnityEngine;


namespace AStar.Behaviours
{
    public class InputHandler : SingleBehaviour<InputHandler>
    {
        [SerializeField] private float _horizontal;
        [SerializeField] private float _vertical;
        [SerializeField] private bool _space;

        public float Horizontal
        {
            get => _horizontal;
        }
        public float Vertical
        {
            get => _vertical;
        }
        public bool Space
        {
            get => _space;
        }
        
        
        private void Update()
        {
            _horizontal = EngineInput.GetAxisRaw("Horizontal");
            _vertical = EngineInput.GetAxisRaw("Vertical");

            _space = EngineInput.GetKeyDown(KeyCode.Space);
        }
    }
}