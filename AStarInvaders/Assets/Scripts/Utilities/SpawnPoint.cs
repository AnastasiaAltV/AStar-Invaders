using System.Collections.Generic;
using System.Collections;
using UnityEngine;


namespace AStar.Utilities
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToSpawn;
        [SerializeField] private float _spawnPeriod;
        [SerializeField] private List<Transform> _spawnPoints;

        private Coroutine _spawnCoroutine;


        private void Start()
        {
            if (_objectToSpawn != null)
                _spawnCoroutine = StartCoroutine(SpawnObjectRoutine());
        }


        private IEnumerator SpawnObjectRoutine()
        {
            var spawnPeriod = new WaitForSeconds(_spawnPeriod);
            
            do
            {
                yield return spawnPeriod;
                Instantiate(_objectToSpawn, _spawnPoints[Random.Range(0, _spawnPoints.Count)]);
            }
            while (General.LevelManager.Instance.PlayerAlive);

            _spawnCoroutine = null;
        }
    }
}