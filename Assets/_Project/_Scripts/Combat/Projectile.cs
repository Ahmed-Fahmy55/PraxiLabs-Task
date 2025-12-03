using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Praxi.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float _force;


        IObjectPool<Projectile> _pool;
        IObjectPool<ParticleSystem> _effectPool;
        Rigidbody _rb;

        private int _damage;
        private float _delta;

        private const float K_DestroyTime = 10;
        bool _isReleased = false;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }


        private void Update()
        {

            _delta += Time.deltaTime;
            if (_delta > K_DestroyTime && gameObject.activeSelf) ReleaseBullet();

        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.TryGetComponent(out Health health))
            {
                if (_effectPool == null)
                {
                    ReleaseBullet();
                    return;
                }

                ParticleSystem effect = _effectPool.Get();
                effect.transform.parent = other.transform;
                effect.Play();
                StartCoroutine(ReleaseEffecte(effect));
            }
            else
            {
                health?.DealDamage(_damage);
            }
        }

        private IEnumerator ReleaseEffecte(ParticleSystem effect)
        {
            yield return new WaitForSeconds(effect.main.duration);
            _effectPool?.Release(effect);
        }

        private void ReleaseBullet()
        {
            if (_isReleased) return;
            _pool?.Release(this);
            _isReleased = true;
        }

        public void Init(Vector3 target, int damage, IObjectPool<Projectile> bulletPool, IObjectPool<ParticleSystem> effectPool)
        {
            _isReleased = false;
            _damage = damage;
            _pool = bulletPool;
            _effectPool = effectPool;

            Vector3 targetDirection = (target - transform.position).normalized;
            transform.forward = targetDirection;
            _rb.AddForce(targetDirection * _force, ForceMode.Impulse);
        }
    }
}
