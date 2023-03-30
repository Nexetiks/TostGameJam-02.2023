using System;
using DG.Tweening;
using Gameplay;
using General;
using UnityEngine;

namespace VFX
{
    public class DayNightVFXController : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer fogRenderer;
        
        [SerializeField]
        private Color dayFogColor;

        [SerializeField]
        private Color nightFogColor;
        
        [SerializeField]
        private float fogChangeDuration = 1f;

        [SerializeField]
        private Ease fogChangeEase = Ease.InOutCubic;

        private Material instancedFogMaterial;
        private Sequence fogSequence;

        private void Awake()
        {
            instancedFogMaterial = Instantiate(fogRenderer.material);
            fogRenderer.material = instancedFogMaterial;
        }

        private void OnEnable()
        {
            DayPhases.OnDay += OnDay;
            DayPhases.OnNight += OnNight;
        }

        private void OnDisable()
        {
            DayPhases.OnDay -= OnDay;
            DayPhases.OnNight -= OnNight;
        }

        private void OnDay()
        {
            PlayFogChangeSequence(dayFogColor);
        }
        private void OnNight()
        {
            PlayFogChangeSequence(nightFogColor);
        }

        private void PrepareFogSequence()
        {
            if (fogSequence != null)
            {
                fogSequence.Kill();
            }
            
            fogSequence = DOTween.Sequence();
        }

        private void PlayFogChangeSequence(Color color)
        {
            PrepareFogSequence();
            fogSequence.Append(instancedFogMaterial.DOColor(color, "_BaseColor", fogChangeDuration).SetEase(fogChangeEase));
        }
    }
}
