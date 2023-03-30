using General;
using Interactions.Interactable;
using UnityEngine;

namespace VFX
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ClickableHitParticleController : MonoBehaviour
    {
        [SerializeField]
        protected float offset = .5f;
        protected ParticleSystem ps;

        private void Awake()
        {
            ps = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            GameManager.Instance.OnAfterInteractableClicked += OnAfterInteractableClicked;
        }
        private void OnDisable()
        {
            GameManager.Instance.OnAfterInteractableClicked -= OnAfterInteractableClicked;
        }
        
        protected virtual void OnAfterInteractableClicked(GameManager.ClickData clickData)
        {
            ps.transform.up = clickData.eventData.pointerPressRaycast.worldNormal;
            ps.transform.position = clickData.eventData.pointerPressRaycast.worldPosition + ps.transform.up * offset;
            ps.Play();
        }
    }
}
