using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider),typeof(DoTweenActions))]
public class DoorObject : Interactable
{
    [SerializeField] public bool isOpen, isUnlocked;
    [SerializeField] UnityEvent afterfirstOpenEvent;
    [SerializeField] UnityEvent aftereveryOpenEvent;
    [SerializeField] UnityEvent afterfirstCloseEvent;
    [SerializeField] UnityEvent aftereveryCloseEvent;
    private bool isFirstOpen, isFirstClose;

    [Header("If has things inside")]
    [SerializeField] public GameObject[] insideObjects;

    [Header("If opens with key")]
    [SerializeField] public GameObject key;
    [SerializeField] UnityEvent afterfirstKeyTryEvent;
    [SerializeField] UnityEvent aftereveryKeyTryEvent;
    private bool isFirstkeyTry;

    [Header("If opens with pads")]
    [SerializeField] int padsNeeded;
    [SerializeField] UnityEvent afterfirstPadTryEvent;
    [SerializeField] UnityEvent aftereveryPadTryEvent;
    private bool isFirstPadTry;

    [Header("If opens with code")]
    [SerializeField] private int[] correctCombination;
    [SerializeField] private GameObject[] wheels;
    [SerializeField] UnityEvent afterfirstCodeCheckTryEvent;
    [SerializeField] UnityEvent aftereveryCodeCheckTryEvent;
    private bool isFirstCodeCheckTry;

    private DoTweenActions doTween;
    private int[] result;
    private int padsNow = 0;

    private void Start()
    {
        result = new int[] { 5, 5, 5 };
        doTween = GetComponent<DoTweenActions>();
        foreach(GameObject wheel in wheels)
        {
            wheel.GetComponent<CodeWheelObject>().Rotated += CodeCheckResults;
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject wheel in wheels)
        {
            wheel.GetComponent<CodeWheelObject>().Rotated -= CodeCheckResults;
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
        if (isFirstOpen)
        {
            afterfirstOpenEvent.Invoke();
        }
        else
        {
            aftereveryOpenEvent.Invoke();
        }
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
        if (isFirstClose)
        {
            afterfirstCloseEvent.Invoke();
        }
        else
        {
            aftereveryCloseEvent.Invoke();
        }
        SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.doorclose);
        doTween.DoAnimationBackward();
        TurnOnOutline(Color.red, true);
        isOpen = false;
        UIActions.instance.ReactToObjectReverse();
    }
    public void IncrementPadNum(int numb)
    {
        if (isFirstPadTry)
        {
            afterfirstPadTryEvent.Invoke();
        }
        else
        {
            aftereveryPadTryEvent.Invoke();
        }
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
        if (isFirstkeyTry)
        {
            afterfirstKeyTryEvent.Invoke();
        }
        else
        {
            aftereveryKeyTryEvent.Invoke();
        }
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

    private void CodeCheckResults(GameObject wheel, int number)
    {
        if (isFirstCodeCheckTry)
        {
            afterfirstCodeCheckTryEvent.Invoke();
        }
        else
        {
            aftereveryCodeCheckTryEvent.Invoke();
        }
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
}
