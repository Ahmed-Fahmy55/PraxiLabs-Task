using UnityEngine;

namespace Praxi.StateMachine
{
    public abstract class StateMachineBase : MonoBehaviour
    {
        private StateBase _currentState;


        protected virtual void Start()
        {
            if (_currentState != null)
            {
                _currentState.Enter();
            }
            else
            {
                _currentState = GetIntialState();
                _currentState.Enter();
            }
        }
        protected virtual void Update()
        {
            _currentState?.Tick(Time.deltaTime);
        }

        public void SwitchState(StateBase newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        public abstract StateBase GetIntialState();
    }
}
