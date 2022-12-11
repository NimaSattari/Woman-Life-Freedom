using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform, objectGrabPointTransform;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask interactableLayer, throwLayer;

    [HideInInspector] public Interactable interactableObjectPicked, interactableObjectSeen;

    bool isLightOn;
    [SerializeField] GameObject canddlelight;
    [SerializeField] GameObject pauseUI;

    void LateUpdate()
    {
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast0, interactDistance, interactableLayer))
        {
            if (raycast0.transform.TryGetComponent(out interactableObjectSeen))
            {
                if (!interactableObjectSeen._outline.enabled)
                {
                    interactableObjectSeen.TurnOnOutline(Color.yellow, true);
                    UIActions.instance.ReactToObjectSeen(interactableObjectSeen.objectName[0]);
                }
            }
        }

        //Pressed Mouse 0
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Grab Object PickUp
            if (interactableObjectPicked == null)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast1, interactDistance, interactableLayer))
                {
                    if (raycast1.transform.TryGetComponent(out Interactable interactableObjectNow))
                    {
                        if (raycast1.transform.TryGetComponent(out PickupObject pickupObject))
                        {
                            interactableObjectPicked = interactableObjectNow;
                        }
                        if (raycast1.transform.TryGetComponent(out AlbumLikeObject albumObject))
                        {
                            interactableObjectPicked = interactableObjectNow;
                        }
                        UIActions.instance.ReactToObjectPick(interactableObjectNow.objectName[0], interactableObjectNow.objectStory[0]);
                        interactableObjectNow.LeftClickOn();
                    }
                }
            }

            //Grab Object Drop
            else
            {
                if(Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast2, interactDistance, interactableLayer))
                {
                    if(raycast2.transform.TryGetComponent(out DoorObject door))
                    {
                        if (!door.isUnlocked)
                        {
                            if (interactableObjectPicked != null)
                            {
                                if (door.TryOpenWithKey(interactableObjectPicked.gameObject))
                                {
                                    interactableObjectPicked.LeftClickOff();
                                    interactableObjectPicked = null;
                                }
                            }
                            else
                            {
                                door.LeftClickOn();
                            }
                        }
                        else
                        {
                            door.LeftClickOn();
                        }
                    }
                    else
                    {
                        interactableObjectPicked.LeftClickOff();
                        interactableObjectPicked = null;
                    }
                }
                else
                {
                    interactableObjectPicked.LeftClickOff();
                    interactableObjectPicked = null;
                }
            }
        }

        //Pressed Mouse 1
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (interactableObjectPicked != null)
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, 500f, throwLayer))
                {
                    interactableObjectPicked.RightClickOn((hit.point - objectGrabPointTransform.position).normalized);
                    interactableObjectPicked = null;
                }
            }
        }

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
                pauseUI.SetActive(false);
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                pauseUI.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
