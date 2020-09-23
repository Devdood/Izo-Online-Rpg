using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITarget : MonoBehaviour
{
    [SerializeField]
    private GameObject targetPanel;

    [SerializeField]
    private Text targetName;

    [SerializeField]
    private Text lvlText;

    [SerializeField]
    private Slider healthBar;

    private void Start()
    {
        TargetController.Instance.OnTargetChanged += Instance_OnTargetChanged;
    }

    private void Instance_OnTargetChanged(Character obj)
    {
        targetPanel.SetActive(obj != null);
        if (obj != null)
        {
            targetName.text = obj.Data.nickname;
            lvlText.text = string.Format("Lv. {0}", obj.Data.lvl);
        }
    }
}
