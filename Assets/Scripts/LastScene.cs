using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastScene : MonoBehaviour
{
    [SerializeField] GameObject phone1, phone, balcony, balcony1, door;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Letter")
        {
            if(other.GetComponent<ObjectGrabbable>().grabPointTransform == null)
            {
                phone1.SetActive(false);
                balcony1.SetActive(false);
                phone.SetActive(true);
                balcony.SetActive(true);
                door.GetComponent<ObjectOpenClose>().Close();
            }
        }
    }
}
