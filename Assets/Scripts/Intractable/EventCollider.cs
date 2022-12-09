using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCollider : MonoBehaviour
{
    [SerializeField] string tagToCompare;
    [SerializeField] UnityEvent Event;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == tagToCompare)
        {
            if (other.GetComponent<PickupObject>().grabPointTransform == null)
            {
                Event.Invoke();
            }
        }
    }
}
