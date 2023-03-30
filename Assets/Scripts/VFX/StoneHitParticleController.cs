using General;
using Interactions.Interactable;
using UnityEngine;

namespace VFX
{
    public class StoneHitParticleController : ClickableHitParticleController
    {
        protected override void OnAfterInteractableClicked(GameManager.ClickData clickData)
        {
            if(clickData.interactable is not Mine) return;
            base.OnAfterInteractableClicked(clickData);
        }
    }
}
