using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CodeWheelObject : Interactable
{
    public event Action<GameObject, int> Rotated = delegate { };

    private bool coroutineAllowed;
    private int numberShown;

    private void Start()
    {
        coroutineAllowed = true;
        numberShown = 5;
        objectStory[0] = numberShown.ToString();
    }

    public override void LeftClickOn()
    {
        base.LeftClickOn();
        if (coroutineAllowed)
        {
            StartCoroutine(RotateWheel());
        }
    }

    private IEnumerator RotateWheel()
    {
        coroutineAllowed = false;
        for (int i = 0; i <= 11; i++)
        {
            transform.Rotate(-3f, 0f, 0f);
            yield return new WaitForSeconds(0.01f);
        }
        numberShown += 1;

        if (numberShown > 9)
        {
            numberShown = 0;
        }
        objectStory[0] = numberShown.ToString();
        UIActions.instance.ReactToObjectPick(objectName[0], objectStory[0]);
        Rotated(gameObject, numberShown);
        coroutineAllowed = true;
    }
}
