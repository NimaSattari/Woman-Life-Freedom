using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private float pickUpDistance = 2f;
    [SerializeField] private Transform objectGrabPointTransform;

    private ObjectGrabbable objectGrabbable;
    private ObjectGrabbable objectGrabbable1;

    private void LateUpdate()
    {
        if(objectGrabbable == null)
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast1, pickUpDistance, pickupLayerMask))
            {
                if (raycast1.transform.TryGetComponent(out objectGrabbable1))
                {
                    if (!objectGrabbable1._outline.enabled)
                    {
                        objectGrabbable1.TurnOnOutline(Color.yellow, true);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(objectGrabbable == null)
            {
                //Not carrying objcet try to grab
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast, pickUpDistance, pickupLayerMask))
                {
                    if (raycast.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(objectGrabPointTransform);
                    }
                }
            }
            else
            {
                //object in hand
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }
    }
}
