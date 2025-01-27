using System.Collections;
using AStar.Utilities;
using UnityEngine;


namespace AStar.Behaviours
{
    [RequireComponent(typeof(PointsCarrier))]
    public class Entity : MonoBehaviour
    {
        [System.Serializable]
        public class PathPosition
        {
            public int ColumnPosiion;
            public int RowPosition;

            
            public PathPosition(int row, int column)
            {
                ColumnPosiion = column;
                RowPosition = row;
            }
        }

        
        [SerializeField] private GameObject _visual;
        [SerializeField] private float _rotationTime;
        [Header("Labyrinth")]
        [SerializeField] protected PathPosition _labyrinthPosition;
        
        protected PointsCarrier _moveController;
        protected Coroutine _rotationCoroutine;
        protected float _targetRotationZ;
        protected float _oldRotationZ;
        protected bool _initialized;


        protected virtual void Awake()
        {
            _moveController = GetComponent<PointsCarrier>();
        }

        protected void Update()
        {
            if(!_initialized)
                StartInitialization();
        }
        
        
        protected IEnumerator RotationRoutine()
        {
            var visualTransform = _visual.transform;
            float beginRotationZ = visualTransform.rotation.z;
            float rotationProgress = 0.0f;

            while (rotationProgress < _rotationTime)
            {
                var currentRotation = visualTransform.rotation.eulerAngles;
                rotationProgress += Time.deltaTime;

                visualTransform.rotation = Quaternion.Euler(
                    currentRotation.x, 
                    currentRotation.y, 
                    Mathf.Lerp(
                        beginRotationZ, 
                        _targetRotationZ, 
                        (rotationProgress / _rotationTime)
                    )
                );
                yield return null;
            }

            _rotationCoroutine = null;
        }
        
        protected virtual void StartInitialization()
        {
            _labyrinthPosition = Labyrinth.LabyrinthManager.Instance
                .GetPosition(transform.position);

            _targetRotationZ = 0.0f;
            _initialized = true;
        }
        
        protected bool ProcessNewInput(int rowShift, int columnShift)
        {
            var transitionAllowed = Labyrinth.LabyrinthManager.Instance.IsPointWalkable(
                _labyrinthPosition.RowPosition + rowShift,
                _labyrinthPosition.ColumnPosiion + columnShift
            );
            if (!transitionAllowed)
                return false;

            var cellPosition = Labyrinth.LabyrinthManager.Instance.GetCellPosition(
                _labyrinthPosition.RowPosition + rowShift,
                _labyrinthPosition.ColumnPosiion + columnShift
            );

            _labyrinthPosition.ColumnPosiion += columnShift;
            _labyrinthPosition.RowPosition += rowShift;
                
            _moveController.To = cellPosition;
            _moveController.StartTransition();
            return true;
        }
    }
}