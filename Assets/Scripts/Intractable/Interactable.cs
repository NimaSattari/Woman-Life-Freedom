using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour
{
    //Public
    public Outline _outline { get { return outline; } }
    public string[] objectName;
    public string[] objectStory;
    //Private
    private Outline outline;
    [SerializeField] UnityEvent OnLeftClickOn, OnLeftClickOff, OnRightClickOn;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        this.gameObject.layer = LayerMask.NameToLayer("Interactable");
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

    public virtual void LeftClickOn()
    {
        OnLeftClickOn.Invoke();
    }
    public virtual void LeftClickOff()
    {
        OnLeftClickOff.Invoke();
    }
    public virtual void RightClickOn(Vector3 forceDirection)
    {
        OnRightClickOn.Invoke();
    }
}
