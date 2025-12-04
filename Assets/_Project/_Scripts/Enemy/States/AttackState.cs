using Praxi.Combat;
using Praxi.Enemy.Base;
using Praxi.Enemy.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;


namespace Praxi.Enemy.States
{
    public class AttackState : EnemyStateBase
    {
        bool _isAttacking;
        Transform _projectilePivot;

        IObjectPool<Projectile> _projectilePool;


        public AttackState(Transform projectilePivot, EnemyStateMachine stateMachine, NavMeshAgent agent,
             Transform playerTransform, EnemyBaseSO data, Health playerHealth) :
            base(stateMachine, agent, playerTransform, data, playerHealth)
        {
            _projectilePivot = projectilePivot;
        }

        public override void Enter()
        {
            if (_projectilePool == null && !_data.IsMelee)
            {
                _projectilePool = new ObjectPool<Projectile>(CreateProjectile,
                                                             OnGetProjectile,
                                                             OnReleaseProjectile,
                                                             OnDestroyProjectile,
                                                             true,
                                                             maxSize: 50);
            }

            _agent.isStopped = true;
            _agent.updateRotation = false;

        }



        public override void Tick(float deltaTime)
        {
            if (!_isAttacking) _stateMachine.StartCoroutine(AttackRoutine());
        }

        public override void Exit()
        {
            _agent.isStopped = false;
            _isAttacking = false;
            _agent.updateRotation = true;
        }


        private IEnumerator AttackRoutine()
        {
            _isAttacking = true;
            FaceDirection(_playerTransform.position);
            Attack();

            yield return new WaitForSeconds(_data.AttackCooldown);

            Debug.Log("Patrol");
            _stateMachine.SwitchState(_stateMachine.PatrolState);
        }

        private void Attack()
        {
            if (_data.IsMelee)
            {
                Debug.Log("Melee Attack");
                _playerHealth?.DealDamage(_data.Damage);
            }
            else
            {
                Debug.Log("Ranged Attack");
                Projectile projectile = _projectilePool.Get();
                projectile.Init(_playerTransform.position + Vector3.up, _data.Damage, _projectilePool, null);
            }
        }


        private void OnDestroyProjectile(Projectile projectile)
        {
            GameObject.Destroy(projectile.gameObject);
        }

        private void OnReleaseProjectile(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
            projectile.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }

        private void OnGetProjectile(Projectile projectile)
        {
            projectile.transform.position = _projectilePivot.position;
            projectile.gameObject.SetActive(true);
        }

        private Projectile CreateProjectile()
        {
            return GameObject.Instantiate(_data.Projectile, _projectilePivot.position, Quaternion.identity);
        }
    }
}
