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
        if(objectGrabbable == null)
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast1, pickUpDistance, pickupLayerMask))
            {
                if (raycast1.transform.TryGetComponent(out objectGrabbable1))
                {
                    if (!objectGrabbable1._outline.enabled)
                    {
                        objectGrabbable1.TurnOnOutline(Color.yellow, true);
                        UIActions.instance.pickup.SetActive(true);
                        UIActions.instance.nameObject.SetActive(true);
                        UIActions.instance.objectNameText.text = objectGrabbable1.objectName;
                    }
                }
            }
        }
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast2, pickUpDistance, openCloseLayerMask))
        {
            if (raycast2.transform.TryGetComponent(out objectOpenClose1))
            {
                if (!objectOpenClose1._outline.enabled)
                {
                    objectOpenClose1.TurnOnOutline(Color.yellow, true);
                    UIActions.instance.open.SetActive(true);
                    UIActions.instance.nameObject.SetActive(true);
                    UIActions.instance.objectNameText.text = objectOpenClose1.objectName;
                }
            }
        }
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast3, pickUpDistance, codeLayerMask))
        {
            if (raycast3.transform.TryGetComponent(out objectCode1))
            {
                if (!objectCode1._outline.enabled)
                {
                    objectCode1.TurnOnOutline(Color.yellow, true);
                    UIActions.instance.change.SetActive(true);
                    UIActions.instance.nameObject.SetActive(true);
                    UIActions.instance.storyObject.SetActive(true);
                    UIActions.instance.objectNameText.text = objectCode1.objectName;
                    UIActions.instance.objectStoryText.text = objectCode1.story;
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
                        if (!string.IsNullOrEmpty(objectGrabbable.story))
                        {
                            UIActions.instance.storyObject.SetActive(true);
                            UIActions.instance.objectStoryText.text = objectGrabbable.story;
                        }
                    }
                }
            }
            else
            {
                //object in hand
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast1, pickUpDistance, openCloseLayerMask))
            {
                if (raycast1.transform.TryGetComponent(out objectOpenClose))
                {
                    objectOpenClose.Handle();
                }
            }
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast4, pickUpDistance, codeLayerMask))
            {
                if (raycast4.transform.TryGetComponent(out objectCode))
                {
                    objectCode.Handle();
                }
            }
        }
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
    }
}
