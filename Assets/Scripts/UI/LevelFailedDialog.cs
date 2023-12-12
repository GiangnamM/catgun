using System;
using DG.Tweening;
using Extension;
using UnityEngine;

namespace App
{
    public enum LevelFailedDialogResult
    {
        RetryLevel,
        EndLevel,
    }

    public class LevelFailedDialog : MonoBehaviour
    {
        [SerializeField] private RectTransform _dialogLayer;

        private Canvas _canvas;
        public Action<LevelFailedDialogResult> OnDidHide { get; set; }

        public static LevelFailedDialog Show(Canvas canvas)
        {
            var prefab = Resources.Load<LevelFailedDialog>($"Prefabs/UI/{nameof(LevelFailedDialog)}");
            var dialog = Instantiate(prefab, canvas.transform, false);
            dialog._canvas = canvas;
            return dialog;
        }

        private void Awake()
        {
            ServiceLocator.Instance.ResolveInjection(this);
        }

        public void OnCloseButtonPressed()
        {
            Hide(LevelFailedDialogResult.EndLevel);
        }


        public void OnReplayButtonPressed()
        {
            Hide(LevelFailedDialogResult.RetryLevel);
        }

        private void Hide(LevelFailedDialogResult result)
        {
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