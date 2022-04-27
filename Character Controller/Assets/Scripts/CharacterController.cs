using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(Animator))]
public class CharacterController: MonoBehaviourPun
{
    public const string JumpingState = "isJumping";
    public const string RunningState = "isRunning";
    public const string TurningState = "turn";

    [Range(5f, 60f)]
    [SerializeField] float _slopeLimit = 45f;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _turnSpeed = 300;
    [SerializeField] bool _allowJump = true;
    [SerializeField] float _jumpSpeed = 4f;

    [SerializeField] CharacterDressing _characterDressing;
    public CharacterDressing CharacterDressing => _characterDressing;

    [SerializeField] Camera _playerCam;
    public Camera Camera => _playerCam;

    public bool IsGrounded { get; private set; }

    private float _forwardInput;
    public float ForwardInput
    {
        get => _forwardInput;
        set
        {
            _forwardInput = value;
            _animator.SetBool(RunningState, !Mathf.Approximately(value, 0));
        }
    }

    private float _turnInput;
    public float TurnInput {
        get => _turnInput;
        set { 
            _turnInput = value;
            _animator.SetFloat("turn", value);
        }
    }

    private bool _jumpInput;
    public bool JumpInput
    {
        get => _jumpInput;
        set
        {
            _jumpInput = value;
            _animator.SetBool(JumpingState, value);
        }
    }

    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    private Animator _animator;
    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            var core = FindObjectOfType<GameCore>();
            core.Init(this);
        }       
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        ProcessInput();
    }

    private void CheckGrounded()
    {
        IsGrounded = false;
        float capsuleHeight = Mathf.Max(_capsuleCollider.radius * 2f, _capsuleCollider.height);
        Vector3 capsuleBottom = transform.TransformPoint(_capsuleCollider.center - Vector3.up * capsuleHeight / 2f);
        float radius = transform.TransformVector(_capsuleCollider.radius, 0f, 0f).magnitude;

        Ray ray = new(capsuleBottom + transform.up * .01f, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, radius * 5f))
        {
            float normalAngle = Vector3.Angle(hit.normal, transform.up);
            if (normalAngle < _slopeLimit)
            {
                float maxDist = radius / Mathf.Cos(Mathf.Deg2Rad * normalAngle) - radius + .02f;
                if (hit.distance < maxDist)
                    IsGrounded = true;
            }
        }
    }

    private void ProcessInput()
    {
        if (TurnInput != 0f)
        {
            float angle = Mathf.Clamp(TurnInput, -1f, 1f) * _turnSpeed;
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * angle);
        }

        if (IsGrounded)
        {
            _rigidbody.velocity = Vector3.zero;

            if (JumpInput && _allowJump)
            {
                _rigidbody.velocity += Vector3.up * _jumpSpeed;
            }

            _rigidbody.velocity += transform.forward * Mathf.Clamp(ForwardInput, -1f, 1f) * _moveSpeed;
        }
        else
        {
            if (!Mathf.Approximately(ForwardInput, 0f))
            {
                Vector3 verticalVelocity = Vector3.Project(_rigidbody.velocity, Vector3.up);
                _rigidbody.velocity = verticalVelocity + transform.forward * Mathf.Clamp(ForwardInput, -1f, 1f) * _moveSpeed / 2f;
            }
        }
    }

    public override bool Equals(object obj)
    {
        return obj is CharacterController controller &&
               base.Equals(obj) &&
               ForwardInput == controller.ForwardInput;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), ForwardInput);
    }
}
