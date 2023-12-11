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

        [SerializeField] private DefaultEntityCreator _entityCreator;

        private bool _initialized;
        private ILevelManager _levelManager;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            var rendererLayer = transform;
            var manager = new DefaultLevelManager
            {
                ConfigManager = _configManager,
                PoolManager = new DefaultPoolManager(_entityCreator),
            };
            manager.EntityManager = new DefaultEntityManager(rendererLayer, manager);
            _levelManager = manager;
        }
    }
}