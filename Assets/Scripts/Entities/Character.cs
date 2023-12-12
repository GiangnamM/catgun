using Extension;
using UnityEngine;

namespace App
{
    public class Character : MonoBehaviour
    {
        private const int MaxJump = 1;
        private const float MoveSpeed = 6.1f;
        private const float JumpPower = 14f;
        private const float BulletSpeed = 15f;

        [SerializeField] private CharacterRenderer _characterRenderer;

        [SerializeField] private DefaultConfigManager _configManager;

        [SerializeField] private GameObject _bullet;

        [Inject] private ISkinGunManager _skinGunManager;
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

        public GunSkin GunSkin
        {
            get => _gunSkin;
            set
            {
                _gunSkin = value;
                Initialize();
                UpdateGunConfig();
            }
        }


        public bool IsStunning { get; private set; }

        private void Awake()
        {
            Initialize();
            GunSkin = GunSkin.Bazooka;
        }

        private void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            ServiceLocator.Instance.ResolveInjection(this);
            UpdateSkin();
            _gun = GetComponent<Gun>();
        }

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _lastDirection = Vector2.right;
        }

        // Update is called once per frame
        void Update()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
            if (Input.GetKey(KeyCode.Space))
            {
                Fire();
            }

            if (Mathf.Abs(_body.velocity.y) < 0.1f && !IsGrounded)
            {
                IsGrounded = IsOnGround();
            }
        }

        private void FixedUpdate()
        {
            if (_isDead) return;
            Direction = new Vector2(_horizontal, _vertical);
        }

        private void UpdateSkin()
        {
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
            _gun.FireRate = _configManager.GunBaseInfo[_gunSkin].Item2;
            _damage = _configManager.GunBaseInfo[GunSkin].Item1;
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

        public void Completed()
        {
            Debug.Log("Complete");
            _isDead = true;
            Move(Vector2.zero);
        }
    }
}