using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;

namespace Gameplay
{
    public class EnemiesSpawner : SingletonUnity<EnemiesSpawner>
    {
        public static event Action<Vector3> SpawningStarted;
        public static event Action SpawningCompleted;

        [SerializeField]
        [Tooltip("Distance from center map at which enemies starts to spawn")]
        float _spawnOffset = 40;
        [SerializeField]
        [Tooltip("Distance from center map at which enemies starts to spawn")]
        float _timeBetweenUnitSpawns = 1;

        [SerializeField]
        EnemyBehaviour _enemyPrefab;
        
        private Transform _landLevel;
        private IWave _waveGenerator = new FibbonaciSequence();

        protected override void Awake()
        {
            base.Awake();
            _landLevel = GameObject.FindWithTag("LandLevel").transform;    
        }

        public void SpawnWave()
        {
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            Vector3 spawnPosition = RandomUtility.RandomPositionOnCircle(_spawnOffset, _landLevel.position);

            SpawningStarted?.Invoke(spawnPosition);

            float unitsToSpawn = _waveGenerator.Next();

            for (int i = 0; i < unitsToSpawn; i++)
            {
                Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(_timeBetweenUnitSpawns);
            }

            SpawningCompleted?.Invoke();
        }
    }

}
