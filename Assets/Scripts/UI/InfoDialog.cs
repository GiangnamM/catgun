using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace App
{
    public enum InfoDialogResult
    {
        Ok,
        Cancel,
    }

    public class InfoDialog : MonoBehaviour
    {
        [SerializeField] private RectTransform _dialogLayer;

        [SerializeField] private TextMeshProUGUI _contentText;

        [SerializeField] private RectTransform _cancelLayer;

        [SerializeField] private TextMeshProUGUI _okText;

        [SerializeField] private TextMeshProUGUI _cancelText;

        private bool _initialized;
        private string _content;
        private bool _hasCancel;
        private string _okString;
        private string _cancelString;
        private Canvas _canvas;
        private bool _isActive;
        public Action<InfoDialogResult> OnDidHide { get; set; }

        public static InfoDialog Show(Canvas canvas)
        {
            var prefab = Resources.Load<InfoDialog>($"Prefabs/UI/{nameof(InfoDialog)}");
            var dialog = Instantiate(prefab, canvas.transform, false);
            dialog._canvas = canvas;
            return dialog;
        }

        public string Content
        {
            get => _content;
            set
            {
                Initialize();
                if (_content == value)
                {
                    return;
                }

                _content = value;
                UpdateContent();
            }
        }

        public string OkText
        {
            get => _okString;
            set
            {
                _okString = value;
                _okText.text = value;
            }
        }

        public string CancelText
        {
            get => _cancelString;
            set
            {
                _cancelString = value;
                _cancelText.text = value;
            }
        }

        public bool HasCancel
        {
            get => _hasCancel;
            set
            {
                _hasCancel = value;
                _cancelLayer.gameObject.SetActive(_hasCancel);
            }
        }

        private void Awake()
        {
            Initialize();
            UpdateContent();
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            Show();
            _initialized = true;
        }

        private void Show()
        {
            _dialogLayer.transform.localScale = Vector3.zero;
            DOTween.Sequence()
                .Append(_dialogLayer.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack));
        }

        private void Hide(InfoDialogResult result)
        {
            DOTween.Sequence()
                .Append(_dialogLayer.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack))
                .AppendCallback(() =>
                {
                    Destroy(gameObject);
                    OnDidHide?.Invoke(result);
                });
        }

        private void UpdateContent()
        {
            _contentText.text = _content;
        }

        public void OnOkButtonPressed()
        {
            Hide(InfoDialogResult.Ok);
        }

        public void OnCloseButtonPressed()
        {
            Hide(InfoDialogResult.Cancel);
        }
    }
}