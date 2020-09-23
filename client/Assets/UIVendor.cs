using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVendor : ItemsContainerInventory
{
    public static UIVendor Instance;

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
    }
}
