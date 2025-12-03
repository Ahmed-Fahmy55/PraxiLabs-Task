using Praxi.Combat;
using Praxi.Enemy.Data;
using Praxi.Enemy.States;
using Praxi.Player;
using Praxi.StateMachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

namespace Praxi.Enemy.Base
{
    public class EnemyStateMachine : StateMachineBase
    {
        [SerializeField] float _arenaSize = 9;
        [SerializeField] float _timeToSwitchPatrolPoint = 5f;




        protected EnemyBaseSO _data;
        protected Health _health;
        protected Health _playerHealth;
        protected Transform _playerTransform;
        protected NavMeshAgent _agent;
        public IObjectPool<EnemyStateMachine> Pool { get; private set; }

        public PatrolState PatrolState { get; private set; }
        public ChaceState ChaceState { get; private set; }
        public AttackState AttackState { get; private set; }

        public DieState DieState { get; private set; }





        private void Awake()
        {
            _health = GetComponent<Health>();
            _playerTransform = FindAnyObjectByType<FPSController>().transform;
            _playerHealth = _playerTransform.GetComponent<Health>();
            _agent = GetComponent<NavMeshAgent>();

        }

        protected override void Start()
        {
            base.Start();
        }

        private void OnEnable()
        {
            _health.OnDie += Kill;
        }

        private void OnDisable()
        {
            _health.OnDie -= Kill;
        }

        public void Setup(EnemyBaseSO data, IObjectPool<EnemyStateMachine> pool)
        {
            _data = data;
            Pool = pool;
            SwitchState(PatrolState);
            SetupStates();
        }

        private void SetupStates()
        {
            PatrolState = new PatrolState(_arenaSize, _timeToSwitchPatrolPoint, this, _agent, _playerTransform,
                _data, _playerHealth);
            ChaceState = new ChaceState(this, _agent, _playerTransform, _data, _playerHealth);
            AttackState = new AttackState(this, _agent, _playerTransform, _data, _playerHealth);
            DieState = new DieState(this, _agent, _playerTransform, _data, _playerHealth);
        }

        public void Kill()
        {
            SwitchState(DieState);
        }

        public override StateBase GetIntialState()
        {
            return PatrolState;
        }
    }
}
