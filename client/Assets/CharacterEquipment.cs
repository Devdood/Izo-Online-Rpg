using System;
using System.Collections;
using System.Collections.Generic;
using UMA.CharacterSystem;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    private Dictionary<string, UMAWardrobeRecipe> equipment = new Dictionary<string, UMAWardrobeRecipe>();
    private DynamicCharacterAvatar avatar;

    void Start()
    {
        avatar = GetComponentInChildren<DynamicCharacterAvatar>();
        GetComponent<Character>().itemContainers[ItemContainerId.EQUIPMENT].OnInventoryChanged += CharacterEquipment_OnInventoryChanged;
    }

    private void CharacterEquipment_OnInventoryChanged(ItemsContainer obj)
    {
        if(avatar)
        {
            avatar.ClearSlots();
            avatar.BuildCharacter();
        }

        foreach (var item in obj.items)
        {
            ItemData itemData = ItemsManager.Instance.GetItemData(item.Value.id);
            if(itemData != null)
            {
                EquipItemVisual(itemData);
            }
        }
    }

    private void EquipItemVisual(ItemData itemData)
    {
        if(itemData.equippable != null)
        {
            if (avatar)
            {
                avatar.SetSlot(itemData.equippable);
                avatar.BuildCharacter();
            }
        }
    }
}
