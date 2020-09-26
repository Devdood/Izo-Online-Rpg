using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBarSlot : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text hotkeyText;

    private CanvasGroup group;
    private int skillId;

    private void Awake()
    {
        group = icon.GetComponent<CanvasGroup>();
        group.alpha = 0;

        GetComponent<Button>().onClick.AddListener(Click);
    }

    public void Click()
    {
        if(skillId != 0)
        {
            Connection.Instance.SendData(new UseSkillPacket(skillId));
        }
    }

    public void SetHotkey(string key)
    {
        hotkeyText.text = key;
    }

    public void Fill(int skillId)
    {
        if(SkillsManager.Instance.GetSkillData(skillId, out SkillDataHandler handler))
        {
            group.alpha = 1;
            icon.sprite = handler.icon;
            this.skillId = handler.id;
        }
        else
        {
            group.alpha = 0;
            this.skillId = 0;
        }
    }
}
