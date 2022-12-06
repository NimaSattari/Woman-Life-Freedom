using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScript : MonoBehaviour
{
    [SerializeField] public
    GameObject move, line1, line2, pick, throwIt, lighter, whereisLight, whereisKeyLight, toomaj, phone, music, mommy;

    [SerializeField] GameObject[] beforePhoneObjects;
    [SerializeField] GameObject[] afterPhoneObjects;

    #region Singleton
    [SerializeField] public static StoryScript instance;
    private void OnEnable()
    {
        if (StoryScript.instance == null)
        {
            StoryScript.instance = this;
        }
        else
        {
            if (StoryScript.instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    private void Start()
    {
        StartCoroutine(TurnOnObjectAtTime(move, 2f));
        StartCoroutine(TurnOffObjectAtTime(move, 4f));
        StartCoroutine(TurnOnObjectAtTime(whereisLight, 5f));
        StartCoroutine(TurnOffObjectAtTime(whereisLight, 8f));
        StartCoroutine(TurnOnObjectAtTime(lighter, 9f));
        StartCoroutine(TurnOffObjectAtTime(lighter, 11f));
        StartCoroutine(TurnOnObjectAtTime(line1, 13f));
        StartCoroutine(TurnOffObjectAtTime(line1, 18));
        StartCoroutine(TurnOnObjectAtTime(line2, 19));
        StartCoroutine(TurnOffObjectAtTime(line2, 24));
        StartCoroutine(TurnOnObjectAtTime(pick, 25));
        StartCoroutine(TurnOffObjectAtTime(pick, 28));
        StartCoroutine(TurnOnObjectAtTime(throwIt, 29));
        StartCoroutine(TurnOffObjectAtTime(throwIt, 31));
        StartCoroutine(TurnOnObjectAtTime(whereisKeyLight, 32));
        StartCoroutine(TurnOffObjectAtTime(whereisKeyLight, 36));

        StartCoroutine(TurnOnObjectAtTime(phone, 37));
    }

    public IEnumerator TurnOffObjectAtTime(GameObject game, float timer)
    {
        yield return new WaitForSeconds(timer);
        game.SetActive(false);
    }
    public IEnumerator TurnOnObjectAtTime(GameObject game, float timer)
    {
        yield return new WaitForSeconds(timer);
        game.SetActive(true);
        if(game == phone)
        {
            music.GetComponent<AudioSource>().Stop();
        }
    }

    public void AfterPhone1()
    {
        foreach (GameObject gameObject in beforePhoneObjects)
        {
            gameObject.SetActive(false);
        }
        foreach (GameObject gameObject in afterPhoneObjects)
        {
            gameObject.SetActive(true);
        }
        StartCoroutine(TurnOnObjectAtTime(mommy, 1));
        StartCoroutine(TurnOffObjectAtTime(mommy, 8));
    }
}
