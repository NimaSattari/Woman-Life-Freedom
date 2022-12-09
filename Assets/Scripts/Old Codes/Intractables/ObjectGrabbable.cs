using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    [SerializeField] private float lerpSpeed = 10f;
    [SerializeField] public string objectName;
    [SerializeField] public string story;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;
    public Outline _outline { get { return outline; } }

    public Transform grabPointTransform;
    private Rigidbody objectRigidbody;
    private Outline outline;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
    }

    private void FixedUpdate()
    {
        if (grabPointTransform != null)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, grabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPos);
        }
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        if (objectName == "Phone")
        {
            StoryScript.instance.AfterPhone1();
        }
        if(tag == "Letter")
        {
            StartCoroutine(StoryScript.instance.TurnOnObjectAtTime(StoryScript.instance.letter, 3f));
            StartCoroutine(StoryScript.instance.TurnOffObjectAtTime(StoryScript.instance.letter, 10f));
        }
        this.grabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        objectRigidbody.isKinematic = true;
        TurnOnOutline(Color.green, false);
        SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.grab);
    }

    public void Drop()
    {
        this.grabPointTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.isKinematic = false;
        TurnOnOutline(Color.red, true);
        UIActions.instance.ReactToObjectReverse();
        SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.throwsound);
    }

    public void Throw(Vector3 forceDirection)
    {
        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        this.grabPointTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.isKinematic = false;
        TurnOnOutline(Color.red, true);

        objectRigidbody.AddForce(forceToAdd, ForceMode.Impulse);
        UIActions.instance.ReactToObjectReverse();
        SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.throwsound);
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
