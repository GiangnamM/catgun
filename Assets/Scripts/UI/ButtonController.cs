using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App
{
    public enum ButtonControllerType
    {
        Right,
        Left,
        Jump,
        Fire,
        Shield,
    }

    public class ButtonController : MonoBehaviour
    {
        [Serializable]
        private class TypeAndSprite
        {
            public ButtonControllerType Type;
            public Sprite Sprite;
        }

        // [Serializable]
        // private class GunTypeAndSprite
        // {
        //     public BoosterType Type;
        //     public Sprite Sprite;
        // }

        [HideInInspector] [SerializeField] private ButtonControllerType _type;

        [SerializeField] private Image _bgImage;

        [SerializeField] private List<TypeAndSprite> _configs;

        [SerializeField] private Transform _counterLayer;

        [SerializeField] private TextMeshProUGUI _counterText;

        // [SerializeField] private List<GunTypeAndSprite> _gunTypeAndSpriteConfig;

        [SerializeField] private Image _iconImage;

        [SerializeField] private Transform _unlimitedBulletLayer;

        private bool _hasCounter;
        private int _counter;
        private bool _initialized;
        // private BoosterType _gunType;

        public ButtonControllerType Type
        {
            get => _type;
            set
            {
                if (_type == value)
                {
                    return;
                }

                _type = value;
                UpdateType();
            }
        }

        // public BoosterType GunType
        // {
        //     get => _gunType;
        //     set
        //     {
        //         _gunType = value;
        //         UpdateGunType();
        //     }
        // }

        public int Counter
        {
            get => _counter;
            set
            {
                _counter = value;
                UpdateCounter();
            }
        }

        public bool HasCounter
        {
            get => _hasCounter;
            set
            {
                _hasCounter = value;
                UpdateHasCounter();
            }
        }

        private void Awake()
        {
            Initialize();
            UpdateType();
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
        }

        private void UpdateType()
        {
            var selectedConfig = _configs.Find(item => item.Type == Type);
            if (selectedConfig == null)
            {
                return;
            }

            _bgImage.sprite = selectedConfig.Sprite;
        }

        private void UpdateHasCounter()
        {
            _counterLayer.gameObject.SetActive(HasCounter);
        }

        private void UpdateCounter()
        {
            HasCounter = Counter > 0;
            _counterText.text = Counter == int.MaxValue ? "" : $"{Counter}";
            _unlimitedBulletLayer.gameObject.SetActive(Counter == int.MaxValue);
        }

        // private void UpdateGunType()
        // {
        //     var selectedConfig = _gunTypeAndSpriteConfig.Find(item => item.Type == GunType);
        //     if (selectedConfig != null)
        //     {
        //         _iconImage.gameObject.SetActive(true);
        //         _iconImage.sprite = selectedConfig.Sprite;
        //     }
        // }

        public void OnPointerEnter()
        {
            if (Type == ButtonControllerType.Left || Type == ButtonControllerType.Right)
            {
                _bgImage.transform.localScale = new Vector3(1.08f, 1.08f, 1f);
            }
        }

        public void OnPointerExit()
        {
            if (Type == ButtonControllerType.Left || Type == ButtonControllerType.Right)
            {
                _bgImage.transform.localScale = Vector3.one;
            }
        }

        public void OnPointerDown()
        {
            // No-op.
        }

        public void OnPointerUp()
        {
            // No-op.
        }
    }
}