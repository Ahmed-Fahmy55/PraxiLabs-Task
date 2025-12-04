using Praxi.Combat;
using Praxi.Enemy.Base;
using Praxi.Enemy.Data;
using Praxi.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Praxi.Enemy.States
{
    public abstract class EnemyStateBase : StateBase
    {

        protected EnemyStateMachine _stateMachine;
        protected NavMeshAgent _agent;
        protected Transform _playerTransform;
        protected EnemyBaseSO _data;
        protected Health _playerHealth;

        public EnemyStateBase(EnemyStateMachine stateMachine, NavMeshAgent agent,
            Transform playerTransform, EnemyBaseSO data, Health playerHealth)
        {
            _stateMachine = stateMachine;
            _agent = agent;
            _playerTransform = playerTransform;
            _data = data;
            _playerHealth = playerHealth;
        }

        protected void Move(Vector3 motion)
        {
            _agent.isStopped = false;

            if (_agent.isOnNavMesh)
            {
                _agent.SetDestination(motion);
            }
        }

        protected void FaceDirection(Vector3 dir)
        {

            Vector3 lookPos = dir - _stateMachine.transform.position;
            lookPos.y = 0f;

            _stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
        }

        protected bool IsPlayerInChaceRange()
        {
            if (_playerHealth.IsDead) return false;
            float distanceToPlayer = (_stateMachine.transform.position - _playerTransform.position).sqrMagnitude;
            return distanceToPlayer <= _data.DetectionRange * _data.DetectionRange;
        }

        protected bool IsInAttackRange()
        {
            if (_playerHealth.IsDead) return false;
            float distance = (_stateMachine.transform.position - _playerTransform.position).sqrMagnitude;
            return distance <= _data.AttackRange * _data.AttackRange;
        }

    }
}
