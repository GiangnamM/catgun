using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace App
{
    public class HealthBar : MonoBehaviour
    {
        private static readonly float DUR_EFFECT_TAKE_DAMAGE = 1.3f;

        [SerializeField] private SpriteRenderer _healthRenderer;

        [SerializeField] private TextMeshPro _damageTxt;

        private bool _initialized;
        private float _maxHealth;
        private float _health;
        private float _healthWidth;
        private Tween _tween;

        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                if (Mathf.Approximately(_maxHealth, value))
                {
                    return;
                }

                _maxHealth = value;
                Initialize();
                UpdateDisplay();
                gameObject.SetActive(false);
            }
        }

        public float Health
        {
            get => _health;
            set
            {
                if (Mathf.Approximately(_health, value))
                {
                    return;
                }

                var damage = _health - value;
                if (damage > 0)
                {
                    TakeDamage(damage);
                }

                _health = value;
                Initialize();
                UpdateDisplay();
            }
        }

        private void Awake()
        {
            Initialize();
            UpdateDisplay();
            Hide();
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _healthWidth = _healthRenderer.size.x;
            _initialized = true;
        }

        private void UpdateDisplay()
        {
            var maxHealth = Mathf.Max(1, MaxHealth);
            var percentage = Health / maxHealth;
            var width = _healthWidth * percentage;
            var trans = _healthRenderer.transform;
            var position = trans.localPosition;
            position.x = -(_healthWidth - width) * 0.5f;
            trans.localPosition = position;
            var size = _healthRenderer.size;
            size.x = width;
            _healthRenderer.size = size;
        }

        private void TakeDamage(float damage)
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }

            var transform1 = transform;
            var localScale = transform1.localScale;
            localScale.x = 1;
            transform1.localScale = localScale;
            AnimDamage(damage);
        }

        private void AnimDamage(float damage)
        {
            var desPos = new Vector3(-0.25f, -0.5f, 0);
            _tween?.Kill();
            _damageTxt.gameObject.SetActive(true);
            _damageTxt.text = $"{Math.Floor(damage)}";
            _damageTxt.transform.localPosition = Vector3.zero;
            _tween = _damageTxt.transform.DOLocalJump(desPos, 0.7f, 1, DUR_EFFECT_TAKE_DAMAGE)
                .Join(DOTween.Sequence()
                    .Append(_damageTxt.DOFade(0, 0f))
                    .Append(_damageTxt.DOFade(1, DUR_EFFECT_TAKE_DAMAGE * 0.5f))
                    .Append(_damageTxt.DOFade(0, DUR_EFFECT_TAKE_DAMAGE * 0.5f))
                    .AppendCallback(() => { _damageTxt.gameObject.SetActive(false); }));
        }

        public void Hide()
        {
            var trans = transform;
            var localScale = trans.localScale;
            localScale.x = 0;
            trans.localScale = localScale;
        }
    }
}