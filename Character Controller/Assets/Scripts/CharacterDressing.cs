using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterDressing : MonoBehaviour
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
