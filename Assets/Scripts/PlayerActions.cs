using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickupLayerMask, throwLayerMask, openCloseLayerMask, codeLayerMask;
    [SerializeField] private float pickUpDistance = 2f;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private GameObject canddlelight;

    private ObjectGrabbable objectGrabbable;
    private ObjectGrabbable objectGrabbable1;

    private ObjectOpenClose objectOpenClose;
    private ObjectOpenClose objectOpenClose1;

    private Rotate objectCode;
    private Rotate objectCode1;

    private bool isLightOn = false;

    private void LateUpdate()
    {
        //Grab Object Seen
        if(objectGrabbable == null)
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast1, pickUpDistance, pickupLayerMask))
            {
                if (raycast1.transform.TryGetComponent(out objectGrabbable1))
                {
                    if (!objectGrabbable1._outline.enabled)
                    {
                        objectGrabbable1.TurnOnOutline(Color.yellow, true);
                        UIActions.instance.ReactToObjectSeen(objectGrabbable1.objectName);
                    }
                }
            }
        }

        //Door Seen
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast2, pickUpDistance, openCloseLayerMask))
        {
            if (raycast2.transform.TryGetComponent(out objectOpenClose1))
            {
                if (!objectOpenClose1._outline.enabled)
                {
                    objectOpenClose1.TurnOnOutline(Color.yellow, true);
                    UIActions.instance.ReactToObjectSeen(objectOpenClose1.objectName);
                }
            }
        }

        //Code Enter Seen
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast3, pickUpDistance, codeLayerMask))
        {
            if (raycast3.transform.TryGetComponent(out objectCode1))
            {
                if (!objectCode1._outline.enabled)
                {
                    objectCode1.TurnOnOutline(Color.yellow, true);
                    UIActions.instance.ReactToObjectPick(objectCode1.objectName,objectCode1.story);
                }
            }
        }

        //Pressed E
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Grab Object PickUp
            if (objectGrabbable == null)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast, pickUpDistance, pickupLayerMask))
                {
                    if (raycast.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(objectGrabPointTransform);
                        UIActions.instance.ReactToObjectPick(objectGrabbable.objectName, objectGrabbable.story);
                    }
                }
            }

            //Grab Object Drop
            else
            {
                objectGrabbable.Drop();
                objectGrabbable = null;
            }

            //Door Open Close
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast1, pickUpDistance, openCloseLayerMask))
            {
                if (raycast1.transform.TryGetComponent(out objectOpenClose))
                {
                    objectOpenClose.Handle();
                }
            }

            //Code Enter Intract
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast4, pickUpDistance, codeLayerMask))
            {
                if (raycast4.transform.TryGetComponent(out objectCode))
                {
                    objectCode.Handle();
                }
            }
        }

        //Grab Object Throw
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(objectGrabbable != null)
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, 500f, throwLayerMask))
                {
                    objectGrabbable.Throw((hit.point - objectGrabPointTransform.position).normalized);
                    objectGrabbable = null;
                }
            }
        }

        //Light TurnOffOn
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (isLightOn)
            {
                canddlelight.SetActive(false);
                isLightOn = false;
            }
            else
            {
                canddlelight.SetActive(true);
                isLightOn = true;
            }
        }

        //Left Mouse On Locked Door
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(objectGrabbable != null)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast5, pickUpDistance, openCloseLayerMask))
                {
                    if (raycast5.transform.TryGetComponent(out objectOpenClose))
                    {
                        if (objectOpenClose.TryOpenWithKey(objectGrabbable.gameObject))
                        {
                            objectGrabbable.Drop();
                            objectGrabbable = null;
                        }
                    }
                }
            }
        }
    }
}
