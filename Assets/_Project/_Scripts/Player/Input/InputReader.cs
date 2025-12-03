using UnityEngine;
using UnityEngine.InputSystem;

namespace Praxi.Player.Input
{
    public class InputReader : MonoBehaviour, Controls.IPlayerActions
    {
        public Vector2 Move { get; private set; }

        public Vector2 Look { get; private set; }

        public bool Jumb { get; set; }
        public bool Run { get; set; }
        public bool Shoot { get; set; }
        public bool Reload { get; set; }

        Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            Jumb = context.ReadValueAsButton();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            Shoot = context.ReadValueAsButton();
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            Reload = context.ReadValueAsButton();
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            Run = context.ReadValueAsButton();
        }
    }
}
