using Praxi.Combat;
using Praxi.Enemy.Base;
using Praxi.Enemy.Data;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Praxi.Enemy.States
{
    [Serializable]
    public class PatrolState : EnemyStateBase
    {
        float _arenaSize;
        Vector3 _targetPoint;
        float _timeToSwitchPoint;
        float _passedTime;

        public PatrolState(float arenaSize, float timeToSwitchPoint, EnemyStateMachine stateMachine, NavMeshAgent agent
             , Transform playerTransform, EnemyBaseSO data, Health playerHealth) :
            base(stateMachine, agent, playerTransform, data, playerHealth)
        {
            _arenaSize = arenaSize;
            _timeToSwitchPoint = timeToSwitchPoint;
        }

        public override void Enter()
        {
            _agent.speed = _data.MoveSpeed;
            _targetPoint = GetRandomPointInSquare();
            Move(_targetPoint);
        }

        public override void Tick(float deltaTime)
        {
            if (IsPlayerInChaceRange())
            {
                Debug.Log("Chase");
                _stateMachine.SwitchState(_stateMachine.ChaceState);
                return;
            }

            _passedTime += deltaTime;
            if (_passedTime >= _timeToSwitchPoint)
            {
                _targetPoint = GetRandomPointInSquare();
                Move(_targetPoint);
                _passedTime = 0f;
            }
        }

        public override void Exit()
        {
        }


        Vector3 GetRandomPointInSquare()
        {
            float half = _arenaSize / 2f;
            float x = UnityEngine.Random.Range(-half, half);
            float z = UnityEngine.Random.Range(-half, half);
            return new Vector3(x, 0, z);
        }
    }
}
