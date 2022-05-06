using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField, Tooltip("Move speed of the character in m/s")]
    protected float moveSpeed = 2.0f;
    [SerializeField, Tooltip("Sprint speed of the character in m/s")]
    protected float sprintSpeed = 5.335f;
    [SerializeField, Tooltip("Acceleration and deceleration rate")]
    protected float speedChangeRate = 10.0f;
    [SerializeField, Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    protected float rotationSmoothTime = 0.12f;

    [Header("Player Grounded")]
    [SerializeField, Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    protected bool grounded = true;
    [SerializeField, Tooltip("Useful for rough ground")]
    protected float groundedOffset = -0.5f;
    [SerializeField, Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    protected float groundedRadius = 1.5f;
    [SerializeField, Tooltip("What layers the character uses as ground")]
    protected LayerMask groundLayers;

    [Space(10)]
    [SerializeField, Tooltip("The height the player can jump")]
    protected float jumpHeight = 3.0f;
    [SerializeField, Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    protected float gravity = -15.0f;

    [Space(10)]
    [SerializeField, Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    protected float jumpTimeout = 0.30f;
    [SerializeField, Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    protected float fallTimeout = 0.25f;

    protected bool _hasAnimator;

    // Animation IDs
    protected int _animIDSpeed;
    protected int _animIDGrounded;
    protected int _animIDJump;
    protected int _animIDFreeFall;
    protected int _animIDMotionSpeed;

    // Player
    protected float _targetSpeed = 0.0f;
    protected float _speedOffset = 0.1f;
    protected float _speed;
    protected float _animationBlend;
    protected float _targetRotation = 0.0f;
    protected float _rotationVelocity;
    protected float _verticalVelocity;
    protected float _terminalVelocity = 53.0f;

    // Timeout deltatime
    protected float _jumpTimeoutDelta;
    protected float _fallTimeoutDelta;

    protected Animator _animator;
    protected CharacterController _controller;

    // Using for movement sycn
    public float TargetSpeed
    {
        get { return _targetSpeed; }
        set { _targetSpeed = value; }
    }
    public float TargetRotation
    {
        get { return _targetRotation; }
        set { _targetRotation = value; }
    }
    public bool Jump { get; set; }
    public bool Sync { get; set; } = false;
    public Vector3 Position { get; set; }
    public float RotationY { get; set; }


    void Start()
    {
        Init();
    }

    void Update()
    {
        OnUpdate();
    }

    void LateUpdate()
    {
        OnLateUpdate();
    }

    protected virtual void Init()
    {
        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();

        AssignAnimationIDs();

        // Reset timeout deltatime 
        _jumpTimeoutDelta = jumpTimeout;
        _fallTimeoutDelta = fallTimeout;
    }

    protected virtual void OnUpdate()
    {
        GroundedCheck();
        JumpAndGravity();
        Move();
    }

    protected virtual void OnLateUpdate()
    {
        SyncPosition();
    }

    protected void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    protected void GroundedCheck()
    {
        // Check if player is on ground using sphere 
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);

        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, grounded);
        }
    }

    protected virtual void JumpAndGravity()
    {
        if (grounded)
        {
            // Reset the fall timeout timer
            _fallTimeoutDelta = fallTimeout;

            // TODO : state machine
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);
            }

            // Stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (Jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, true);
                }
            }

            // Jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // Reset the jump timeout timer
            _jumpTimeoutDelta = jumpTimeout;

            // Fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }
            }
        }

        // Apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += gravity * Time.deltaTime;
        }
    }

    protected virtual void Move()
    {
        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        // Accelerate or decelerate to target speed
        if (currentHorizontalSpeed < _targetSpeed - _speedOffset || currentHorizontalSpeed > _targetSpeed + _speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note: T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, _targetSpeed, Time.deltaTime * speedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            // current speed is in target speed range
            _speed = _targetSpeed;
        }
        _animationBlend = Mathf.Lerp(_animationBlend, _targetSpeed, Time.deltaTime * speedChangeRate);

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input, rotate player when the player is moving
        if (_targetSpeed != 0.0f)
        {
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // Move the player (xz + y)
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, 1.0f);
        }
    }

    void SyncPosition()
    {
        // Sync position when receive packet and not jump
        if (Sync)
        {
            Vector3 startPos = transform.position;
            Vector3 destPos = Position;
            Quaternion startRot = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            Quaternion destRot = Quaternion.Euler(0, RotationY, 0);
            for (float t = 0.2f; t <= 1.0f; t += 0.2f)
            {
                transform.position = Vector3.Lerp(startPos, destPos, t);
                transform.rotation = Quaternion.Lerp(startRot, destRot, t);
            }
            Sync = false;
        }
    }
}
