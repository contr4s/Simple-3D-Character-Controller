using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] Transform _referenceTransform;
    [SerializeField] float _collisionOffset = 0.3f;
    [SerializeField] float _cameraSpeed = 15f; 

    Vector3 _defaultPos;
    Vector3 _directionNormalized;
    Transform _parentTransform;
    float _defaultDistance;

    void Start()
    {
        _defaultPos = transform.localPosition;
        _directionNormalized = _defaultPos.normalized;
        _parentTransform = transform.parent;
        _defaultDistance = Vector3.Distance(_defaultPos, Vector3.zero);
    }

    void LateUpdate()
    {
        MoveToSafePos();
    }

    private void MoveToSafePos()
    {
        Vector3 currentPos = _defaultPos;
        Vector3 dirTmp = _parentTransform.TransformPoint(_defaultPos) - _referenceTransform.position;
        if (Physics.SphereCast(_referenceTransform.position, _collisionOffset, dirTmp, out RaycastHit hit, _defaultDistance))
        {
            currentPos = (_directionNormalized * (hit.distance - _collisionOffset));
            transform.localPosition = currentPos;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, Time.deltaTime * _cameraSpeed);
        }
    }
}
