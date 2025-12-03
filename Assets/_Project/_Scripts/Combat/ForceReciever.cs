using UnityEngine;
using UnityEngine.AI;

namespace Praxi.Combat
{
    public class ForceReciever : MonoBehaviour
    {
        [Header("GroundCheck")]
        [SerializeField] private Transform _groundCheckTransorm;
        [SerializeField] private float _groundCheckRad;
        [SerializeField] private LayerMask _groundCheckLayer;
        [SerializeField] private float _drag = 0.3f;

        NavMeshAgent _agent;
        private Vector3 _dampingVelocity;
        private Vector3 _impact;
        private float _verticalVelocity;
        private bool _isGrounded;

        public Vector3 Movement => _impact + Vector3.up * _verticalVelocity;



        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            _isGrounded = Physics.CheckSphere(_groundCheckTransorm.position, _groundCheckRad, _groundCheckLayer) ? true : false;

            if (_verticalVelocity < 0f && _isGrounded)
            {
                _verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, _drag);

            if (_impact.sqrMagnitude < .3f * .3f && _agent)
            {
                _impact = Vector3.zero;
                _agent.enabled = true;
            }
        }


        public void AddForce(Vector3 force)
        {
            _impact += force;
            if (_agent) _agent.enabled = false;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_groundCheckTransorm == null) return;
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawSphere(_groundCheckTransorm.position, _groundCheckRad);
        }
#endif
    }
}
