using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UISkillBar : Singleton<UISkillBar>
{
    private List<SkillBarSlot> slots = new List<SkillBarSlot>();

    private void Awake()
    {
        Instance = this;
    }

    public void UseSlot(int slot)
    {
        slots[slot].Click();
    }

    private void Start()
    {
        foreach (var item in GetComponentsInChildren<DraggableButton>())
        {
            item.OnDrop.AddListener((ev) =>
            {
                SkillBarSlot slot = item.GetComponentInParent<SkillBarSlot>();
                DraggableButton dragged = ev.pointerDrag.GetComponent<DraggableButton>();
                slot.Fill(dragged.GetComponentInParent<SkillButton>().skillId);
            });
        }

        slots = GetComponentsInChildren<SkillBarSlot>().ToList();
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetHotkey((i + 1).ToString());
        }
    }
}
