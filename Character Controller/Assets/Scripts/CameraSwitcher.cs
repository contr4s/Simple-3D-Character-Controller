using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] Camera _main;
    [SerializeField] Camera _additional;

    bool _isMainActive;

    private void Start()
    {
        ChangeCamera();
    }

    public void ChangeCamera()
    {
        _isMainActive = !_isMainActive;
        _main.gameObject.SetActive(_isMainActive);
        _additional.gameObject.SetActive(!_isMainActive);
    }
}
