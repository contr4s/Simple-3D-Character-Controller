using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    [SerializeField] CharacterController _characterPrefab;
    [SerializeField] Vector3 _spawnPosition = new Vector3(0, 4, 0);

    [SerializeField] CameraSwitcher _cameraSwitcher;
    [SerializeField] DressButton[] _dressButtons;

    private void Start()
    {
        PhotonNetwork.Instantiate(_characterPrefab.name, _spawnPosition, Quaternion.identity);
    }

    public void Init(CharacterController character)
    {
        _cameraSwitcher.Additional = character.Camera;
        foreach (var button in _dressButtons)
        {
            button.Character = character.CharacterDressing;
        }
    }
}
