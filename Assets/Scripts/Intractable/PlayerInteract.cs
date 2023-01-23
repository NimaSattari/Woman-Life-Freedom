using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    ThirdPersonController thirdPersonController;
    [SerializeField] private Transform playerCameraTransform, objectGrabPointTransform;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask interactableLayer, throwLayer;

    [HideInInspector] public Interactable interactableObjectPicked, interactableObjectSeen;

    bool isLightOn;
    [SerializeField] GameObject canddlelight;
    [SerializeField] GameObject pauseUI;

    [SerializeField] bool isInspecting = false;
    [SerializeField] Camera inspectCam;
    private const string mouseX = "Mouse X";
    private const string mouseY = "Mouse Y";

    private void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    void LateUpdate()
    {
        //Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseUI.activeInHierarchy)
            {
                thirdPersonController.enabled = true;
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                UIActions.instance.DisableMiddlePoint(false);
                pauseUI.SetActive(false);
            }
            else
            {
                thirdPersonController.enabled = false;
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                UIActions.instance.DisableMiddlePoint(true);
                pauseUI.SetActive(true);
            }
        }

        if (!isInspecting)
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
                    if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycast2, interactDistance, interactableLayer))
                    {
                        if (raycast2.transform.TryGetComponent(out DoorObject door))
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

        //Inspect
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (interactableObjectPicked)
            {
                interactableObjectPicked.TryGetComponent<PickupObject>(out PickupObject pickup);
                if (pickup)
                {
                    if (isInspecting)
                    {
                        thirdPersonController.enabled = true;
                        Cursor.lockState = CursorLockMode.Locked;
                        inspectCam.gameObject.SetActive(false);
                        UIActions.instance.DisableMiddlePoint(false);
                        isInspecting = false;
                    }
                    else
                    {
                        thirdPersonController.enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        inspectCam.gameObject.SetActive(true);
                        UIActions.instance.DisableMiddlePoint(true);
                        isInspecting = true;
                    }
                }
            }
        }

        if (isInspecting)
        {
            float h = 5 * Input.GetAxis(mouseX);
            float v = 5 * Input.GetAxis(mouseY);

            if (Input.GetKey(KeyCode.Mouse0))
            {
                interactableObjectPicked.gameObject.transform.Rotate(-v, -h, 0);
            }
        }
    }
}
