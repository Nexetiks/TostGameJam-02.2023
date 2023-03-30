using System.Collections.Generic;
using Gameplay;
using Interactions;

namespace WorkStations
{
    public interface IWorkStations : IInteractable
    {
        List<WorkerInteractable> WorkersOnStation { get; set; }
        void AddWorker(WorkerInteractable worker);
        void RemoveWorker(WorkerInteractable worker);

        void Use();
    }
}