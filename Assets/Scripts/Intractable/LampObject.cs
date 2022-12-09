using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LampObject : Interactable
{
    [SerializeField] GameObject lamp;
    public override void LeftClickOn()
    {
        base.LeftClickOn();
        if (lamp.activeInHierarchy)
        {
            lamp.SetActive(false);
        }
        else
        {
            lamp.SetActive(true);
        }
    }
}
