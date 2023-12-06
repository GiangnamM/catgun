using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace App
{
    public class SkeletonAnimHelper : MonoBehaviour
    {
        private SkeletonAnimation _animation;
        private Spine.AnimationState _animationState;
        private Vector2 _direction;

        public SkeletonAnimation Animation => _animation;

        public Vector2 Direction
        {
            get => _direction;
            set
            {
                var lastDir = _direction;
                _direction = value;
                UpdateDirection(lastDir);
            }
        }

        private void Awake()
        {
            _animation = GetComponent<SkeletonAnimation>();
            _animationState = _animation.AnimationState;
        }

        public TrackEntry PlayAnimByName(string anim, bool loop = true, Action complete = null, int trackIndex = 0)
        {
            var track = _animationState.SetAnimation(trackIndex, anim, loop);
            if (complete != null)
            {
                track.Complete += entry => { complete?.Invoke(); };
            }

            return track;
        }

        public void UpdateDirection(Vector2 lastDir)
        {
            if (_direction != lastDir)
            {
                if (_direction.x != 0)
                {
                    _animation.Skeleton.ScaleX = _direction.x > 0 ? 1 : -1;
                }
            }
        }

        public void SetSkin(string skin)
        {
            _animation.Skeleton.SetSkin(skin);
        }
    }
}