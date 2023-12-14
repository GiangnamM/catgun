using System;
using DG.Tweening;
using Extension;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App
{
    public class Enemy : MonoBehaviour
    {
        private readonly float MoveSpeed = 1.5f;
        [SerializeField] private SkeletonAnimHelper _animHelper;
        [SerializeField] private HealthBar _healthBar;
        private int _health;
        private Tween _tween;
        private Rigidbody2D _body;
        private Transform _trans;
        private float _distance;
        private bool _isMoveDone;
        private float _interval;

        public bool IsMoveDone
        {
            get => _isMoveDone;
            set
            {
                _isMoveDone = value;
                _distance = Random.Range(-2, 2);
                var scale = _animHelper.transform.localScale;
                scale.x = _distance > 0 ? 1 : -1;
                _animHelper.transform.localScale = scale;
                _interval = Math.Abs(_distance) / MoveSpeed;
            }
        }

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
            var defaultHealth = 200;
            _healthBar.MaxHealth = defaultHealth;
            Health = defaultHealth;
            _trans = transform;
            IsMoveDone = false;
        }

        private void Update()
        {
            if (gameObject.transform.position.x < -5f)
            {
                PoolManager.ReturnObject(gameObject);
            }

            _interval -= Time.deltaTime;
            if (_interval > 0)
            {
                _trans.position += MoveSpeed * Time.deltaTime * (_distance > 0 ? Vector3.left : Vector3.right);
                return;
            }

            IsMoveDone = _isMoveDone;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent<Character>(out var c))
            {
                c.TakeDamage(1);
            }
        }

        public void TakeDamage(float damage, Vector2 force)
        {
            Health -= (int)damage;
            Hit();
            if (Health <= 0)
            {
                _body.velocity = force;
                gameObject.layer = LayerMask.NameToLayer("FlyBody");
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