using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DressButton : MonoBehaviour
{
    [SerializeField] CharacterDressing.ClothesType _clothesType;
    [SerializeField] CharacterDressing _character;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => _character.WearClothes(_clothesType));
    }
}
