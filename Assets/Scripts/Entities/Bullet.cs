using System;
using System.Collections.Generic;
using UnityEngine;

namespace App
{
    public class Bullet : Entity
    {
        private const float MaxDistance = 16f;

        [Serializable]
        private class SkinAndRenderLayer
        {
            public GunSkin Skin;
            public Transform RenderLayer;
        }

        [SerializeField] private List<SkinAndRenderLayer> _skinAndRenderLayers;

        [SerializeField] private Transform _renderer;

        private bool _initialized;
        private Vector2 _direction;
        private float _angle;
        private GunSkin _skin;
        private float _interval;
        public float Speed { get; set; }
        public float Damage { get; set; }

        public Vector2 Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                UpdateDirection();
            }
        }

        public GunSkin Skin
        {
            get => _skin;
            set
            {
                _skin = value;
                UpdateSkin();
            }
        }

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            var trans = transform;
            _interval -= Time.deltaTime;
            if (_interval < 0)
            {
                Destroy(gameObject);
                return;
            }

            var distance = Direction.x * Speed * Time.deltaTime;
            trans.localPosition += new Vector3(distance, 0);
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
        }

        private void UpdateDirection()
        {
            var scale = _renderer.localScale;
            scale.x = _direction.x > 0 ? 1 : -1;
            _renderer.localScale = scale;
        }

        private void UpdateSkin()
        {
            foreach (var skinAndRenderLayer in _skinAndRenderLayers)
            {
                skinAndRenderLayer.RenderLayer.gameObject.SetActive(skinAndRenderLayer.Skin == Skin);
            }

            _interval = MaxDistance / Speed;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<Enemy>(out var e))
            {
                e.TakeDamage(20,
                    transform.position.x - e.transform.position.x > 0 ? new Vector2(-1, 9) : new Vector2(1, 9));
            }
        }
    }
}