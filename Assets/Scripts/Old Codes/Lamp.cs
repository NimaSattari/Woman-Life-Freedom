using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] Outline outline;

    public IEnumerator enumerator()
    {
        yield return new WaitForSeconds(1f);
        outline.enabled = false;
    }
}
