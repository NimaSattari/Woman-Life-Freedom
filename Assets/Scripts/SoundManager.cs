using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip codeshuffle1, codeshuffle2, codesolve, doorclose, dooropen, doorlocked, lighter, grab, throwsound, phone;

    public AudioSource audioS;

    #region Singleton
    [SerializeField] public static SoundManager instance;
    private void OnEnable()
    {
        if (SoundManager.instance == null)
        {
            SoundManager.instance = this;
        }
        else
        {
            if (SoundManager.instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion


    public void VolumeButton(float change)
    {
        audioS.volume += change;
    }
}
