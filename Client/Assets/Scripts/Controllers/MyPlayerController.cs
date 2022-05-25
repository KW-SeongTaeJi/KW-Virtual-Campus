using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MyPlayerController : PlayerController
{
    [Header("Cinemachine")]
    [SerializeField, Tooltip("How far in degrees can you move the camera up")]
    protected float topClamp = 70.0f;
    [SerializeField, Tooltip("How far in degrees can you move the camera down")]
    protected float bottomClamp = -30.0f;
    [SerializeField, Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    protected float cameraAngleOverride = 0.0f;
    [SerializeField, Tooltip("For locking the camera position on all axis")]
    protected bool lockCameraPosition = false;
    [SerializeField, Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    protected GameObject cinemachineCameraTarget;

    [SerializeField]
    private LayerMask buildingLayer;

    // cinemachine
    protected float _cinemachineTargetYaw;
    protected float _cinemachineTargetPitch;

    protected const float _threshold = 0.01f;

    MyPlayerInput _input;
    GameObject _mainCamera;

    float _lastTargetSpeed = 0.0f;
    float _lastTargetRotation = 0.0f;
    bool _lastJumpInput = false;


    void Awake()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    protected override void Init()
    {
        base.Init();
        _input = ((GameScene)Managers.SceneLoad.CurrentScene).GetComponent<MyPlayerInput>();
    }

    protected override void OnUpdate()
    {
        GroundedCheck();
        JumpAndGravity();
        Move();
        RaycastBuilding();
        CheckChanges();
    }

    protected override void OnLateUpdate()
    {
        CameraRotation();
    }

    protected override void JumpAndGravity()
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
            if (_input.Jump && _jumpTimeoutDelta <= 0.0f)
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

    protected override void Move()
    {
        // Set target speed based on move speed, sprint speed and if sprint is pressed
        _targetSpeed = _input.Sprint ? sprintSpeed : moveSpeed;

        /* A simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon */
        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_input.Move == Vector2.zero)
            _targetSpeed = 0.0f;

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

        // Normalise input direction
        Vector3 inputDirection = new Vector3(_input.Move.x, 0.0f, _input.Move.y).normalized;
       
        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input, rotate player when the player is moving
        if (_input.Move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
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

    void RaycastBuilding()
    {
        RaycastHit hit;
        //Debug.DrawRay(transform.position, (transform.forward + new Vector3(0, 0.2f, 0)) * 50, Color.red);
        if (Physics.Raycast(transform.position, transform.forward + new Vector3(0,0.2f,0), out hit, 50, buildingLayer))
        {
            switch (hit.transform.name)
            {
                case "비마":
                    ((UI_GameScene)Managers.UI.SceneUI).SetBuildingButton("비마관");
                    break;
                case "한울":
                    ((UI_GameScene)Managers.UI.SceneUI).SetBuildingButton("한울관");
                    break;
                case "화도":
                    ((UI_GameScene)Managers.UI.SceneUI).SetBuildingButton("화도관");
                    break;
                case "중앙도서관":
                    ((UI_GameScene)Managers.UI.SceneUI).SetBuildingButton("중앙도서관");
                    break;
                case "옥의":
                    ((UI_GameScene)Managers.UI.SceneUI).SetBuildingButton("옥의관");
                    break;
                case "새빛":
                    ((UI_GameScene)Managers.UI.SceneUI).SetBuildingButton("새빛관");
                    break;
            }
        }
        else
        {
            ((UI_GameScene)Managers.UI.SceneUI).UnsetBuildingButton();
        }
    }

    void CheckChanges()
    {
        // If movement chage, send C_Move packet to server
        if (_lastTargetSpeed != _targetSpeed || _lastTargetRotation != _targetRotation ||
            _lastJumpInput != _input.Jump)
        {
            C_Move movePacket = new C_Move();
            movePacket.Position = new Vector3D();
            movePacket.Position.X = transform.position.x;
            movePacket.Position.Y = transform.position.y;
            movePacket.Position.Z = transform.position.z;
            movePacket.RotationY = transform.eulerAngles.y;
            movePacket.TargetSpeed = _targetSpeed;
            movePacket.TargetRotation = _targetRotation;
            movePacket.Jump = _input.Jump;
            Managers.Network.Send(movePacket);

            // update last movement
            _lastTargetSpeed = _targetSpeed;
            _lastTargetRotation = _targetRotation;
            _lastJumpInput = _input.Jump;
        }
    }

    void CameraRotation()
    {
        // If there is an input and camera position is not fixed
        if (_input.Look.sqrMagnitude >= _threshold && !lockCameraPosition)
        {
            _cinemachineTargetYaw += _input.Look.x * Time.deltaTime;
            _cinemachineTargetPitch += _input.Look.y * Time.deltaTime;
        }

        // Clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

        // Cinemachine will follow this target
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
