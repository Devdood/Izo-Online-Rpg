using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillHoverController : MonoBehaviour
{
    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Text descriptionText;

    [SerializeField]
    private Text reqLvlText;

    public void Fill(SkillDataHandler skillData)
    {
        this.nameText.text = skillData.name;
        this.descriptionText.text = skillData.description;
        this.reqLvlText.text = skillData.reqLvl.ToString();
    }
}
