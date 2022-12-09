using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public static event Action<string, int> Rotated = delegate { };

    public bool coroutineAllowed;

    private int numberShown;
    [SerializeField] public string objectName;
    [SerializeField] public string story;

    [SerializeField] private Outline outline;
    public Outline _outline { get { return outline; } }

    private void Start()
    {
        coroutineAllowed = true;
        numberShown = 5;
        story = numberShown.ToString();
    }

    public void Handle()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(RotateWheel());
        }
    }

    private IEnumerator RotateWheel()
    {
        coroutineAllowed = false;
        for (int i = 0; i<= 11; i++)
        {
            transform.Rotate(-3f, 0f, 0f);
            yield return new WaitForSeconds(0.01f);
        }
        numberShown += 1;

        if(numberShown > 9)
        {
            numberShown = 0;
        }
        story = numberShown.ToString();
        UIActions.instance.ReactToObjectPick(objectName, story);
        Rotated(name, numberShown);
        coroutineAllowed = true;
    }

    public void TurnOnOutline(Color color, bool turnoff)
    {
        outline.enabled = true;
        outline.OutlineColor = color;
        if (turnoff)
        {
            Invoke("TurnOffOutline", 1f);
        }
        else
        {
            CancelInvoke();
        }
    }

    public void TurnOffOutline()
    {
        outline.enabled = false;
        UIActions.instance.ReactToObjectReverse();
    }
}
