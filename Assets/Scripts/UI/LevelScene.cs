using System;
using System.Collections;
using System.Collections.Generic;
using Extension;
using UnityEngine;

namespace App
{
    public class LevelScene : MonoBehaviour
    {
        [SerializeField] private DefaultConfigManager _configManager;
        
        private bool _initialized;


        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            var rendererLayer = transform;
            
        }
    }
}