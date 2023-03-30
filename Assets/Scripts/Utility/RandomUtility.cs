using System;
using UnityEngine;

namespace Utility
{
    public static class RandomUtility
    {
        public static Vector3 RandomPositionOnCircle(float radius, Vector3 center)
        {
            float theta = UnityEngine.Random.Range(0f, 1f) * 2 * Mathf.PI;
            float x = center.x + radius * Mathf.Cos(theta);
            float z = center.z + radius * Mathf.Sin(theta);
            Vector3 position = new Vector3(x, center.y, z);
            return position;
        }
    }
}
