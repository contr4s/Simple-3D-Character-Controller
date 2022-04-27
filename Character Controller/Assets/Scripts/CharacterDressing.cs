using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class CharacterDressing : MonoBehaviour, IPunObservable
{
    [SerializeField] private WearPair[] _clothes;

    private Dictionary<ClothesType, WearPair> _availibleClothes;

    private void Awake()
    {
        _availibleClothes = _clothes.ToDictionary(x => x.type);
    }

    public void WearClothes(ClothesType type)
    {
        var wear = _availibleClothes[type];
        bool isWeared = wear.wearSet.activeInHierarchy;
        wear.wearSet.SetActive(!isWeared);
        wear.nakedSet.SetActive(isWeared);
    }

    public void WearClothes(ClothesType type, bool putOn)
    {
        var wear = _availibleClothes[type];
        wear.wearSet.SetActive(putOn);
        wear.nakedSet.SetActive(!putOn);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            foreach (var val in _availibleClothes)
            {
                stream.SendNext(val.Value.wearSet.activeInHierarchy);
            }
        }
        else
        {
            WearClothes(ClothesType.Shirt, (bool)stream.ReceiveNext());
            WearClothes(ClothesType.Pants, (bool)stream.ReceiveNext());
            WearClothes(ClothesType.Boots, (bool)stream.ReceiveNext());
        }
    }

    public enum ClothesType
    {
        Shirt,
        Pants,
        Boots,
    }

    [Serializable]
    private struct WearPair
    {
        public ClothesType type;
        public GameObject wearSet;
        public GameObject nakedSet;
    }
}
