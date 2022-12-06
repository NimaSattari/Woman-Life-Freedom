using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Electronic : MonoBehaviour
{
    [SerializeField] public string objectName;
    [SerializeField] public string[] names;
    [SerializeField] public string story;
    [SerializeField] public bool islaptop;
    [SerializeField] public bool isSelected;
    [SerializeField] public Sprite[] sprites;
    [SerializeField] public AudioClip[] audios;
    [SerializeField] Image laptopImageUi;
    [SerializeField] GameObject[] directionsUIs;
    [SerializeField] int whichnumb = -1;

    public Outline _outline { get { return outline; } }

    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void Update()
    {
        if (isSelected)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Before();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                Next();
            }
        }
    }

    private void Next()
    {
        whichnumb++;
        if(whichnumb >= sprites.Length && whichnumb >= audios.Length)
        {
            whichnumb = 0;
        }
        if (islaptop)
        {
            laptopImageUi.sprite = sprites[whichnumb];
        }
        else
        {
            SoundManager.instance.audioS.clip = audios[whichnumb];
            SoundManager.instance.audioS.Play();
            if (whichnumb == 2)
            {
                StartCoroutine(StoryScript.instance.TurnOnObjectAtTime(StoryScript.instance.toomaj, 3f));
                StartCoroutine(StoryScript.instance.TurnOnObjectAtTime(StoryScript.instance.toomaj, 8f));
            }
        }
        UIActions.instance.ReactToObjectPick(names[whichnumb], story);
    }
    private void Before()
    {
        whichnumb--;
        if (islaptop)
        {
            if (whichnumb < 0)
            {
                whichnumb = sprites.Length;
            }
            laptopImageUi.sprite = sprites[whichnumb];
        }
        else
        {
            if (whichnumb < 0)
            {
                whichnumb = audios.Length;
            }
            SoundManager.instance.audioS.clip = audios[whichnumb];
            SoundManager.instance.audioS.Play();
            if (whichnumb == 2)
            {
                StartCoroutine(StoryScript.instance.TurnOnObjectAtTime(StoryScript.instance.toomaj, 1f));
                StartCoroutine(StoryScript.instance.TurnOnObjectAtTime(StoryScript.instance.toomaj, 10f));
            }
        }
        UIActions.instance.ReactToObjectPick(names[whichnumb], story);
    }

    public void Grab()
    {
        isSelected = true;
        if (islaptop)
        {
            laptopImageUi.enabled = true;
        }
        foreach (GameObject gameObject in directionsUIs)
        {
            gameObject.SetActive(true);
        }
        Next();
        TurnOnOutline(Color.green, false);
    }

    public void Drop()
    {
        isSelected = false;
        if (islaptop)
        {
            laptopImageUi.enabled = false;
        }
        foreach (GameObject gameObject in directionsUIs)
        {
            gameObject.SetActive(false);
        }
        TurnOnOutline(Color.red, true);
        UIActions.instance.ReactToObjectReverse();
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
}
