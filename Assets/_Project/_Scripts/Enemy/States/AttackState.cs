using Praxi.Combat;
using Praxi.Enemy.Base;
using Praxi.Enemy.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Praxi.Enemy.States
{
    public class AttackState : EnemyStateBase
    {

        private float _passedTimeSinceLastAttack;

        public AttackState(EnemyStateMachine stateMachine, NavMeshAgent agent,
             Transform playerTransform, EnemyBaseSO data, Health playerHealth) :
            base(stateMachine, agent, playerTransform, data, playerHealth)
        {

        }

        public override void Enter()
        {
            _agent.isStopped = true;
        }

        public override void Tick(float deltaTime)
        {
            if (!IsInAttackRange())
            {
                Debug.Log("patrol");
                _stateMachine.SwitchState(_stateMachine.PatrolState);
                return;
            }

            if (_passedTimeSinceLastAttack < _data.AttackCooldown)
            {
                _passedTimeSinceLastAttack += deltaTime;
            }
            else
            {
                _passedTimeSinceLastAttack = 0f;
                Attack();
            }
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
                _playerHealth?.DealDamage(_data.Damage);
            }
            _stateMachine.SwitchState(_stateMachine.PatrolState);
        }

        public override void Exit()
        {
            _agent.isStopped = false;
        }
    }
}
