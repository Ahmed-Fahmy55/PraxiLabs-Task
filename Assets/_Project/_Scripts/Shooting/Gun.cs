using Praxi.Player.Input;
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Praxi.Shooting
{
    public class Gun : MonoBehaviour
    {
        public event Action<int> OnShoot;
        public event Action OnReload;

        [SerializeField] GunSO _weapon;
        [SerializeField] Transform _firePoint;
        [SerializeField] ParticleSystem[] _hitEffects;

        public bool IsReloading { get; set; }


        InputReader _inputManager;
        Camera _mainCam;
        Animator _animator;
        ParticleSystem _muzzleEffect;
        IObjectPool<Bullet> _bulletPool;
        IObjectPool<ParticleSystem> _effectBool;


        private int _maxMagNumb;
        private int _currentMagNumb;
        private float _fireDelta = 0;

        private int _shootID;
        private int _reLoadID;

        private void Awake()
        {
            _inputManager = GetComponentInChildren<InputReader>();

            _mainCam = Camera.main;
            _animator = GetComponentInChildren<Animator>();

            SetAnimationIDS();

            _bulletPool = new ObjectPool<Bullet>(CreatBullet, OnGetBullet, OnReleaseBullet,
                OnDestroyBullet, maxSize: _weapon.AmmoNumb);


            if (_hitEffects != null && _hitEffects.Length > 0)
                _effectBool = new ObjectPool<ParticleSystem>(CreateHitEffect, OnGetHitEffect, OnReleaseHitEffect,
               OnDestroyHitEffect, true, _weapon.AmmoNumb);

        }


        private void Start()
        {
            _maxMagNumb = _weapon.AmmoNumb;
            _currentMagNumb = _maxMagNumb;
            SetupMuzzleEffect();
            _fireDelta = Time.time + _weapon.FireRate;

        }

        private void Update()
        {
            HandleShooting();
            if (_inputManager.Reload)
            {
                Reload();
                _inputManager.Reload = false;
            }
        }

        private void HandleShooting()
        {
            Vector2 crosshair = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = _mainCam.ScreenPointToRay(crosshair);

            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitPoint, 900))
            {
                if (!IsReloading) Fire(hitPoint.point);
            }
        }


        private void Fire(Vector3 point)
        {
            if (_inputManager.Shoot)
            {
                if (Time.time < _fireDelta) return;

                Bullet bullet = _bulletPool.Get();
                bullet.Init(point, _weapon.Damage, _bulletPool, _effectBool);


                if (_muzzleEffect != null) _muzzleEffect.Play();
                _animator.SetTrigger(_shootID);

                _currentMagNumb--;

                OnShoot?.Invoke(_currentMagNumb);

                if (!_weapon.IsAutomatic) _inputManager.Shoot = false;

                if (_currentMagNumb == 0)
                {
                    Reload();
                }
                _fireDelta = Time.time + _weapon.FireRate;
            }
        }

        private void Reload()
        {
            IsReloading = true;
            _animator.SetTrigger(_reLoadID);
            OnReload?.Invoke();
        }

        public void ResetAmmo()
        {
            _currentMagNumb = _maxMagNumb;
        }

        public int GetMaxMagNumb()
        {
            return _weapon.AmmoNumb;
        }

        private void SetupMuzzleEffect()
        {
            if (_weapon.MuzzleEffect == null) return;
            _muzzleEffect = Instantiate(_weapon.MuzzleEffect);
            _muzzleEffect.transform.parent = _firePoint;
            _muzzleEffect.transform.localPosition = Vector3.zero;
            _muzzleEffect.transform.localRotation = Quaternion.identity;
        }

        private void SetAnimationIDS()
        {
            _shootID = Animator.StringToHash("Shoot");
            _reLoadID = Animator.StringToHash("Reload");
        }

        public Bullet CreatBullet()
        {
            Bullet bullet = Instantiate(_weapon.BulletPrefab, _firePoint.position, Quaternion.identity);
            return bullet;
        }

        private void OnGetBullet(Bullet bullet)
        {
            bullet.transform.position = _firePoint.position;
            bullet.gameObject.SetActive(true);
        }

        private void OnReleaseBullet(Bullet bullet)
        {
            bullet.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            bullet.gameObject.SetActive(false);
        }

        private void OnDestroyBullet(Bullet bullet)
        {
            Destroy(bullet.gameObject);
        }

        private void OnReleaseHitEffect(ParticleSystem system)
        {
            system.gameObject.SetActive(false);
            system.Stop();
        }

        private void OnGetHitEffect(ParticleSystem system)
        {
            system.gameObject.SetActive(true);
        }

        private ParticleSystem CreateHitEffect()
        {
            return Instantiate(_hitEffects[UnityEngine.Random.Range(0, _hitEffects.Length)]);
        }

        private void OnDestroyHitEffect(ParticleSystem system)
        {
            Destroy(system.gameObject);
        }
    }
}
