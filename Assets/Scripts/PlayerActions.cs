using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickupLayerMask, throwLayerMask, openCloseLayerMask, codeLayerMask, elctroLayerMask, lampLayerMask, balcony;
    [SerializeField] private float pickUpDistance = 2f;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private GameObject canddlelight;

    private ObjectGrabbable objectGrabbable;
    private ObjectGrabbable objectGrabbable1;

    private ObjectOpenClose objectOpenClose;
    private ObjectOpenClose objectOpenClose1;

    private Rotate objectCode;
    private Rotate objectCode1;

    private Electronic electronic;
    private Electronic electronic1;

    private bool isLightOn = false;

    [SerializeField] GameObject lamp;
    [SerializeField] bool isLampOn = false;

    [SerializeField] GameObject pauseUI;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
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

        //Grab Object Seen
        if (objectGrabbable == null)
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

        //Electronic Seen
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast6, pickUpDistance, elctroLayerMask))
        {
            if (raycast6.transform.TryGetComponent(out electronic1))
            {
                if (!electronic1._outline.enabled)
                {
                    electronic1.TurnOnOutline(Color.yellow, true);
                    UIActions.instance.ReactToObjectSeen(electronic1.objectName);
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

        //Lamp Seen
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast8, pickUpDistance, lampLayerMask))
        {
            if (raycast8.transform.TryGetComponent(out Outline outline))
            {
                outline.enabled = true;
                StartCoroutine(raycast8.transform.GetComponent<Lamp>().enumerator());
            }
        }

        //Pressed Mouse 0
        if (Input.GetKeyDown(KeyCode.Mouse0))
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
                if(!Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast10, pickUpDistance, openCloseLayerMask))
                {
                    objectGrabbable.Drop();
                    objectGrabbable = null;
                }
            }

            //Electo Object PickUp
            if (electronic == null)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast7, pickUpDistance, elctroLayerMask))
                {
                    if (raycast7.transform.TryGetComponent(out electronic))
                    {
                        electronic.Grab();
                        //UIActions.instance.ReactToObjectPick(electronic.objectName, electronic.story);
                    }
                }
            }

            //Electro Object PickUp
            else
            {
                electronic.Drop();
                electronic = null;
            }

            //Door Open Close
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast1, pickUpDistance, openCloseLayerMask))
            {
                if (raycast1.transform.TryGetComponent(out objectOpenClose))
                {
                    if (!objectOpenClose.isUnlocked)
                    {
                        if(objectGrabbable != null)
                        {
                            if (objectOpenClose.TryOpenWithKey(objectGrabbable.gameObject))
                            {
                                objectGrabbable.Drop();
                                objectGrabbable = null;
                            }
                        }
                        else
                        {
                            objectOpenClose.Handle();
                        }
                    }
                    else
                    {
                        objectOpenClose.Handle();
                    }
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

            //Lamp On Off
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast9, pickUpDistance, lampLayerMask))
            {
                if (isLampOn)
                {
                    isLampOn = false;
                    lamp.SetActive(false);
                }
                else
                {
                    isLampOn = true;
                    lamp.SetActive(true);
                }
            }
        }

        //Grab Object Throw
        if (Input.GetKeyDown(KeyCode.Mouse1))
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isLightOn)
            {
                canddlelight.SetActive(false);
                isLightOn = false;
            }
            else
            {
                SoundManager.instance.audioS.PlayOneShot(SoundManager.instance.lighter);
                canddlelight.SetActive(true);
                isLightOn = true;
            }
        }

        //Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseUI.activeInHierarchy)
            {
                Time.timeScale = 1;
                pauseUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Time.timeScale = 0;
                pauseUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }


        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast12, pickUpDistance, balcony))
        {
            if (raycast12.transform.TryGetComponent(out BalconyEnd balconyEnd))
            {
                if (!balconyEnd._outline.enabled)
                {
                    balconyEnd.TurnOnOutline(Color.yellow, true);
                    UIActions.instance.ReactToObjectSeen(balconyEnd.objectname);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast13, pickUpDistance, balcony))
            {
                if (raycast13.transform.TryGetComponent(out BalconyEnd balconyEnd))
                {
                    balconyEnd.End();
                }
            }
        }

    }
}
