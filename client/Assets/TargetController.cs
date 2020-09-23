using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : Singleton<TargetController>
{
    private Character playerTarget;

    [SerializeField]
    private GameObject targetSelector;

    public event Action<Character> OnTargetChanged = delegate { };

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        targetSelector.gameObject.SetActive(false);
    }

    public void SetTarget(Character target)
    {
        playerTarget = target;
        targetSelector.gameObject.SetActive(true);
        OnTargetChanged(target);
    }

    void Update()
    {
        if (playerTarget != null)
        {
            targetSelector.transform.position = playerTarget.transform.position;
        }
        else
        {
            if (targetSelector.activeInHierarchy)
            {
                targetSelector.gameObject.SetActive(false);
            }
        }
    }
}
