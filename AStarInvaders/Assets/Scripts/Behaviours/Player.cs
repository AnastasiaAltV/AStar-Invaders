using Unity.Mathematics;
using System.Drawing;
using UnityEngine;


namespace AStar.Behaviours
{
    public class Player : Entity
    {
        [Header("Shooting")]
        [SerializeField] private Bullet _bullet;
        [SerializeField] private float _shootingTime;

        private float _timeFromLastShot;
        private Transform _transform;


        protected override void Awake()
        {
            base.Awake();
            
            _timeFromLastShot = _shootingTime;
            _transform = transform;
        }

        private void Update()
        {
            base.Update();

            
            _timeFromLastShot += Time.deltaTime;
            if (_moveController.InTransition || !General.LevelManager.Instance.InputAllowed)
                return;
            
            var horizontal = InputHandler.Instance.Horizontal;
            var vertical = InputHandler.Instance.Vertical;
            var space = InputHandler.Instance.Space;
            var transitionAllowed = false;

            if (horizontal != 0.0f)
            {
                transitionAllowed = ProcessNewInput(0, (int)horizontal);

                _oldRotationZ = _targetRotationZ;
                _targetRotationZ = (horizontal < 0.0f) ? Utilities.Constants.RotationAngles.ANGLE_LEFT :
                    Utilities.Constants.RotationAngles.ANGLE_RIGHT;
            }
            else if (vertical != 0.0f)
            {
                transitionAllowed = ProcessNewInput((int)vertical, 0);

                _oldRotationZ = _targetRotationZ;
                _targetRotationZ = (vertical < 0.0f) ? Utilities.Constants.RotationAngles.ANGLE_BOTTOM :
                    Utilities.Constants.RotationAngles.ANGLE_TOP;
            }
            else if (space && (_timeFromLastShot >= _shootingTime))
            {
                _timeFromLastShot = 0.0f;

                var newBullet = Instantiate<Bullet>(_bullet, _transform.position, quaternion.identity);
                newBullet.MovementDirection = 
                    (Mathf.Approximately(_targetRotationZ, Utilities.Constants.RotationAngles.ANGLE_TOP))? new Point(1 , 0) : 
                        (Mathf.Approximately(_targetRotationZ, Utilities.Constants.RotationAngles.ANGLE_BOTTOM))? new Point(-1, 0) : 
                            (Mathf.Approximately(_targetRotationZ, Utilities.Constants.RotationAngles.ANGLE_LEFT))? new Point(0, -1) : 
                    new Point(0, 1);
            }

            if (transitionAllowed && !Mathf.Approximately(_oldRotationZ, _targetRotationZ))
                _rotationCoroutine = StartCoroutine(RotationRoutine());
        }
    }
}