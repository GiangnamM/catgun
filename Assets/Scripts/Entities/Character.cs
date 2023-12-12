using System;
using Extension;
using UnityEngine;

namespace App
{
    public enum State
    {
        Fail,
        Complete,
    }

    public class Character : MonoBehaviour
    {
        private const int MaxJump = 1;
        private const float MoveSpeed = 5f;
        private const float JumpPower = 14f;
        private const float BulletSpeed = 15f;
        private const float InvincibleTime = 2f;

        [SerializeField] private CharacterRenderer _characterRenderer;

        [SerializeField] private DefaultConfigManager _configManager;

        [SerializeField] private GameObject _bullet;
        [SerializeField] private HealthBar _healthBar;

        [Inject] private ISkinGunManager _skinGunManager;
        [Inject] private IUpgradeGunManager _upgradeGunManager;
        private Rigidbody2D _body;
        private Collider2D _collider;
        private int _jumpCount;
        private bool _isGrounded;
        private Vector2 _direction;
        private Vector2 _lastDirection;
        private float _horizontal;
        private float _vertical;
        private bool _isDead;
        private Gun _gun;
        private GunSkin _gunSkin;
        private bool _initialized;
        private float _damage;
        private int _health;
        private bool _isInvincible;
        private float _remainInvincibleTime;

        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                _healthBar.Health = value;
            }
        }

        public bool IsGrounded
        {
            get => _isGrounded;
            private set
            {
                _isGrounded = value;
                if (_isGrounded)
                {
                    _jumpCount = 0;
                }
            }
        }

        public Vector2 Velocity
        {
            get => _body.velocity;
            set
            {
                if (_body.velocity != value)
                {
                    _body.velocity = value;
                }
            }
        }

        public Vector2 Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                if (_direction.x != 0)
                {
                    _lastDirection = _direction;
                }

                UpdateDirection();
                _characterRenderer.Direction = _direction;
            }
        }

        private GunSkin GunSkin
        {
            get => _gunSkin;
            set
            {
                _gunSkin = value;
                Initialize();
                UpdateGunConfig();
                _characterRenderer.Gun = _gunSkin;
            }
        }


        public bool IsStunning { get; private set; }

        public Action<State> OnStateCallBack { get; set; }

        private void Awake()
        {
            Initialize();
            var defaultHealth = 3;
            _healthBar.MaxHealth = defaultHealth;
            Health = defaultHealth;
            var gun = _skinGunManager.CurrentSkin;
            GunSkin = gun;
        }

        private void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            ServiceLocator.Instance.ResolveInjection(this);
            _gun = GetComponent<Gun>();
        }

        private void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _lastDirection = Vector2.right;
        }

        // Update is called once per frame
        private void Update()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
            if (Input.GetKey(KeyCode.Space))
            {
                Fire();
            }

            if (Mathf.Abs(_body.velocity.y) < 0.1f && !IsGrounded)
            {
                IsGrounded = IsOnGround();
            }

            var trans = transform;
            if (trans.position.y < -3f)
            {
                PoolManager.ReturnObject(gameObject);
                OnStateCallBack?.Invoke(State.Fail);
            }

            if (!_isInvincible)
            {
                return;
            }

            _remainInvincibleTime += Time.deltaTime;
            if (_remainInvincibleTime > InvincibleTime)
            {
                _isInvincible = false;
                _remainInvincibleTime = 0;
            }
        }

        private void FixedUpdate()
        {
            if (_isDead) return;
            Direction = new Vector2(_horizontal, _vertical);
        }

        private void UpdateDirection()
        {
            Move(_direction);
            if (_direction.y > 0f)
            {
                Jump();
            }
        }

        private void Move(Vector2 dir)
        {
            Velocity = new Vector2(dir.x * MoveSpeed, Velocity.y);
        }

        private bool IsOnGround()
        {
            var raycastHit = Physics2D.BoxCast(_collider.bounds.center - new Vector3(0f, 0.2f, 0f),
                _collider.bounds.size, 0, Vector2.down,
                0.1f, LayerMask.GetMask("Wall"));
            return raycastHit.collider != null;
        }

        public void Jump()
        {
            if (_jumpCount == MaxJump || _isDead)
            {
                return;
            }

            _jumpCount++;
            IsGrounded = false;
            Velocity = new Vector2(Velocity.x, JumpPower);
        }

        private void Fire()
        {
            var gunType = GunSkin;
            if (!_gun.Fire())
            {
                return;
            }

            _characterRenderer.Fire();
            SpawnNormalBullet(gunType);
        }

        private void UpdateGunConfig()
        {
            var baseFireRate = _configManager.GunBaseInfo[_gunSkin].Item2;
            var baseDame = _configManager.GunBaseInfo[_gunSkin].Item1;
            if (!_configManager.AllUpgradeGunTuples.TryGetValue(_gunSkin, out var arr))
            {
                Debug.LogWarning("Config doesn't have config upgrade gun name: " + _gunSkin);
                return;
            }

            var (arrDame, arrFireRate) = arr;
            var level = _upgradeGunManager.GetLevelGun(_gunSkin);
            var upgradeDame = MathHelper.ValueAtIndex(arrDame, level);
            var upgradeFireRate = MathHelper.ValueAtIndex(arrFireRate, level);
            _gun.FireRate = baseFireRate + upgradeFireRate;
            _damage = baseDame + upgradeDame;
        }

        private void SpawnNormalBullet(GunSkin gunType, float startX = 0f, float startY = 0f)
        {
            var obj = PoolManager.SpawnObject(_bullet, _gun.GunPos + new Vector3(startX, startY, -0.01f),
                Quaternion.identity);
            var trans = obj.transform;
            trans.localPosition = _gun.GunPos + new Vector3(startX, startY, -0.01f);
            if (!obj.TryGetComponent<Bullet>(out var b))
            {
                Debug.LogWarning("Object can have bullet component");
                return;
            }

            b.Direction = _lastDirection;
            b.Speed = BulletSpeed;
            b.Damage = _damage;
            b.Skin = gunType;
        }

        public void TakeDamage(float damage)
        {
            if (_isInvincible)
            {
                return;
            }

            Health -= (int)damage;
            _characterRenderer.Hit();
            _isInvincible = true;
            if (Health <= 0)
            {
                OnStateCallBack?.Invoke(State.Fail);
                PoolManager.ReturnObject(gameObject);
            }
        }

        public void Completed()
        {
            _isDead = true;
            OnStateCallBack?.Invoke(State.Complete);
            Move(Vector2.zero);
        }
    }
}