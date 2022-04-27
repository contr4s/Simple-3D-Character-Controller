using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class InputController : MonoBehaviourPun
{
    [SerializeField] CharacterController _characterController;

    private void FixedUpdate()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        int vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        bool jump = Input.GetKey(KeyCode.Space);

        _characterController.ForwardInput = vertical;
        _characterController.TurnInput = horizontal;
        _characterController.JumpInput = jump;
    }
}
