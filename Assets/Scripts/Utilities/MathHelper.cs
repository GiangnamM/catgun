using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App
{
    public class MathHelper : MonoBehaviour
    {
        public static float ValueAtIndex(List<float> arr, int index)
        {
            float sum = 0;
            for (int i = 0; i <= index; i++)
            {
                sum += arr[i];
            }

            return sum;
        }
    }
}