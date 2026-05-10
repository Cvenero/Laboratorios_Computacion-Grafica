using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Movimiento")]
        public float MoveSpeed = 2.0f;
        public float SprintSpeed = 5.33f;
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;
        public float SpeedChangeRate = 10.0f;

        [Header("Salto y Gravedad")]
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;
        public float JumpTimeout = 0.50f;
        public float FallTimeout = 0.15f;

        [Header("Suelo")]
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public LayerMask GroundLayers;

        // Referencia a cámara — la usarás cuando implementes primera/tercera persona
        private GameObject _mainCamera;

        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private Animator _animator;

        private float _speed;
        private float _animationBlend;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private float _rotationVelocity;
        private float _targetRotation;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        private bool _grounded;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            _animator = GetComponent<Animator>();
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _grounded = IsGrounded();
            JumpAndGravity();
            Move();
        }

        private void Move()
        {
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            if (_input.move == Vector2.zero) targetSpeed = 0f;

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            float currentSpeed = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z).magnitude;
            _speed = Mathf.Abs(currentSpeed - targetSpeed) > 0.1f
                ? Mathf.Round(Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate) * 1000f) / 1000f
                : targetSpeed;

            if (_input.move != Vector2.zero)
            {
                Vector3 inputDir = new Vector3(_input.move.x, 0f, _input.move.y).normalized;
                _targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg
                                   + _mainCamera.transform.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0f,
                    Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
                        ref _rotationVelocity, RotationSmoothTime), 0f);
            }

            Vector3 moveDir = Quaternion.Euler(0f, _targetRotation, 0f) * Vector3.forward;
            _controller.Move(moveDir.normalized * (_speed * Time.deltaTime)
                           + new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);

            if (_animator)
            {
                _animator.SetFloat("Speed", _animationBlend);
                _animator.SetFloat("MotionSpeed", _input.move.magnitude);
                _animator.SetBool("Grounded", _grounded);
            }
        }

        private void JumpAndGravity()
        {
            if (_grounded)
            {
                _fallTimeoutDelta = FallTimeout;
                if (_verticalVelocity < 0f) _verticalVelocity = -2f;

                if (_input.jump && _jumpTimeoutDelta <= 0f)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    if (_animator) _animator.SetBool("Jump", true);
                }

                if (_jumpTimeoutDelta >= 0f) _jumpTimeoutDelta -= Time.deltaTime;

                if (_animator)
                {
                    _animator.SetBool("Jump", false);
                    _animator.SetBool("FreeFall", false);
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;
                _input.jump = false;

                if (_fallTimeoutDelta >= 0f) _fallTimeoutDelta -= Time.deltaTime;
                else if (_animator) _animator.SetBool("FreeFall", true);
            }

            if (_verticalVelocity < _terminalVelocity)
                _verticalVelocity += Gravity * Time.deltaTime;
        }

        private bool IsGrounded() =>
            Physics.CheckSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
        
                
        private void OnLand(AnimationEvent animationEvent) { }
        private void OnFootstep(AnimationEvent animationEvent) { }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Debug.Log("Colision con: " + hit.gameObject.name + " Tag: " + hit.gameObject.tag);

            if (hit.gameObject.CompareTag("WinZone"))
            {
                GameManager.instance.WinGame();
            }
        }

    }


}
