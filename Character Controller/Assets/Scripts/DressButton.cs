using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DressButton : MonoBehaviour
{
    [SerializeField] CharacterDressing.ClothesType _clothesType;
    public CharacterDressing Character { get; set; }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => Character.WearClothes(_clothesType));
    }
}
