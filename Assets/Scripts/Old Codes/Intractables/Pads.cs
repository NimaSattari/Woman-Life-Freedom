using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pads : MonoBehaviour
{
    [SerializeField] ObjectOpenClose myDoor;
    [SerializeField] DoTweenActions tweenActions;
    int numb = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Grab" || other.tag == "Player")
        {
            print("1");
            myDoor.IncrementPadNum(1);
            numb++;
            tweenActions.DoAnimation();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Grab" || other.tag == "Player")
        {
            myDoor.IncrementPadNum(-1);
            numb--;
            if(numb <= 0)
            {
                tweenActions.DoAnimationBackward();
            }
        }
    }
}
