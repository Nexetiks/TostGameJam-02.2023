using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using VFX;

public class WorkerSpitProjectilesManager : MonoBehaviour
{
    [SerializeField]
    private WorkerSpitProjectileController workerSpitProjectilesPrefab;

    private void OnEnable()
    {
        WorkerInteractable.OnSpitStarted += OnSpitStarted;
    }
    
    private void OnDisable()
    {
        WorkerInteractable.OnSpitStarted -= OnSpitStarted;
    }
    

    private void OnSpitStarted(WorkerInteractable.SpitProjectileActionData spitData)
    {
        WorkerSpitProjectileController workerSpitProjectileInstance = Instantiate(workerSpitProjectilesPrefab);
        workerSpitProjectileInstance.Fire(spitData.startPosition, spitData.destination, spitData.flightDuration);
    }
}
