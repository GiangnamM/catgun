using System.Collections.Generic;

using Spine.Unity;

using UnityEngine;

namespace App {
    public class SkinGunSelector : MonoBehaviour {
        [SerializeField]
        private SkeletonGraphic _animation;

        private bool _initialized;
        private GunSkin _gunType;

        private Dictionary<GunSkin, string> _configAnimation = new()
        {
            { GunSkin.Bazooka, "GUN 2" },
            { GunSkin.M4A1, "GUN 3" },
            { GunSkin.FireBlaster, "GUN 4" },
            { GunSkin.Laser, "GUN 5" },

        };

        public GunSkin GunType {
            get => _gunType;
            set {
                _gunType = value;
                UpdateSkin();
            }
        }

        private void Awake() {
            Initialize();
        }

        private void Initialize() {
            if (_initialized) {
                return;
            }
            _initialized = true;
            UpdateSkin();
        }

        private void UpdateSkin() {
            var anim = _configAnimation[_gunType];
            if (anim == null) {
                return;
            }
            _animation.AnimationState.SetAnimation(0, anim, true);
            _animation.Skeleton.SetSlotsToSetupPose();
        }
    }
}