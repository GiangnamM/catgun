using System;
using System.Collections;
using System.Collections.Generic;
using App;
using DG.Tweening;
using UnityEngine;

namespace App
{
    public class CharacterRenderer : MonoBehaviour
    {
        private const int MoveTrack = 2;
        private const int FireTrack = 1;

        private enum AnimState
        {
            IDLE,
            MOVE,
            DIE,
            JUMP,
            FIRE,
        }

        [SerializeField] private SkeletonAnimHelper _animHelper;

        [SerializeField] private Character _character;

        private bool _initialized;
        private Vector2 _direction;
        private AnimState _animState;
        private IDictionary<AnimState, string> _skinAndName;
        private string _skinName;
        private GunSkin _gun;
        private Tween _tween;

        public GunSkin Gun
        {
            get => _gun;
            set
            {
                _gun = value;
                UpdateGunType();
            }
        }

        public Vector2 Direction
        {
            get => _direction;
            set
            {
                var lastDir = _direction;
                _direction = value;
                UpdateDirection(lastDir);
                _animHelper.Direction = _direction;
            }
        }

        private AnimState State
        {
            get => _animState;
            set
            {
                var lastState = _animState;
                _animState = value;
                if (_animState != lastState)
                {
                    AnimStateChanged();
                }
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

            _initialized = false;
            _direction = Vector2.zero;
            _skinAndName = new Dictionary<AnimState, string>()
            {
                { AnimState.IDLE, "" },
                { AnimState.MOVE, "" },
                { AnimState.JUMP, "" },
                { AnimState.FIRE, "" },
                { AnimState.DIE, "" },
            };
        }

        private void OnDestroy()
        {
            _tween?.Kill();
        }

        private void AnimStateChanged()
        {
            switch (_animState)
            {
                case AnimState.IDLE:
                    Idle();
                    break;
                case AnimState.MOVE:
                    Move();
                    break;
                case AnimState.DIE:
                    Die();
                    break;
                case AnimState.JUMP:
                    Jump();
                    break;
                case AnimState.FIRE:
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void UpdateGunType()
        {
            var gun = _gun switch
            {
                GunSkin.M4A1 => null,
                GunSkin.Bazooka => "Gun 04",
                GunSkin.FireBlaster => "GUN 03",
                GunSkin.Laser => "GUN 05",
                _ => throw new ArgumentOutOfRangeException()
            };
            _skinAndName[AnimState.IDLE] = $"{gun}/IDLE";
            _skinAndName[AnimState.MOVE] = $"{gun}/RUN";
            _skinAndName[AnimState.JUMP] = $"{gun}/JUMP";
            _skinAndName[AnimState.FIRE] = $"{gun}/ATTACK GUN";
            _skinAndName[AnimState.DIE] = $"{gun}/DIE GUN";
            State = AnimState.IDLE;
        }

        private void Jump()
        {
            _animHelper.PlayAnimByName(_skinAndName[AnimState.JUMP], false);
            _animHelper.Animation.AnimationState.ClearTrack(MoveTrack);
        }

        private void Move()
        {
            _animHelper.PlayAnimByName(_skinAndName[AnimState.MOVE], true, null, MoveTrack);
        }

        private void Idle()
        {
            _animHelper.PlayAnimByName(_skinAndName[AnimState.IDLE], true);
            _animHelper.Animation.AnimationState.ClearTrack(MoveTrack);
        }

        private void Die()
        {
            _animHelper.PlayAnimByName(_skinAndName[AnimState.DIE], false);
        }

        public void Fire()
        {
            _animHelper.PlayAnimByName(_skinAndName[AnimState.FIRE], false,
                () => { _animHelper.Animation.AnimationState.ClearTrack(FireTrack); }, FireTrack);
        }

        private void UpdateDirection(Vector2 lastDir)
        {
            if (!_character.IsGrounded)
            {
                State = AnimState.JUMP;
                return;
            }

            State = _direction.x != 0 ? AnimState.MOVE : AnimState.IDLE;
        }

        public void Hit()
        {
            _tween?.Kill();
            _tween = DOTween.Sequence()
                .AppendCallback(() =>
                {
                    _animHelper.Animation.Skeleton.A = .3f;
                    _animHelper.Animation.Skeleton.R = .7f;
                    _animHelper.Animation.Skeleton.G = .3f;
                    _animHelper.Animation.Skeleton.B = .3f;
                })
                .AppendInterval(.14f)
                .AppendCallback(() =>
                {
                    _animHelper.Animation.Skeleton.A = 1f;
                    _animHelper.Animation.Skeleton.R = 1f;
                    _animHelper.Animation.Skeleton.G = 1f;
                    _animHelper.Animation.Skeleton.B = 1f;
                })
                .AppendInterval(.14f)
                .SetLoops(5);
        }
    }
}