using System;
using UnityEngine;

namespace App
{
    public class EndGate : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimHelper _animHelper;

        private bool _isNotLoop;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent<Character>(out var c) || _isNotLoop) return;
            _isNotLoop = true;
            _animHelper.PlayAnimByName("End", false);
            c.Completed();
        }
    }
}