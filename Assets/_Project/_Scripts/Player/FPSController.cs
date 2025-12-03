using Praxi.Player.Input;
using System;
using UnityEngine;

namespace Praxi.Player
{
    public class FPSController : MonoBehaviour
    {
        [SerializeField] float _walkSpeed = 4;
        [SerializeField] float _runSpeed = 8;
        [SerializeField] float _speedChangeRate = 10;


        [Header("Rotation settings")]
        [SerializeField] float _rotationSpeed = 1;
        [SerializeField] Transform _camRoot;
        [SerializeField, Range(-360, 360)] float _maxLookAngel;
        [SerializeField, Range(-360, 360)] float _minLookAngel;

        [Header("jumping")]
        [SerializeField] float _jumbHeight;
        [SerializeField] float _gravity = -9.8f;


        [Header("Ground check")]
        [SerializeField] float _groundOfset;
        [SerializeField] float _groundedRadius;
        [SerializeField] LayerMask _groundLayers;



        InputReader _inputReader;
        CharacterController _controller;
        Animator _anim;

        private float _xRotaton;
        private float _yRotaton;
        private float _verticalVelocity;
        private bool _isGrounded = true;
        private float _fallSpeed = 2;
        private int _speedID;
        private int _jumpID;
        private float _blendValue;



        private void Awake()
        {
            _inputReader = GetComponent<InputReader>();
            _controller = GetComponent<CharacterController>();
            _anim = GetComponentInChildren<Animator>();
        }

        private void Start()
        {

            SetupAnimationsIDs();
        }


        private void SetupAnimationsIDs()
        {
            _speedID = Animator.StringToHash("Speed");
            _jumpID = Animator.StringToHash("Jumb");
        }

        private void Update()
        {
            GroundedCheck();
            HandleMovement();
            HandleJumpingAndGravity();

        }

        private void LateUpdate()
        {
            HandleRotation();
        }

        private void HandleJumpingAndGravity()
        {

            if (_isGrounded)
            {

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                if (_inputReader.Jumb)
                {
                    _verticalVelocity = Mathf.Sqrt(_jumbHeight * -2f * _gravity);
                    _anim.SetTrigger(_jumpID);
                    _inputReader.Jumb = false;

                }
            }
            else
            {
                _verticalVelocity += _fallSpeed * _gravity * Time.deltaTime;
            }
        }

        private void HandleRotation()
        {
            _xRotaton -= _inputReader.Look.y * _rotationSpeed * Time.smoothDeltaTime;
            _yRotaton += _inputReader.Look.x * _rotationSpeed * Time.smoothDeltaTime;
            _xRotaton = Mathf.Clamp(_xRotaton, _minLookAngel, _maxLookAngel);
            _camRoot.localRotation = Quaternion.Euler(_xRotaton, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, _yRotaton, 0), Time.smoothDeltaTime * _rotationSpeed * 10);
        }

        private void HandleMovement()
        {

            float speed = _inputReader.Run ? _runSpeed : _walkSpeed;
            if (_inputReader.Move == Vector2.zero) speed = 0;
            Vector3 dir = transform.right * _inputReader.Move.x + transform.forward * _inputReader.Move.y;
            _controller.Move(dir * speed * Time.smoothDeltaTime + new Vector3(0, _verticalVelocity, 0) * Time.smoothDeltaTime);

            _blendValue = Mathf.Lerp(_blendValue, speed, Time.deltaTime * _speedChangeRate);
            if (_blendValue < 0.01f) _blendValue = 0;
            _anim.SetFloat(_speedID, _blendValue);
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundOfset, transform.position.z);
            _isGrounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers, QueryTriggerInteraction.Ignore);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - _groundOfset, transform.position.z), _groundedRadius);
        }
#endif

    }
}
