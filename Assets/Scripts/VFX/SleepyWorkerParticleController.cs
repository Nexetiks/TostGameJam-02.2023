using System;
using Gameplay;
using UnityEngine;

namespace VFX
{
    [RequireComponent(typeof(ParticleSystem))]
    public class SleepyWorkerParticleController : MonoBehaviour
    {
        [SerializeField]
        private WorkerInteractable workerInteractable;

        private ParticleSystem ps;

        private void Awake()
        {
            ps = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            workerInteractable.WorkingLevelHandler.OnWorkingLevelChanged += OnWorkingLevelChanged;
        }

        private void OnDisable()
        {
            workerInteractable.WorkingLevelHandler.OnWorkingLevelChanged -= OnWorkingLevelChanged;
        }

        private void OnWorkingLevelChanged(WorkerInteractable.WorkingLevel workingLevel)
        {
            if (workingLevel.WorkingLeveLType == WorkerInteractable.WorkingLevelType.Asleep)
            {
                ps.Play();
            }
            else
            {
                ps.Stop();
            }
        }
    }
}
