using System.Collections;
using System.Drawing;
using UnityEngine;


namespace AStar.Behaviours
{
    public class Bullet : Entity
    {
        [SerializeField] private float _movementPeriod;
        
        private Coroutine _movementCoroutine;

        public Point MovementDirection { get; set; }


        private void Start()
        {
            if(!_initialized)
                StartInitialization();
            _movementCoroutine = StartCoroutine(FollowPlayerRoutine());
        }

        private void OnDestroy()
        {
            if(_movementCoroutine != null)
                StopCoroutine(_movementCoroutine);
        }
        
        
        private IEnumerator FollowPlayerRoutine()
        {
            var movementPeriod = new WaitForSeconds(_movementPeriod);

            while (true)
            {
                var bulletMovmentAllowed = ProcessNewInput(MovementDirection.X, MovementDirection.Y);
                if (!bulletMovmentAllowed)
                {
                    Destroy(gameObject);
                    yield break;
                }

                yield return movementPeriod;
            }
        }
    }
}