using System.Collections.Generic;

using Spine.Unity;

using UnityEngine;

namespace App {
    public class SkinGunSelector : MonoBehaviour {
        [SerializeField]
        private SkeletonGraphic _animation;

        private bool _initialized;
        private GunSkin _gunType;

        private Dictionary<GunSkin, string> _configAnimation = new Dictionary<GunSkin, string> {
            // { GunType.Ak47, "GUN 1" },
            // { GunType.Bazooka, "GUN 2" },
            // { GunType.M4A1, "GUN 3" },
            // { GunType.FireBlaster, "GUN 4" },
            // { GunType.Laser, "GUN 5" },

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