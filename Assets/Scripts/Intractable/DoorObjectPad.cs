using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(DoTweenActions))]
public class DoorObjectPad : MonoBehaviour
{
    [SerializeField] DoorObject myDoor;
    DoTweenActions myTweenActions;
    BoxCollider myCollider;
    int numb = 0;

    private void Awake()
    {
        myTweenActions = GetComponent<DoTweenActions>();
        myCollider = GetComponent<BoxCollider>();
        myCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Grab" || other.tag == "Player")
        {
            myDoor.IncrementPadNum(1);
            numb++;
            myTweenActions.DoAnimation();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Grab" || other.tag == "Player")
        {
            myDoor.IncrementPadNum(-1);
            numb--;
            if (numb <= 0)
            {
                myTweenActions.DoAnimationBackward();
            }
        }
    }
}
