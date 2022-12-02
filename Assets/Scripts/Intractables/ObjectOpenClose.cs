using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOpenClose : MonoBehaviour
{
    [SerializeField] DoTweenActions doTween;
    [SerializeField] private Outline outline;
    public Outline _outline { get { return outline; } }

    private bool isOpen = false;
    [SerializeField] public string objectName;
    [SerializeField] public bool isUnlocked;

    public void Handle()
    {
        if (isUnlocked)
        {
            if (isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }

    public void Open()
    {
        doTween.DoAnimation();
        TurnOnOutline(Color.green, true);
        isOpen = true;
        UIActions.instance.open.SetActive(false);
        UIActions.instance.nameObject.SetActive(false);
    }
    public void Close()
    {
        doTween.DoAnimationBackward();
        TurnOnOutline(Color.red, true);
        isOpen = false;
        UIActions.instance.open.SetActive(false);
        UIActions.instance.nameObject.SetActive(false);
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
        UIActions.instance.open.SetActive(false);
        UIActions.instance.nameObject.SetActive(false);
    }
}
