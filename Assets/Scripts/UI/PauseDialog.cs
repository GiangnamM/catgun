using System;
using DG.Tweening;
using UnityEngine;

namespace App
{
    public enum PauseDialogResult
    {
        Continue,
        Restart,
        GoToMenu,
    }

    public class PauseDialog : MonoBehaviour
    {
        [SerializeField] private RectTransform _dialogLayer;

        private bool _initialized;
        private Canvas _canvas;

        private bool _isPaused;

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                Time.timeScale = _isPaused ? 0 : 1;
            }
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            IsPaused = true;
            _initialized = true;
        }

        public Action<PauseDialogResult> OnDidHide { get; set; }

        public static PauseDialog Show(Canvas canvas)
        {
            var prefab = Resources.Load<PauseDialog>($"Prefabs/UI/{nameof(PauseDialog)}");
            var dialog = Instantiate(prefab, canvas.transform, false);
            dialog._canvas = canvas;
            return dialog;
        }

        public void OnCloseButtonPressed()
        {
            Hide(PauseDialogResult.Continue);
        }

        public void OnContinueButtonPressed()
        {
            Hide(PauseDialogResult.Continue);
        }

        public void OnReplayButtonPressed()
        {
            Hide(PauseDialogResult.Restart);
        }


        public void OnQuitButtonPressed()
        {
            Hide(PauseDialogResult.GoToMenu);
        }

        private void Hide(PauseDialogResult result)
        {
            IsPaused = false;
            DOTween.Sequence()
                .Append(_dialogLayer.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack))
                .AppendCallback(() =>
                {
                    Destroy(gameObject);
                    OnDidHide?.Invoke(result);
                });
        }
    }
}