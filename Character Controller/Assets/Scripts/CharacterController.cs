using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class CharacterController : MonoBehaviour
{
    [Range(5f, 60f)]
    [SerializeField] float _slopeLimit = 45f;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _turnSpeed = 300;
    [SerializeField] bool _allowJump = true;
    [SerializeField] float _jumpSpeed = 4f;

    public bool IsGrounded { get; private set; }
    public float ForwardInput { get; set; }
    public float TurnInput { get; set; }
    public bool JumpInput { get; set; }

    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
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
}
