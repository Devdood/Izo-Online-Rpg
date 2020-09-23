using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillsPanel : MonoBehaviour
{
    private Vector3 draggableStartPosition;

    [SerializeField]
    private List<int> skills = new List<int>();

    [SerializeField]
    private SkillHoverController skillHoverController;

    private UIPanel uiPanelSkillHoverController;

    private void Start()
    {
        PlayerController.Instance.OnLocalPlayerChanged += Instance_OnLocalPlayerChanged;

        uiPanelSkillHoverController = skillHoverController.GetComponent<UIPanel>();
    }

    private void Instance_OnLocalPlayerChanged(Character obj)
    {
        List<SkillDataHandler> classSkills = SkillsManager.Instance.GetSkillsForClass((Class)obj.Data.@class);
        SkillButton[] bts = GetComponentsInChildren<SkillButton>();
        for (int i = 0; i < bts.Length; i++)
        {
            SkillDataHandler handler = classSkills[i];
            bts[i].Fill(classSkills[i].id);

            var draggable = bts[i].GetComponentInChildren<DraggableButton>();
            draggable.OnHover.AddListener(delegate { FillSkillHoverWithSkillData(handler); });
            draggable.OnExitHover.AddListener(delegate { skillHoverController.GetComponent<UIPanel>().Deactivate(); });
        }
    }

    public void FillSkillHoverWithSkillData(SkillDataHandler skillData)
    {
        skillHoverController.Fill(skillData);
        uiPanelSkillHoverController.Activate();
    }

    private void Update()
    {
        if(uiPanelSkillHoverController.Active)
        {
            uiPanelSkillHoverController.transform.position = Input.mousePosition;
        }
    }
}
