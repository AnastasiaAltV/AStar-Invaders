using System.Collections;
using UnityEngine;


namespace AStar.Utilities
{
    public class PointsCarrier : MonoBehaviour
    {
        [SerializeField] private Vector3 _from;
        [SerializeField] private Vector3 _to;
        [Header("Additionals")]
        [SerializeField] private float _transitionTime;
        [SerializeField] private bool _startFromTransform;

        private Coroutine _transitionCoroutine;
        private Transform _transform;

        public bool InTransition
        {
            get { return _transitionCoroutine != null; }
        }
        public Vector3 From
        {
            get { return _from; }
            set { _from = value; }
        }
        public Vector3 To
        {
            get { return _to; }
            set { _to = value; }
        }


        private void Awake()
        {
            _transform = transform;
        }
        

        private IEnumerator TransitionRoutine()
        {
            float transitionPeriod = 0.0f;
            if (_startFromTransform)
                _from = _transform.position;

            while (transitionPeriod < _transitionTime)
            {
                transitionPeriod += Time.deltaTime;

                _transform.position = Vector3.Lerp(_from, _to, (transitionPeriod / _transitionTime));
                yield return null;
            }

            _transitionCoroutine = null;
        }


        public void StartTransition()
        {
            if (_transitionCoroutine == null)
                _transitionCoroutine = StartCoroutine(TransitionRoutine());
        }
    }
}