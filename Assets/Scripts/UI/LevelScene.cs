using System;
using Cysharp.Threading.Tasks;
using Extension;
using UnityEngine;

namespace App
{
    public class LevelScene : MonoBehaviour
    {
        [SerializeField] private DefaultConfigManager _configManager;
        [SerializeField] private Canvas _canvas;
        private Character _character;

        [Inject] private ISceneManager _sceneManager;

        private bool _initialized;


        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_initialized) return;
            ServiceLocator.Instance.ResolveInjection(this);
            _initialized = true;
            _character = FindObjectOfType<Character>();
            _character.OnStateCallBack = (result) =>
            {
                switch (result)
                {
                    case State.Fail:
                        ShowLevelFailDialog();
                        break;
                    case State.Complete:
                        ReloadLevel();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(result), result, null);
                }
            };
        }

        public void OnPauseButtonPressed()
        {
            var dialog = PauseDialog.Show(_canvas);
            dialog.OnDidHide = (result) =>
            {
                switch (result)
                {
                    case PauseDialogResult.Continue:
                        break;
                    case PauseDialogResult.Restart:
                        ReloadLevel();
                        break;
                    case PauseDialogResult.GoToMenu:
                        BackToMenu();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(result), result, null);
                }
            };
        }

        private void ShowLevelFailDialog()
        {
            var dialog = LevelFailedDialog.Show(_canvas);
            dialog.OnDidHide = (result) =>
            {
                switch (result)
                {
                    case LevelFailedDialogResult.EndLevel:
                        BackToMenu();
                        break;
                    case LevelFailedDialogResult.RetryLevel:
                        ReloadLevel();
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException(nameof(result), result, null);
                }
            };
        }

        private void ReloadLevel()
        {
            UniTask.Create(async () =>
            {
                await TransitionLayer.Fade(0.3f,
                    async () => await _sceneManager.LoadScene<LevelScene>(nameof(LevelScene)));
            });
        }

        private void BackToMenu()
        {
            UniTask.Create(async () =>
            {
                await TransitionLayer.Fade(0.3f,
                    async () => await _sceneManager.LoadScene<MenuScene>(nameof(MenuScene)));
            });
        }
    }
}