using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using AStar.Labyrinth.Data;
using UnityEngine;


namespace AStar.Behaviours
{
    public class Enemy : Entity
    {
        [SerializeField] private float _movementPeriod;
        
        private Coroutine _movementCoroutine;
        private List<Point> _destinationPath
            = new List<Point>();
        private Transform _transform;


        protected override void Awake()
        {
            base.Awake();

            _transform = transform;
        }

        private void OnDestroy()
        {
            Labyrinth.LabyrinthManager.Instance.OnPlayerPathChange -= RebuildPath;
            if(_movementCoroutine != null)
                StopCoroutine(_movementCoroutine);
        }
        
        
        private IEnumerator FollowPlayerRoutine()
        {
            var movementPeriod = new WaitForSeconds(_movementPeriod);

            while (General.LevelManager.Instance.PlayerAlive && _destinationPath.Count > 0)
            {
                var currentPoint = Labyrinth.LabyrinthManager.Instance.GetPosition(_transform.position);
                var nextPoint = _destinationPath[0];
                _destinationPath.RemoveAt(0);

                var horizontal = nextPoint.Y - currentPoint.ColumnPosiion;
                var vertical = nextPoint.X - currentPoint.RowPosition;
                var transitionAllowed = false;

                if (horizontal != 0.0f)
                {
                    transitionAllowed = ProcessNewInput(0, (int) horizontal);
                
                    _oldRotationZ = _targetRotationZ;
                    _targetRotationZ = (horizontal < 0.0f) ? Utilities.Constants.RotationAngles.ANGLE_LEFT : 
                        Utilities.Constants.RotationAngles.ANGLE_RIGHT;
                }
                else if (vertical != 0.0f)
                {
                    transitionAllowed = ProcessNewInput((int) vertical, 0);
                
                    _oldRotationZ = _targetRotationZ;
                    _targetRotationZ = (vertical < 0.0f) ? Utilities.Constants.RotationAngles.ANGLE_BOTTOM : 
                        Utilities.Constants.RotationAngles.ANGLE_TOP;
                }

                if (transitionAllowed && !Mathf.Approximately(_oldRotationZ, _targetRotationZ))
                    _rotationCoroutine = StartCoroutine(RotationRoutine());
                
                yield return movementPeriod;
            }

            _movementCoroutine = null;
        }
        
        protected override void StartInitialization()
        {
            base.StartInitialization();

            Labyrinth.LabyrinthManager.Instance.OnPlayerPathChange += RebuildPath;
            RebuildPath(Labyrinth.LabyrinthManager.Instance.PlayerPosition);

            _movementCoroutine = StartCoroutine(FollowPlayerRoutine());
        }
        
        private void RebuildPath(Point playerPosition)
        {
            _destinationPath = Labyrinth.PathGenerator.
                GenerateAPath(
                    new Point(_labyrinthPosition.RowPosition, _labyrinthPosition.ColumnPosiion),
                    playerPosition
                );
        }
        
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                General.LevelManager.Instance.KillPlayer();
                return;
            }

            var bullet = other.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                Destroy(bullet.gameObject);
                Destroy(gameObject);
            }
        }
    }
}