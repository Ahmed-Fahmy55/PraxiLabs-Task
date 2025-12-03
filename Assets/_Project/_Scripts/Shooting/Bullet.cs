using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Praxi.Shooting
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float _force;



        IObjectPool<Bullet> _bulletPool;
        IObjectPool<ParticleSystem> _effectPool;
        Rigidbody _rb;

        private int _damage;
        private float _delta;

        private const float K_DestroyTime = 10;



        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }


        private void Update()
        {

            _delta += Time.deltaTime;
            if (_delta > K_DestroyTime && gameObject.activeSelf) _bulletPool?.Release(this);

        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.TryGetComponent(out Health health))
            {
                if (_effectPool == null) return;

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


        public void Init(Vector3 target, int damage, IObjectPool<Bullet> bulletPool, IObjectPool<ParticleSystem> effectPool)
        {
            _damage = damage;
            _bulletPool = bulletPool;
            _effectPool = effectPool;

            Vector3 targetDirection = (target - transform.position).normalized;
            transform.forward = targetDirection;
            _rb.AddForce(targetDirection * _force, ForceMode.Impulse);
        }
    }
}
