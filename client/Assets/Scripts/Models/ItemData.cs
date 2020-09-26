using System.Collections;
using System.Collections.Generic;
using UMA.CharacterSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item Data")]
public class ItemData : ScriptableObject
{
    public int id;
    public string name;
    public Sprite icon;
    public UMAWardrobeRecipe equippable;
    public string description;
    public int reqLevel;
}
