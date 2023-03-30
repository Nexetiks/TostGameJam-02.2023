using System;
using UnityEngine;

namespace Gameplay
{
    public interface ITarget
    {
        public event Action<float, float> OnHit; // damage taken, remaining hitpoints
        public event Action OnDeath;

        public void Hit(float damage);
        public Vector3 GetPosition();
    }
}
