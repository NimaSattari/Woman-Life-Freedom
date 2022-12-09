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
    [SerializeField] public GameObject key;
    [SerializeField] int padsNeeded;
    [SerializeField] private int padsNow = 0;
    [SerializeField] GameObject[] afterOpenObjects;
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
        else
        {
            SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.doorlocked);
        }
    }

    public void IncrementPadNum(int numb)
    {
        padsNow += numb;
        if(padsNow == padsNeeded)
        {
            isUnlocked = true;
            Open();
        }
        else
        {
            isUnlocked = false;
            Close();
        }
    }

    public bool TryOpenWithKey(GameObject key)
    {
        if(key == this.key)
        {
            isUnlocked = true;
            Open();
            return true;
        }
        else
        {
            SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.doorlocked);
            return false;
        }
    }

    public void Open()
    {
        SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.dooropen);
        doTween.DoAnimation();
        foreach(GameObject afterObject in afterOpenObjects)
        {
            afterObject.SetActive(true);
        }
        TurnOnOutline(Color.green, true);
        isOpen = true;
        UIActions.instance.ReactToObjectReverse();
    }
    public void Close()
    {
        SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.doorclose);
        doTween.DoAnimationBackward();
        TurnOnOutline(Color.red, true);
        isOpen = false;
        UIActions.instance.ReactToObjectReverse();
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
