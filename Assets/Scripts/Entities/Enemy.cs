using System;
using DG.Tweening;
using UnityEngine;

namespace App
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimHelper _animHelper;
        [SerializeField] private HealthBar _healthBar;
        private int _health;
        private Tween _tween;
        private Rigidbody2D _body;

        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                _healthBar.Health = value;
            }
        }

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            Health = 200;
        }

        private void Update()
        {
            if (gameObject.transform.position.x < -5f) {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent<Character>(out var c))
            {
                Debug.Log("Collider With character");
            }
            
        }

        public void TakeDamage(float damage, Vector2 force)
        {
            Health -= (int)damage;
            Hit();
            if (Health <= 0)
            {
                _body.velocity = force;
            }
        }

        public void Hit()
        {
            _tween?.Kill();
            _tween = DOTween.Sequence()
                .AppendCallback(() =>
                {
                    _animHelper.Animation.Skeleton.A = .3f;
                    _animHelper.Animation.Skeleton.R = .7f;
                    _animHelper.Animation.Skeleton.G = .3f;
                    _animHelper.Animation.Skeleton.B = .3f;
                })
                .AppendInterval(.14f)
                .AppendCallback(() =>
                {
                    _animHelper.Animation.Skeleton.A = 1f;
                    _animHelper.Animation.Skeleton.R = 1f;
                    _animHelper.Animation.Skeleton.G = 1f;
                    _animHelper.Animation.Skeleton.B = 1f;
                })
                .AppendInterval(.14f);
            // .SetLoops(5);
        }
    }
}