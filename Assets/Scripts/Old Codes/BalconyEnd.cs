using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BalconyEnd : MonoBehaviour
{
    public string objectname;
    public Outline _outline { get { return outline; } }
    private Outline outline;


    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
    }

    public void TurnOnOutline(Color color, bool turnoff)
    {
        outline.enabled = true;
        outline.OutlineColor = color;
        if (turnoff)
        {
            Invoke("TurnOffOutline", 1f);
        }
        else
        {
            CancelInvoke();
        }
    }

    public void TurnOffOutline()
    {
        outline.enabled = false;
        UIActions.instance.ReactToObjectReverse();
    }

    public void End()
    {
        SceneManager.LoadScene(2);
    }
}
