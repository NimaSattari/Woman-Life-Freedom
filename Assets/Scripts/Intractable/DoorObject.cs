using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider),typeof(DoTweenActions))]
public class DoorObject : Interactable
{
    [SerializeField] public bool isOpen, isUnlocked;

    [Header("If has things inside")]
    [SerializeField] public GameObject[] insideObjects;

    [Header("If opens with key")]
    [SerializeField] public GameObject key;

    [Header("If opens with pads")]
    [SerializeField] int padsNeeded;
    private int padsNow = 0;

    [Header("If opens with code")]
    [SerializeField] private int[] correctCombination;
    [SerializeField] private GameObject[] wheels;

    private DoTweenActions doTween;
    [SerializeField] private int[] result;

    private void Start()
    {
        result = new int[] { 5, 5, 5 };
        doTween = GetComponent<DoTweenActions>();
        foreach(GameObject wheel in wheels)
        {
            wheel.GetComponent<CodeWheelObject>().Rotated += CheckResults;
        }
    }

    private void CheckResults(GameObject wheel, int number)
    {
        int numb = Random.Range(0, 2);
        if (numb == 0)
        {
            SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.codeshuffle1);
        }
        else
        {
            SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.codeshuffle2);
        }
        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i] == wheel)
            {
                result[i] = number;
            }
        }
        if (result[0] == correctCombination[0] && result[1] == correctCombination[1] && result[2] == correctCombination[2])
        {
            isUnlocked = true;
            Open();
            SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.codesolve);
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject wheel in wheels)
        {
            wheel.GetComponent<CodeWheelObject>().Rotated -= CheckResults;
        }
    }

    public override void LeftClickOn()
    {
        base.LeftClickOn();
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

    public void Open()
    {
        SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.dooropen);
        doTween.DoAnimation();
        foreach (GameObject insideObject in insideObjects)
        {
            insideObject.SetActive(true);
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
    public void IncrementPadNum(int numb)
    {
        padsNow += numb;
        if (padsNow == padsNeeded)
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
        if (key == this.key)
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
}
