using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastScene : MonoBehaviour
{
    [SerializeField] GameObject phone1, phone, EndDoor, dumbDoor, safeDoor, letter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Letter")
        {
            if(other.GetComponent<ObjectGrabbable>().grabPointTransform == null)
            {
                phone1.SetActive(false);
                dumbDoor.SetActive(false);
                phone.SetActive(true);
                EndDoor.SetActive(true);
                safeDoor.GetComponent<ObjectOpenClose>().Close();
                letter.SetActive(false);
            }
        }
    }
}
