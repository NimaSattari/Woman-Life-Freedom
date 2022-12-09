using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PickupObject : Interactable
{
    [Header("Grab")]
    [SerializeField]
    public float lerpSpeed = 20f;
    public Transform grabPointTransform;
    Rigidbody objectRigidbody;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;

    private void Start()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (grabPointTransform != null)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, grabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPos);
        }
    }

    public override void LeftClickOn()
    {
        base.LeftClickOn();
        this.grabPointTransform = GameObject.FindGameObjectWithTag("GrabPoint").transform;
        objectRigidbody.useGravity = false;
        objectRigidbody.isKinematic = true;
        TurnOnOutline(Color.green, false);
        SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.grab);
    }
    public override void LeftClickOff()
    {
        base.LeftClickOff();
        this.grabPointTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.isKinematic = false;
        TurnOnOutline(Color.red, true);
        UIActions.instance.ReactToObjectReverse();
        SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.throwsound);
    }
    public override void RightClickOn(Vector3 forceDirection)
    {
        base.RightClickOn(forceDirection);
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        this.grabPointTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.isKinematic = false;
        TurnOnOutline(Color.red, true);

        objectRigidbody.AddForce(forceToAdd, ForceMode.Impulse);
        UIActions.instance.ReactToObjectReverse();
        SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.throwsound);
    }
}
