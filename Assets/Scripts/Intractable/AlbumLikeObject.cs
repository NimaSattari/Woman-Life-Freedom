using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AlbumLikeObject : Interactable
{
    [Header("If has pictures")]
    [SerializeField] public bool hasPicture;
    [SerializeField] public Sprite[] sprites;

    [Header("If has audios")]
    [SerializeField] public bool hasAudio;
    [SerializeField] public AudioClip[] audios;

    [HideInInspector] public bool isSelected;
    [HideInInspector] public int whichnumb = -1;

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
            if (hasAudio)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    SoundManager.instance.VolumeButton(+0.1f);
                }
                if (Input.GetKeyDown(KeyCode.T))
                {
                    SoundManager.instance.VolumeButton(-0.1f);
                }
            }
            if (hasPicture)
            {

            }
        }
    }

    private void Next()
    {
        whichnumb++;
        if (whichnumb >= sprites.Length && whichnumb >= audios.Length)
        {
            whichnumb = 0;
        }
        if (hasPicture)
        {
            UIActions.instance.ChangeStorySprite(sprites[whichnumb]);
        }
        if (hasAudio)
        {
            SoundManager.instance.audioS.clip = audios[whichnumb];
            SoundManager.instance.audioS.Play();
            if (whichnumb == 2)
            {
                StartCoroutine(StoryScript.instance.TurnOnObjectAtTime(StoryScript.instance.toomaj, 3f));
                StartCoroutine(StoryScript.instance.TurnOffObjectAtTime(StoryScript.instance.toomaj, 8f));
            }
        }
        UIActions.instance.ReactToObjectPick(objectName[whichnumb], objectStory[whichnumb]);
    }
    private void Before()
    {
        whichnumb--;
        if (hasPicture)
        {
            if (whichnumb < 0)
            {
                whichnumb = sprites.Length - 1;
            }
            UIActions.instance.ChangeStorySprite(sprites[whichnumb]);
        }
        if (hasAudio)
        {
            if (whichnumb < 0)
            {
                whichnumb = audios.Length - 1;
            }
            SoundManager.instance.audioS.clip = audios[whichnumb];
            SoundManager.instance.audioS.Play();
            if (whichnumb == 2)
            {
                StartCoroutine(StoryScript.instance.TurnOnObjectAtTime(StoryScript.instance.toomaj, 1f));
                StartCoroutine(StoryScript.instance.TurnOffObjectAtTime(StoryScript.instance.toomaj, 10f));
            }
        }
        UIActions.instance.ReactToObjectPick(objectName[whichnumb], objectStory[whichnumb]);
    }

    public override void LeftClickOn()
    {
        base.LeftClickOn();
        isSelected = true;
        if (hasPicture)
        {
            UIActions.instance.PictureActive(true);
        }
        if(hasAudio)
        {
            UIActions.instance.AudioActive(true);
        }
        Next();
        TurnOnOutline(Color.green, false);
    }
    public override void LeftClickOff()
    {
        base.LeftClickOff();
        isSelected = false;
        if (hasPicture)
        {
            UIActions.instance.PictureActive(false);
        }
        if (hasAudio)
        {
            UIActions.instance.AudioActive(false);
        }
        TurnOnOutline(Color.red, true);
        UIActions.instance.ReactToObjectReverse();
    }
}
