using Praxi.Combat;
using Praxi.Enemy.Base;
using Praxi.Enemy.Data;
using UnityEngine;
using UnityEngine.AI;
using Zone8.Events;

namespace Praxi.Enemy.States
{
    public class DieState : EnemyStateBase
    {

        public DieState(EnemyStateMachine stateMachine, NavMeshAgent agent,
             Transform playerTransform, EnemyBaseSO data, Health playerHealth) :
            base(stateMachine, agent, playerTransform, data, playerHealth)
        {

        }

        public override void Enter()
        {
            _agent.isStopped = true;

            EventBus<EnemyDieEvent>.Raise(new EnemyDieEvent(_stateMachine));
            _stateMachine.Pool.Release(_stateMachine);
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void Exit()
        {
        }
    }
}
