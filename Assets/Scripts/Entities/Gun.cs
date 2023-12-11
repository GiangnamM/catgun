using System;
using System.Collections.Generic;
using Extension;
using UnityEngine;

namespace App
{
    public class Gun : EntityComponent
    {
        [SerializeField] private List<Transform> _gunPosList;

        private bool _initialized;
        private float _fireRate;
        private bool _canFire;
        private GunSkin _type;
        private float _elapsed;

        public Vector3 GunPos => _gunPosList[0].position;

        public float FireRate
        {
            get => _fireRate;
            set => _fireRate = value;
        }

        public GunSkin Type
        {
            get => _type;
            set
            {
                _type = value;
                UpdateType();
            }
        }


        private void Awake()
        {
            Initialize();
            ServiceLocator.Instance.ResolveInjection(this, "entity");
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            if (_elapsed > 1 / _fireRate)
            {
                _elapsed = 0;
                _canFire = true;
            }
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            ServiceLocator.Instance.ResolveInjection(this);
        }

        public void Reset()
        {
            _canFire = true;
        }

        public bool Fire()
        {
            if (!_canFire)
            {
                return false;
            }

            _canFire = false;
            return true;
        }

        private void UpdateType()
        {
        }
    }
}