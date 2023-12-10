using System.Collections.Generic;
using UnityEngine;

namespace App
{
    public class UpgradeHelper : MonoBehaviour
    {
        public static int GetBulletAtLevel(int level, List<int> arr)
        {
            var sum = 0;
            for (var i = 0; i <= Mathf.Clamp(level, 0, arr.Count - 1); i++)
            {
                sum += arr[i];
            }

            return sum;
        }

        public static float GetDameAtLevel(int level, List<float> arr)
        {
            float sum = 0;
            for (var i = 0; i <= Mathf.Clamp(level, 0, arr.Count - 1); i++)
            {
                sum += arr[i];
            }

            return sum;
        }
    }
}