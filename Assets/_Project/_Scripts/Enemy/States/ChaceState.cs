using Praxi.Combat;
using Praxi.Enemy.Base;
using Praxi.Enemy.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Praxi.Enemy.States
{
    public class ChaceState : EnemyStateBase
    {


        public ChaceState(EnemyStateMachine stateMachine, NavMeshAgent agent,
             Transform playerTransform, EnemyBaseSO data, Health playerHealth) :
            base(stateMachine, agent, playerTransform, data, playerHealth)
        {

        }

        public override void Enter()
        {

        }

        public override void Tick(float deltaTime)
        {
            if (!IsPlayerInChaceRange())
            {
                _stateMachine.SwitchState(_stateMachine.PatrolState);
                return;
            }

            if (IsInAttackRange())
            {
                _stateMachine.SwitchState(_stateMachine.AttackState);
                return;
            }

            Move(_playerTransform.position);
        }

        public override void Exit()
        {

        }

    }
}
