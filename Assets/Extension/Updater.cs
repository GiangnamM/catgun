using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using UnityEngine;

namespace App {
    [DisallowMultipleComponent]
    public class Updater : EntityComponent {
        private readonly List<Action> _beginCallbacks = new(1);
        private readonly List<Action> _endCallbacks = new(1);
        private readonly List<Action> _pauseCallbacks = new(1);
        private readonly List<Action> _resumeCallbacks = new(1);
        private readonly List<Action<float>> _updateCallbacks = new(1);

        private float _timeMultiplier = 1;

        public Updater OnBegin([NotNull] Action callback) {
            _beginCallbacks.Add(callback);
            return this;
        }

        public Updater OnEnd([NotNull] Action callback) {
            _endCallbacks.Add(callback);
            return this;
        }

        public Updater OnPause([NotNull] Action callback) {
            _pauseCallbacks.Add(callback);
            return this;
        }

        public Updater OnResume([NotNull] Action callback) {
            _resumeCallbacks.Add(callback);
            return this;
        }

        public Updater OnUpdate([NotNull] Action<float> callback) {
            _updateCallbacks.Add(callback);
            return this;
        }

        public void Begin() {
            for (var i = _beginCallbacks.Count - 1; i >= 0; --i) {
                _beginCallbacks[i]();
            }
        }

        public void End() {
            for (var i = _endCallbacks.Count - 1; i >= 0; --i) {
                _endCallbacks[i]();
            }
        }

        public void Pause() {
            for (var i = _pauseCallbacks.Count - 1; i >= 0; --i) {
                _pauseCallbacks[i]();
            }
        }

        public void Resume() {
            for (var i = _resumeCallbacks.Count - 1; i >= 0; --i) {
                _resumeCallbacks[i]();
            }
        }

        public void ProcessUpdate(float delta) {
            for (var i = _updateCallbacks.Count - 1; i >= 0; --i) {
                _updateCallbacks[i](delta * _timeMultiplier);
            }
        }
    }
}